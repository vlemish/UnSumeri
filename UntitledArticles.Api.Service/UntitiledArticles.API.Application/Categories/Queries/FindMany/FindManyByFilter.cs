using System.Linq.Expressions;
using MediatR;
using UntitiledArticles.API.Application.Models.Mediatr;
using UntitledArticles.API.Domain.Entities;

namespace UntitiledArticles.API.Application.Categories.Queries.FindMany;

public record FindManyByFilter(Expression<Func<Category, bool>> FilterExpression) : IRequest<ResultDto<FindManyByFilterResult>>;
