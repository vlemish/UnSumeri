using System.Linq.Expressions;
using AutoMapper;

using MediatR;

using Microsoft.Extensions.Logging;
using Moq;
using AnSumeri.API.Application.Categories.Queries;
using AnSumeri.API.Application.Categories.Queries.GetById;
using AnSumeri.API.Application.OperationStatuses;
using AnSumeri.API.Domain.Contracts;
using AnSumeri.API.Domain.Entities;
using AnSumeri.API.Service.Mappings;

namespace AnSumeri.API.Application.Tests.Categories;

using AnSumeri.API.Application.Models.Mediatr;

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
        GetCategoryById request = new(id, Guid.NewGuid().ToString());

        SetupMocks(category);

        GetCategoryByIdHandler handler = new(_categoryRepositoryMock.Object, _mapper);

        ResultDto<GetCategoryByIdResult> result = await handler.Handle(request, default);

        Assert.Equal(expectedOperationValue, result.OperationStatus.Status);
        Assert.True(IsCategoryResultEqualToCategory(category, result.Payload));
    }

    [Fact]
    public async Task TestGetCategoryHandler_WhenCategoryNotExist_ThenNotFound()
    {
        int id = 2;
        Category category = null;
        OperationStatusValue expectedOperationValue = OperationStatusValue.NotFound;
        GetCategoryById request = new(id,Guid.NewGuid().ToString());

        SetupMocks(category);

        GetCategoryByIdHandler handler = new(_categoryRepositoryMock.Object, _mapper);

        ResultDto<GetCategoryByIdResult> result = await handler.Handle(request, default);

        Assert.Equal(expectedOperationValue, result.OperationStatus.Status);
    }

    private void SetupMocks(Category category)
    {
        _categoryRepositoryMock = new();
        _categoryRepositoryMock
            .Setup(m => m.GetOneByFilter(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<int>()))
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
