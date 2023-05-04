using UntitiledArticles.API.Application.OperationStatuses;
using UntitledArticles.API.Domain.Contracts;

namespace UntitiledArticles.API.Application.Categories.Queries.GetAll;

public record GetAllCategoriesResponse(IOperationStatus Status, IPaginatedResult<GetAllCategoriesResult> Result);