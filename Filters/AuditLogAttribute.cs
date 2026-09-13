using Microsoft.AspNetCore.Mvc.Filters;
using MVCDemo.Context;
using MVCDemo.Models;

namespace MVCDemo.Filters
{
    // Module 6: Custom Action Filter -> logs every create/update/delete into the AuditLogs table
    public class AuditLogAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            // resolve the DbContext from DI instead of creating a new instance
            var db = context.HttpContext.RequestServices.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;

            var userName = context.HttpContext.User?.Identity?.Name ?? "Anonymous";
            var userId = context.HttpContext.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var controller = context.RouteData.Values["controller"];
            var action = context.RouteData.Values["action"];

            var log = new AuditLog
            {
                UserId = userId,
                UserName = userName,
                ActionDetails = $"{controller}/{action}",
                Timestamp = DateTime.Now
            };

            db?.AuditLogs.Add(log);
            db?.SaveChanges();

            base.OnActionExecuted(context);
        }
    }
}
