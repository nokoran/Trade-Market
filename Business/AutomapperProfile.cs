using AutoMapper;
using Business.Models;
using Data.Entities;
using System.Linq;

namespace Business
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Receipt, ReceiptModel>()
                .ForMember(rm => rm.ReceiptDetailsIds, r => r.MapFrom(x => x.ReceiptDetails.Select(rd => rd.Id)))
                .ReverseMap();

            CreateMap<Product, ProductModel>()
                .ForMember(pm => pm.ReceiptDetailIds,
                    p => p.MapFrom(x => x.ReceiptDetails.Select(rd => rd.Id)))
                .ForMember(pm => pm.CategoryName, p => p.MapFrom(x => x.Category.CategoryName))
                .ForMember(pm => pm.ProductCategoryId, p => p.MapFrom(x => x.ProductCategoryId));

            CreateMap<ProductModel, Product>();
            
            CreateMap<ReceiptDetail, ReceiptDetailModel>()
                .ForMember(rdm => rdm.Id,
                    r => r.MapFrom(x => x.Id))
                .ForMember(rdm => rdm.ProductId,
                    r => r.MapFrom(x => x.ProductId))
                .ForMember(rdm => rdm.ReceiptId,
                    r => r.MapFrom(x => x.ReceiptId))
                .ReverseMap();
            
            CreateMap<Customer, CustomerModel>()
                .ForMember(t => t.ReceiptsIds, t => t.MapFrom(x => x.Receipts.Select(m => m.Id)))
                .IncludeMembers(t => t.Person)
                .ReverseMap();

            CreateMap<Person, CustomerModel>().ReverseMap();
            
            CreateMap<ProductCategory, ProductCategoryModel>()
                .ForMember(pcm => pcm.ProductIds,
                    p => p.MapFrom(x => x.Products.Select(pr => pr.Id)))
                .ForMember(pcm => pcm.Id,
                    p => p.MapFrom(x => x.Id))
                .ReverseMap();
        }
    }
}