using MediatR;

namespace AnSumeri.API.Application.Categories.Commands.Update;

using Models.Mediatr;

public record UpdateCategory(int Id, string UserId, string Name) : CategoryBaseRequest<ResultDto>(UserId);
