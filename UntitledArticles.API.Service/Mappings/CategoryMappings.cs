using AutoMapper;

using UntitiledArticles.API.Application.Categories.Queries.GetAll;
using UntitiledArticles.API.Application.Categories.Queries.GetById;

using UntitledArticles.API.Domain.Entities;

namespace UntitledArticles.API.Service.Mappings
{
    using UntitiledArticles.API.Application.Models;

    public class CategoryMappings : Profile
    {
        public CategoryMappings()
        {
            CreateMap<Category, GetCategoryByIdResult>();
            CreateMap<Category, GetCategoryByIdResult>().ReverseMap();
            CreateMap<Category, GetAllCategoriesResult>();
            CreateMap<Article, ArticleDto>();
        }
    }
}
