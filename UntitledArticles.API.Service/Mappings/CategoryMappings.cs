using AutoMapper;

using UntitiledArticles.API.Application.Categories.Queries;
using UntitiledArticles.API.Application.Categories.Queries.GetAll;
using UntitiledArticles.API.Application.Categories.Queries.GetById;

using UntitledArticles.API.Domain.Entities;

namespace UntitledArticles.API.Service.Mappings
{
    public class CategoryMappings : Profile
    {
        public CategoryMappings()
        {
            CreateMap<Category, GetCategoryByIdResult>();
            CreateMap<Category, GetCategoryByIdResult>().ReverseMap();
            CreateMap<Category, GetAllCategoriesResult>();
        }
    }
}
