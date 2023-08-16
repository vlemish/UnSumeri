using System.Linq.Expressions;
using AnSumeri.API.Application.Models.Mediatr;
using MediatR;
using AnSumeri.API.Domain.Entities;

namespace AnSumeri.API.Application.Categories.Queries.FindMany;

public record FindManyByFilter(Expression<Func<Category, bool>> FilterExpression) : IRequest<ResultDto<FindManyByFilterResult>>;
