namespace AnSumeri.API.Application.Categories;

using MediatR;

public abstract record CategoryBaseRequest<TResponse>(string UserId) : IRequest<TResponse> where TResponse : class;
