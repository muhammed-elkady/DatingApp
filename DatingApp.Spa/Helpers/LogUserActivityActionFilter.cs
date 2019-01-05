using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using DatingApp.Infrastructure.Repositories.Interfaces;

namespace DatingApp.Core.Helpers
{
    public class LogUserActivityActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var actionContext = await next();
            var userId = actionContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userRepo = actionContext.HttpContext.RequestServices.GetService<IUserRepository>();
            var user = await userRepo.GetUserById(userId);
            user.LastActive = DateTime.Now;
            await userRepo.SaveAll();
        }
    }
}
