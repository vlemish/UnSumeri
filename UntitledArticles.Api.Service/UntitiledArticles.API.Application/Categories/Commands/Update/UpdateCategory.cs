using MediatR;

namespace UntitiledArticles.API.Application.Categories.Commands.Update;

using Models.Mediatr;

public record UpdateCategory(int Id, string UserId, string Name) : CategoryBaseRequest<ResultDto>(UserId);
