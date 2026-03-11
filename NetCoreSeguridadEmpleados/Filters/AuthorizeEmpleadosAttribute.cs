using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NetCoreSeguridadEmpleados.Filters
{
    public class AuthorizeEmpleadosAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //POR AHORA,SOLAMENTE NOS INTERESA VALIDAR SI EXISTE O NO EL EMPELADO
            var user = context.HttpContext.User;

            if (user.Identity.IsAuthenticated == false)
            {
                context.Result = GetRoute("Managed", "Login");
            }
            else
            {
                //PARA EL NUEVO MÉTODO PARA CONTROLLAR QUE MÉTODOS PUEDE USAR EL USER DEPENDIENDO DE SU ROL
                //TENEMOS QUE TENER EN CUENTA MAYUSCULAS Y MINUSCULAS
                if (user.IsInRole("PRESIDENTE")==false && user.IsInRole("DIRECTOR")==false 
                    && user.IsInRole("ANALISTA") == false)
                {//si no entran uno de estos 3 pues se le lleva ahí
                    context.Result = this.GetRoute("Managed", "ErrorAcceso");
                }
            }
            
        }
        //EN ALGUN MOMENTO TENDREMOS MAS REDIRECCIONES QUE SOLO A LOGIN,POR LO QUE CREAMOS UN MÉTODO PARA REDIECCIONAR

        private RedirectToRouteResult GetRoute(string controller,string action)
        {
            RouteValueDictionary ruta =
                new RouteValueDictionary(new
                {
                    controller = controller,
                    action = action
                });

            RedirectToRouteResult result = new RedirectToRouteResult(ruta);
            
            return result;
        }


    }
}
