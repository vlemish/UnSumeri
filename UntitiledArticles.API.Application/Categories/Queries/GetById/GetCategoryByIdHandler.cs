using System.Linq.Expressions;
using AutoMapper;

using MediatR;

using Microsoft.Extensions.Logging;

using UntitiledArticles.API.Application.Categories.Queries.GetById.Statuses;

using UntitledArticles.API.Domain.Contracts;
using UntitledArticles.API.Domain.Entities;

namespace UntitiledArticles.API.Application.Categories.Queries.GetById
{
    public class GetCategoryByIdHandler : IRequestHandler<GetCategoryById, GetCategoryByIdResponse>
    {
        private readonly ILogger<GetCategoryByIdHandler> _logger;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public GetCategoryByIdHandler(ILogger<GetCategoryByIdHandler> logger, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _logger = logger;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<GetCategoryByIdResponse> Handle(GetCategoryById request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetOneByFilter(c=> c.Id == request.Id, request.Depth);
            if (category is null)
            {
                return ReportNotFound(request);
            }

            var result = _mapper.Map<GetCategoryByIdResult>(category);

            return ReportSuccess(request, result);
        }

       private Expression<Func<T, bool>> GetConstComparison<T, P>(string propertyNameOrPath, P value)
        {
            ParameterExpression paramT = Expression.Parameter(typeof(T), "x");
            Expression expr = getPropertyPathExpression(paramT, propertyNameOrPath.Split('.'));
            return Expression.Lambda<Func<T, bool>>(Expression.Equal(expr, Expression.Constant(value)), paramT);
        }

        private Expression getPropertyPathExpression(Expression expr, IEnumerable<string> propertyNameOrPath)
        {
            var mExpr = Expression.PropertyOrField(expr, propertyNameOrPath.First());
            if (propertyNameOrPath.Count() > 1)
            {
                return getPropertyPathExpression(mExpr, propertyNameOrPath.Skip(1));
            }
            else
            {
                return mExpr;
            }
        }

        private GetCategoryByIdResponse ReportNotFound(GetCategoryById request)
        {
            _logger.LogDebug($"Failed to get a category where Id = {request.Id}: Category not found");
            return new GetCategoryByIdResponse(new GetCategoryByIdNotFound(request.Id), null);
        }


        private GetCategoryByIdResponse ReportSuccess(GetCategoryById request, GetCategoryByIdResult result)
        {
            _logger.LogDebug($"Get Category where Id = {request.Id} was successfully handled");
            return new GetCategoryByIdResponse(new GetCategoryByIdSuccess(request.Id), result);
        }
    }
}
