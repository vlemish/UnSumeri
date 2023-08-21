using AutoMapper;
using MediatR;
using Moq;
using AnSumeri.API.Application.Categories.Commands.Delete;
using AnSumeri.API.Application.Categories.Queries.GetById;
using AnSumeri.API.Application.Categories.Queries.GetById.Statuses;
using AnSumeri.API.Application.Models.Mediatr;
using AnSumeri.API.Application.OperationStatuses;
using AnSumeri.API.Application.OperationStatuses.Shared.Categories;
using AnSumeri.API.Domain.Contracts;
using AnSumeri.API.Domain.Entities;
using AnSumeri.API.Service.Mappings;

namespace AnSumeri.API.Application.Tests.Categories;

public class DeleteCategoryHandlerTest
{
    private Mock<IMediator> _mediatorMock;
    private Mock<ICategoryRepository> _repositoryMock;

    private IMapper _mapper;

    private DeleteCategoryHandler _handler;

    [Fact]
    public async Task TestDeleteCategoryHandler_WhenCategoryExist_ThenSuccess()
    {
        int id = 2;
        string userId = Guid.NewGuid().ToString();
        OperationStatusValue expectedOperationStatus = OperationStatusValue.OK;

        SetupMocks(id);

        _handler = new(_repositoryMock.Object, _mediatorMock.Object, _mapper);

        ResultDto<DeleteCategoryResult> result = await _handler.Handle(new(id, userId), default);

        Assert.NotNull(result);
        Assert.Equal(expectedOperationStatus, result.OperationStatus.Status);
        Assert.Equal(id, result.Payload.Id);

        VerifyPositiveMocks();
    }

    [Fact]
    public async Task TestDeleteCategoryHandler_WhenCategoryNotExist_ThenNotFound()
    {
        int id = 2;
        string userId = Guid.NewGuid().ToString();
        OperationStatusValue expectedOperationStatus = OperationStatusValue.NotFound;

        SetupNotFoundMocks(id);

        _handler = new(_repositoryMock.Object, _mediatorMock.Object, _mapper);

        ResultDto<DeleteCategoryResult> result = await _handler.Handle(new DeleteCategory(id, userId), default);

        Assert.NotNull(result);
        Assert.Equal(expectedOperationStatus, result.OperationStatus.Status);
        Assert.Null(result.Payload);

        VerifyCategoryNotFoundMocks();
    }

    private void SetupMocks(int id)
    {
        _mediatorMock = new();
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new CategoryMappings())));
        _repositoryMock = new();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetCategoryById>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ResultDto<GetCategoryByIdResult>(new GetCategoryByIdSuccess(id),
                new GetCategoryByIdResult() { Name = "Name", Id = id }));
        _repositoryMock
            .Setup(m => m.DeleteAsync(It.IsAny<Category>()))
            .ReturnsAsync(id);
    }

    private void SetupNotFoundMocks(int id)
    {
        SetupMocks(id);
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetCategoryById>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ResultDto<GetCategoryByIdResult>(new CategoryNotFound(id), null));
    }

    private void VerifyPositiveMocks()
    {
        _mediatorMock
            .Verify(m => m.Send(It.IsAny<GetCategoryById>(),
                It.IsAny<CancellationToken>()), Times.Once());
        _repositoryMock
            .Verify(m => m.DeleteAsync(It.IsAny<Category>()),
                Times.Once());
    }

    private void VerifyCategoryNotFoundMocks()
    {
        _mediatorMock
            .Verify(m => m.Send(It.IsAny<GetCategoryById>(),
                It.IsAny<CancellationToken>()), Times.Once());
        _repositoryMock
            .Verify(m => m.DeleteAsync(It.IsAny<Category>()),
                Times.Never());
    }
}
