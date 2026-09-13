namespace MVCDemo.Middleware
{
    // Module 6: Custom Middleware -> redirects to a maintenance page if IsMaintenanceMode is true
    public class MaintenanceModeMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;

        public MaintenanceModeMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            _config = config;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            bool isMaintenanceMode = _config.GetValue<bool>("AppSettings:IsMaintenanceMode");

            // always allow the maintenance page itself and static files through
            var path = context.Request.Path.Value ?? "";
            bool isAllowedPath = path.StartsWith("/Home/Maintenance") || path.StartsWith("/css") || path.StartsWith("/js") || path.StartsWith("/lib");

            if (isMaintenanceMode && !isAllowedPath)
            {
                context.Response.Redirect("/Home/Maintenance");
                return;
            }

            await _next(context);
        }
    }

    // Extension method so it can be registered easily in Program.cs
    public static class MaintenanceModeMiddlewareExtensions
    {
        public static IApplicationBuilder UseMaintenanceMode(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MaintenanceModeMiddleware>();
        }
    }
}
