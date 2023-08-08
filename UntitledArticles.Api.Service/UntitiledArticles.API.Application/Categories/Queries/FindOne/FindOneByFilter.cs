namespace UntitiledArticles.API.Application.Categories.Queries.FindOne;

using System.Linq.Expressions;
using MediatR;
using UntitiledArticles.API.Application.Categories.Queries.GetById;
using UntitiledArticles.API.Application.Models.Mediatr;
using UntitledArticles.API.Domain.Entities;

public record FindOneByFilter(Expression<Func<Category, bool>> FilterExpression) : IRequest<ResultDto<FindOneByFilterResult>>;
