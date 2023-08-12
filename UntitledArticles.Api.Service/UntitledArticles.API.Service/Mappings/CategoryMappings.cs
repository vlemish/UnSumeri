using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using UntitiledArticles.API.Application.Categories.Queries.FindMany;
using UntitiledArticles.API.Application.Categories.Queries.GetAll;
using UntitiledArticles.API.Application.Categories.Queries.GetById;
using UntitledArticles.API.Domain.Entities;

namespace UntitledArticles.API.Service.Mappings
{
    using System.Linq.Expressions;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using UntitiledArticles.API.Application.Categories.Queries.FindOne;
    using UntitiledArticles.API.Application.Models;

    public class CategoryMappings : Profile
    {
        public CategoryMappings()
        {
            CreateMap<Category, GetCategoryByIdResult>();
            CreateMap<Category, GetCategoryByIdResult>().ReverseMap();
            CreateMap<Category, FindOneByFilterResult>();
            CreateMap<Category, FindOneByFilterResult>().ReverseMap();
            CreateMap<Category, GetAllCategoriesResult>();
            CreateMap<Article, ArticleDto>();
        }
    }
}
