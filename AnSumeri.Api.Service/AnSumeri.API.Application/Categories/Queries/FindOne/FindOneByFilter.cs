namespace AnSumeri.API.Application.Categories.Queries.FindOne;

using System.Linq.Expressions;
using MediatR;
using Application.Categories.Queries.GetById;
using Application.Models.Mediatr;
using Domain.Entities;

public record FindOneByFilter(Expression<Func<Category, bool>> FilterExpression) : IRequest<ResultDto<FindOneByFilterResult>>;
