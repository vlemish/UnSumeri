using AutoMapper;

using UntitiledArticles.API.Application.Categories.Commands.Add;
using UntitiledArticles.API.Application.Models.Categories;

using UntitledArticles.API.Domain.Entities;

namespace UntitledArticles.API.Service.Mappings
{
    public class CategoryMappings : Profile
    {
        public CategoryMappings()
        {
            CreateMap<Category, CategoryReadDto>();
            CreateMap<Category, AddCategoryResult>();
        }
    }
}
