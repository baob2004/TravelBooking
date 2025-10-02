using TravelBooking.API.Middlewares;

namespace TravelBooking.API.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IServiceCollection AddGlobalExceptionMiddleware(this IServiceCollection services)
        {
            services.AddTransient<ErrorHandlingMiddleware>();
            return services;
        }

        public static IApplicationBuilder UseGlobalExceptionMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}