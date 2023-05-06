using System.Reflection;
using MediatR;
using Microsoft.Extensions.Logging;

namespace UntitiledArticles.API.Application.PipelineBehaviours
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            List<string> properties = GetRequestProperties(request);
            _logger.LogInformation($"Handling {typeof(TRequest).Name} where {string.Join(", ", properties)}!");
            TResponse result = await next();
            _logger.LogInformation($"{typeof(TRequest).Name} where {string.Join(", ", properties)} was handled!");
            return result;
        }

        private List<string> GetRequestProperties(TRequest request)
        {
            List<string> properties = new List<string>();
            Type type = request.GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(type.GetProperties());
            foreach (var prop in props)
            {
                object propValue = prop.GetValue(request, null);
                properties.Add($"{prop.Name} = {propValue}");
            }

            return properties;
        }
    }
}