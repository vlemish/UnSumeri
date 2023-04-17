namespace UntitledArticles.API.Service.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                HandleException(httpContext, ex);
            }
        }

        private void HandleException(HttpContext httpContext, Exception exception)
        {
            httpContext.Response.ContentType = "application/json";
            HttpResponse response = httpContext.Response;

            switch (exception)
            {
                case FluentValidation.ValidationException:
                    {
                        response.StatusCode = StatusCodes.Status400BadRequest;
                        break;
                    }
                default:
                    {
                        response.StatusCode = StatusCodes.Status500InternalServerError;
                        break;
                    }
            }

            _logger.LogError($"Failed to process the request: {exception.Message}");
        }
    }
}
