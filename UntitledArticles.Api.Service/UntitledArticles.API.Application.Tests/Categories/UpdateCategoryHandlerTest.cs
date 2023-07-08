using MediatR;
using Moq;
using UntitiledArticles.API.Application.Categories.Commands.Update;
using UntitiledArticles.API.Application.Categories.Queries.GetById;
using UntitiledArticles.API.Application.Categories.Queries.GetById.Statuses;
using UntitledArticles.API.Domain.Contracts;
using UntitledArticles.API.Domain.Entities;

namespace UntitledArticles.API.Application.Tests.Categories;

using FluentValidation;
using FluentValidation.Results;
using FluentValidation.TestHelper;
using UntitiledArticles.API.Application.Articles.Commands.Update;
using UntitiledArticles.API.Application.Models.Mediatr;

public class UpdateCategoryHandlerTest
{
    private Mock<ICategoryRepository> _categoryRepositoryMock;
    private Mock<IMediator> _mediatorMock;

    private UpdateCategoryHandler _handler;

    [Theory]
    [InlineData(1, "content", "title")]
    public void TestUpdateArticleValdiator_WhenCommandValid_ThenValid(int id, string content, string title)
    {
        UpdateArticle command = new(id, Guid.NewGuid().ToString(), title, content);

        AbstractValidator<UpdateArticle> validator = new UpdateArticleValidator();
        var validationResult = validator.TestValidate(command);

        validationResult.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(0, "", "")]
    [InlineData(-1, "", "")]
    [InlineData(-1, null, "")]
    [InlineData(-1, null, null)]
    public void TestUpdateArticleValdiator_WhenCommandNotValid_ThenInvalid(int id, string content, string title)
    {
        UpdateArticle command = new(id, Guid.NewGuid().ToString(), title, content);

        AbstractValidator<UpdateArticle> validator = new UpdateArticleValidator();
        var validationResult = validator.TestValidate(command);

        validationResult.ShouldHaveValidationErrorFor(p => p.Id);
        validationResult.ShouldHaveValidationErrorFor(p => p.Content);
        validationResult.ShouldHaveValidationErrorFor(p => p.Title);
    }

    [Fact]
    public async Task TestUpdateCategoryHandler_WhenCategoryExist_ThenSuccess()
    {
        int id = 2;
        string name = "testname2";

        ResultDto<GetCategoryByIdResult> expectedGetByIdResponse = new(new GetCategoryByIdSuccess(id),
            new GetCategoryByIdResult() { Id = 2, Name = "testname1", });

        SetupMocks(expectedGetByIdResponse);

        _handler = new(_categoryRepositoryMock.Object, _mediatorMock.Object);

        ResultDto actual = await _handler.Handle(new UpdateCategory(id, Guid.NewGuid().ToString(), name), default);

        Assert.Equal(UntitiledArticles.API.Application.OperationStatuses.OperationStatusValue.OkNoContent,
            actual.OperationStatus.Status);
    }

    [Fact]
    public async Task TestUpdateCategoryHandler_WhenCategoryNotExist_ThenNotFound()
    {
        int id = 2;
        string name = "testname2";

        ResultDto<GetCategoryByIdResult> expectedGetByIdResponse = new(new GetCategoryByIdNotFound(id), null);

        SetupMocks(expectedGetByIdResponse);

        _handler = new(_categoryRepositoryMock.Object, _mediatorMock.Object);

        ResultDto actual = await _handler.Handle(new UpdateCategory(id, Guid.NewGuid().ToString(), name), default);

        Assert.Equal(UntitiledArticles.API.Application.OperationStatuses.OperationStatusValue.NotFound,
            actual.OperationStatus.Status);
    }

    private void SetupMocks(ResultDto<GetCategoryByIdResult> expectedGetByIdResponse)
    {
        _categoryRepositoryMock = new();
        _mediatorMock = new();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetCategoryById>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedGetByIdResponse);

        _categoryRepositoryMock
            .Setup(m => m.UpdateAsync(It.IsAny<Category>()))
            .Returns(Task.CompletedTask);
    }
}
