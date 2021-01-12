using AnonymousWebApi.Data.Contracts;
using AnonymousWebApi.Data.EFCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnonymousWebApi.ActionFilters
{
    public class ValidateEntityExistsAttribute<T> : IActionFilter where T : class, IEntityNew
    {
        private readonly AnonymousDBContext _context;
        public ValidateEntityExistsAttribute(AnonymousDBContext context)
        {
            _context = context;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            //Guid id = Guid.Empty;
            int id = 0;
            if (context.ActionArguments.ContainsKey("id"))
            {
                id = (int)context.ActionArguments["id"];
            }
            else
            {
                context.Result = new BadRequestObjectResult("Bad id parameter");
                return;
            }
            var entity = _context.Set<T>().SingleOrDefault(x => x.Id.Equals(id));
            if (entity == null)
            {
                context.Result = new NotFoundResult();
            }
            else
            {
                context.HttpContext.Items.Add("entity", entity);
            }
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
