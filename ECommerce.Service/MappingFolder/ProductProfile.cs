using AutoMapper;
using ECommerce.Domin.Models.ProudctModule;
using ECommerce.Shared.DTOS.ProductDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service.MappingFolder
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductBrand, BrandDTO>();
            CreateMap<ProductType, TypeDTO>();
            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.ProductBrand , opt => opt.MapFrom(src => src.ProductBrands.Name))
                .ForMember(dest => dest.ProductType , opt => opt.MapFrom(src => src.ProductTypes.Name));
        }
    }
}
