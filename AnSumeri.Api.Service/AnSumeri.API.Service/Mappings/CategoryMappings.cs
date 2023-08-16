using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using AnSumeri.API.Application.Categories.Queries.FindMany;
using AnSumeri.API.Application.Categories.Queries.GetAll;
using AnSumeri.API.Application.Categories.Queries.GetById;
using AnSumeri.API.Domain.Entities;

namespace AnSumeri.API.Service.Mappings
{
    using System.Linq.Expressions;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using AnSumeri.API.Application.Categories.Queries.FindOne;
    using AnSumeri.API.Application.Models;

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
