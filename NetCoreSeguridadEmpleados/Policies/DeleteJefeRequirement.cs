using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http; // Importante para el casting de HttpContext
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NetCoreSeguridadEmpleados.Models;
using NetCoreSeguridadEmpleados.Repositories;
using System.Security.Claims;

namespace NetCoreSeguridadEmpleados.Policies
{
    public class DeleteJefeRequirement : AuthorizationHandler<DeleteJefeRequirement>, IAuthorizationRequirement
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,DeleteJefeRequirement requirement)
        {
            var filterContext = context.Resource as AuthorizationFilterContext;
            var httpContext = filterContext.HttpContext;
            var repo = httpContext?.RequestServices.GetService<RepositoryHospital>();

            if (context.User.HasClaim(x => x.Type == ClaimTypes.NameIdentifier) == true && repo != null)
            {
                var claimId = context.User.FindFirst(ClaimTypes.NameIdentifier);
                int id = int.Parse(claimId.Value);

                List<Empleado> subordinados = await repo.GetSubordinados(id);

                if (subordinados.Count==0)
                {
                    context.Fail();
                }
                else
                {
                    context.Succeed(requirement);
                }
            }
            else
            {
                context.Fail();
            }
        }
    }
}