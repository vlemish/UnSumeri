using AutoMapper;

using MediatR;

using Microsoft.Extensions.Logging;
using Moq;
using UntitiledArticles.API.Application.Categories.Queries;
using UntitiledArticles.API.Application.Categories.Queries.GetById;
using UntitiledArticles.API.Application.OperationStatuses;
using UntitledArticles.API.Domain.Contracts;
using UntitledArticles.API.Domain.Entities;
using UntitledArticles.API.Service.Mappings;

namespace UntitledArticles.API.Application.Tests.Categories;

public class GetCategoryHandlerTest
{
    private Mock<ICategoryRepository> _categoryRepositoryMock;
    private IMapper _mapper;

    [Fact]
    public async Task TestGetCategoryHandler_WhenCategoryExist_ThenSuccess()
    {
        int id = 2;
        List<Category> subCategories = CreateSubCategories(id, 3);
        OperationStatusValue expectedOperationValue = OperationStatusValue.OK;
        Category category = new()
        {
            Id = id,
            Name = "test_category",
            SubCategories = subCategories,
        };
        GetCategoryById request = new(id);

        SetupMocks(category);

        GetCategoryByIdHandler handler = new(new Mock<ILogger<GetCategoryByIdHandler>>().Object,
            _categoryRepositoryMock.Object, _mapper);

        GetCategoryByIdResponse result = await handler.Handle(request, default);

        Assert.Equal(expectedOperationValue, result.Status.Status);
        Assert.True(IsCategoryResultEqualToCategory(category, result.Result));
    }
    
    [Fact]
    public async Task TestGetCategoryHandler_WhenCategoryNotExist_ThenNotFound()
    {
        int id = 2;
        Category category = null;
        OperationStatusValue expectedOperationValue = OperationStatusValue.NotFound;
        GetCategoryById request = new(id);

        SetupMocks(category);

        GetCategoryByIdHandler handler = new(new Mock<ILogger<GetCategoryByIdHandler>>().Object,
            _categoryRepositoryMock.Object, _mapper);

        GetCategoryByIdResponse result = await handler.Handle(request, default);

        Assert.Equal(expectedOperationValue, result.Status.Status);
    }

    private void SetupMocks(Category category)
    {
        _categoryRepositoryMock = new();
        _categoryRepositoryMock
            .Setup(m => m.GetOneByFilter(It.IsAny<Func<Category, bool>>()))
            .ReturnsAsync(category);
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new CategoryMappings())));
    }
    
    private bool IsCategoryResultEqualToCategory(Category category, GetCategoryByIdResult result)
    {
        if (category.Id == result.Id || category.Name == result.Name || category.ParentId == result.ParentId ||
            category.SubCategories.Count == category.SubCategories.Count)
        {
            return true;
        }

        return false;
    }

    private List<Category> CreateSubCategories(int id, int numberOfSubcategories)
    {
        List<Category> subCategories = new();
        for (int i = 0; i < numberOfSubcategories; i++)
        {
            subCategories.Add(new()
            {
                Name = $"name{i + 1}",
                ParentId = id,
            });
        }

        return subCategories;
    }
}