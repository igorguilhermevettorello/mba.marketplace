using AutoMapper;
using MBA.Marketplace.Core.Entities;
using MBA.Marketplace.Web.ViewModels;


namespace MBA.Marketplace.Web.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProdutoViewModel, ProdutoFormViewModel>()
                .ForMember(dest => dest.Imagem, opt => opt.Ignore());

            CreateMap<ProdutoFormViewModel, ProdutoViewModel>()
                .ForMember(dest => dest.Imagem, opt => opt.Ignore());

            CreateMap<Categoria, CategoriaViewModel>();

            CreateMap<Categoria, CategoriaFormViewModel>();
        }
    }
}
