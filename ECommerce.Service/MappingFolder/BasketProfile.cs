using AutoMapper;
using ECommerce.Domin.Models.BasketModule;
using ECommerce.Domin.Models.ProudctModule;
using ECommerce.Shared.DTOS.BasketDTOS;
using ECommerce.Shared.DTOS.ProductDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service.MappingFolder
{
    public class BasketProfile : Profile
    {
        public BasketProfile()
        {
            CreateMap<CustomerBasket, BasketDTO>().ReverseMap();
            CreateMap<BasketItem, BasketItemDTO>().ReverseMap();
            
        }
    }
}
