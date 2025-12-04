using AutoMapper;
using ECommerce.Domin.Models.OrderModule;
using ECommerce.Shared.DTOS.OrderDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service.MappingFolder
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<AddressDTO , OrderAddress>().ReverseMap();

            CreateMap<Order, OrderToReturnDTO>()
                .ForMember(D => D.DeliveryMethod , O => O.MapFrom(S => S.DeliveryMethod.ShortName)).ReverseMap();

            CreateMap<OrderItem , OrderItemDTO>()
                .ForMember(D => D.ProductName , O => O.MapFrom(S => S.Product.ProductName))
                .ForMember(D => D.PictureUrl , O => O.MapFrom(S => S.Product.PictureUrl))
                .ReverseMap();

            CreateMap<DeliveryMethod, DeliveryMethodDTO>();
        }
    }
}
