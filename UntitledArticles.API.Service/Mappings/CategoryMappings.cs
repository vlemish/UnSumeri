using AutoMapper;

using UntitiledArticles.API.Application.Categories.Queries;

using UntitledArticles.API.Domain.Entities;

namespace UntitledArticles.API.Service.Mappings
{
    public class CategoryMappings : Profile
    {
        public CategoryMappings()
        {
            //CreateMap<Category, CategoryReadDto>();
            CreateMap<Category, GetCategoryResult>();
            CreateMap<Category, GetCategoryResult>().ReverseMap();
        }
    }
}
