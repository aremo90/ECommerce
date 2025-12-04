using AutoMapper;
using ECommerce.Domin.Contract;
using ECommerce.Domin.Models.BasketModule;
using ECommerce.Domin.Models.OrderModule;
using ECommerce.Service.Exceptions;
using ECommerce.ServiceAbstractions;
using ECommerce.Shared.CommonResult;
using ECommerce.Shared.DTOS.BasketDTOS;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration configuration;

        public PaymentService(IBasketRepository basketRepository, IMapper mapper, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            this.configuration = configuration;
        }


        public async Task<BasketDTO> CreateOrUpdatePaymentIntnetAsync(string BasketId)
        {
            StripeConfiguration.ApiKey = configuration["Stripe:SecretKey"];


            var Basket = await _basketRepository.GetBasketAsync(BasketId);
            if (Basket is null)
                throw new BasketNotFoundException(BasketId);

            var Product = _unitOfWork.GetRepositoryAsync<Domin.Models.ProudctModule.Product, int>();
            foreach (var Item in Basket.Items)
            {
                var product = await Product.GetByIdAsync(Item.Id) ?? throw new ProductNotFoundException(Item.Id);
                Item.Price = product.Price;
            }
            // DeliveryMethod
            var DeliveryMethod = await _unitOfWork.GetRepositoryAsync<DeliveryMethod, int>().GetByIdAsync(Basket.DeliveryMethodId.Value);

            Basket.ShippingPrice = DeliveryMethod.Price;

            var BasketAmount = (long)(Basket.Items.Sum(item => item.Quantity * item.Price) + DeliveryMethod.Price) * 100;

            // Create PaymentIntent
            var PaymentService = new PaymentIntentService();
            if (Basket.PaymentIntentId is null)
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = BasketAmount,
                    Currency = "USD",
                    PaymentMethodTypes = ["card"]
                };
                var PaymentIntent = await PaymentService.CreateAsync(options);
                Basket.PaymentIntentId = PaymentIntent.Id;
                Basket.ClientSecret = PaymentIntent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = BasketAmount,
                };
                await PaymentService.UpdateAsync(Basket.PaymentIntentId, options);
            }

            await _basketRepository.CreateOrUpdateBasketAsync(Basket);
            // Mapping
            return _mapper.Map<CustomerBasket, BasketDTO>(Basket);

        }
    }
}
