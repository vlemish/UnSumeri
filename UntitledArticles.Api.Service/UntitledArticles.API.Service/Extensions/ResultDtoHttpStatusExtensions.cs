namespace UntitledArticles.API.Service.Extensions;

using Microsoft.AspNetCore.Mvc;
using UntitiledArticles.API.Application.Models.Mediatr;
using UntitiledArticles.API.Application.OperationStatuses;

public static class ResultDtoHttpStatusExtensions
{
    private static Dictionary<OperationStatusValue, int> operationToHttpStatusMappings = new()
    {
        { OperationStatusValue.OK, StatusCodes.Status200OK },
        { OperationStatusValue.NoContent, StatusCodes.Status204NoContent },
        { OperationStatusValue.Created, StatusCodes.Status201Created },
        { OperationStatusValue.NotModified, StatusCodes.Status304NotModified },
        { OperationStatusValue.NotFound, StatusCodes.Status404NotFound },
        { OperationStatusValue.ParentNotExists, StatusCodes.Status404NotFound },
        { OperationStatusValue.Duplicate, StatusCodes.Status409Conflict }
    };

    public static ObjectResult ToHttpObjectResult(this ResultDto resultDto) =>
        new(null) { StatusCode = ParseHttpStatusCode(resultDto.OperationStatus.Status) };

    public static ObjectResult ToHttpObjectResult<T>(this ResultDto<T> resultDto) =>
        new(resultDto.Payload) { StatusCode = ParseHttpStatusCode(resultDto.OperationStatus.Status) };

    private static int ParseHttpStatusCode(OperationStatusValue statusValue) =>
        operationToHttpStatusMappings.ContainsKey(statusValue)
            ? operationToHttpStatusMappings[statusValue]
            : throw new ArgumentNullException($"Mapping {statusValue} to HttpStatusCode isn't supported");
}
