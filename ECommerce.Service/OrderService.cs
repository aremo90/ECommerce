using AutoMapper;
using ECommerce.Domin.Contract;
using ECommerce.Domin.Models.BasketModule;
using ECommerce.Domin.Models.OrderModule;
using ECommerce.Domin.Models.ProudctModule;
using ECommerce.Service.Specification;
using ECommerce.ServiceAbstractions;
using ECommerce.Shared.CommonResult;
using ECommerce.Shared.DTOS.OrderDTOS;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service
{
    public class OrderService : IOrderService
    {
        #region CLR
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IBasketRepository _basketRepository;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, IBasketRepository basketRepository , ILogger<OrderService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _basketRepository = basketRepository;
            _logger = logger;
        } 
        #endregion

        public async Task<Result<OrderToReturnDTO>> CreateOrderAsync(OrderDTO orderDTO, string email)
        {
            // Step 1 :-
            // Map From AddressDTO to Address
            var OrderAddress = _mapper.Map<OrderAddress>(orderDTO.Address);

            // Step 2 :-
            // Retrieves Basket and check items in it
            var Basket = await _basketRepository.GetBasketAsync(orderDTO.BasketId);
            if (Basket is null)
                return Error.NotFound("Basket Not Found");

            // Check PaymentIntent
            ArgumentNullException.ThrowIfNullOrEmpty(Basket.PaymentIntentId);
            var OrderRepo = _unitOfWork.GetRepositoryAsync<Order, Guid>();


            var Spec = new OrderWithPaymentIntentSpec(Basket.PaymentIntentId);
            var ExistOrder = await OrderRepo.GetByIdAsync(Spec);
            if (ExistOrder is not null) OrderRepo.DeleteAsync(ExistOrder);




            // Step 3 :-
            // If Basket is not null => Products in basket => List<OrderItem>
            List<OrderItem> OrderItems = new List<OrderItem>();
            foreach (var item in Basket.Items)
            {
                var Product = await _unitOfWork.GetRepositoryAsync<Product, int>().GetByIdAsync(item.Id);
                if (Product is null)
                    return Error.NotFound("Product Not Found");
                OrderItems.Add(CreateOrderItem(item, Product));
            }

            // Step 4 :-
            // Get Delivery Method
            var DeliveryMethod = await _unitOfWork.GetRepositoryAsync<DeliveryMethod, int>().GetByIdAsync(orderDTO.DeliveryMethodId);
            if (DeliveryMethod is null)
                return Error.NotFound("DeliveryMethod not Found");

            // Step 5 :-
            // subTotal
            var SubTotal = OrderItems.Sum(I => I.Price * I.Quantity);

            // Step 6 :-
            // Create Order
            var Order = new Order()
            {
                Address = OrderAddress,
                DeliveryMethod = DeliveryMethod,
                Items = OrderItems,
                SubTotal = SubTotal,
                UserEmail = email,
                PaymentIntentId = Basket.PaymentIntentId,
            };

            // Step 7 :-
            // Save Order in DataBase
            await _unitOfWork.GetRepositoryAsync<Order, Guid>().AddAsync(Order);
            int Result = await _unitOfWork.SaveChangesAsync();
            if (Result == 0)
                return Error.Failure("Error Completing Order Please Try Again Later");

            // Step 8 :-
            return _mapper.Map<OrderToReturnDTO>(Order);
        }

        private static OrderItem CreateOrderItem(BasketItem item, Product Product)
        {
            return new OrderItem()
            {
                Product = new ProductItemOredered()
                {
                    ProductId = Product.Id,
                    ProductName = Product.Name,
                    PictureUrl = Product.PictureUrl,
                },
                Price = Product.Price,
                Quantity = item.Quantity
            };
        }

        public async Task<Result<IEnumerable<OrderToReturnDTO>>> GetAllOrdersAsync(string email)
        {
            var Spec = new OrderSpecification(email);
            var Orders = await _unitOfWork.GetRepositoryAsync<Order , Guid>().GetAllAsync(Spec);

            if (Orders is null)
                return Error.NotFound("Order Not found");

            var Data = _mapper.Map<IEnumerable<OrderToReturnDTO>>(Orders);
            return Result<IEnumerable<OrderToReturnDTO>>.Ok(Data);
        }

        public async Task<Result<IEnumerable<DeliveryMethodDTO>>> GetDeliveMethodsAsync()
        {
            var DeliveryMethod = await _unitOfWork.GetRepositoryAsync<DeliveryMethod, int>().GetAllAsync();
            if (!DeliveryMethod.Any())
                return Error.NotFound("No Delivery Method Found");
            var Data = _mapper.Map<IEnumerable<DeliveryMethod> , IEnumerable<DeliveryMethodDTO>>(DeliveryMethod);
            return Result<IEnumerable<DeliveryMethodDTO>>.Ok(Data);
        }

        public async Task<Result<OrderToReturnDTO>> GetOrderByIdAsync(Guid id, string Email)
        {
            var Spec = new OrderSpecification( id, Email);
            var Order = await _unitOfWork.GetRepositoryAsync<Order , Guid>().GetByIdAsync( Spec);
            if (Order is null)
                return Error.NotFound("Order Not Found");
            return _mapper.Map<OrderToReturnDTO>(Order);
        }
    }
}
