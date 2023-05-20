using UntitiledArticles.API.Application.OperationStatuses;

namespace UntitiledArticles.API.Application.Articles.Commands.Add;

public record AddArticleResponse(IOperationStatus OperationStatus, AddArticleResult Result);
