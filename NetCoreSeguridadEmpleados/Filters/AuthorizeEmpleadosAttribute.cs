using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace NetCoreSeguridadEmpleados.Filters
{
    public class AuthorizeEmpleadosAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {

            //POR AHORA,SOLAMENTE NOS INTERESA VALIDAR SI EXISTE O NO EL EMPELADO
            var user = context.HttpContext.User;

            //NECESITAMOS EL ACTION T EL CONTROLLER DE DONDE EL USUARIO HA PULSADO
            //PARA ELLO TENEMOS ROUTEValues QUE CONTIENE LA INFORMACION
            //RouteData["controller"]
            //RouteData["action"]
            //RouteData["idalgo"],eso es con if

            //VAMOS A RECUPERAR EL CONTROLLER y action
            string controller = context.RouteData.Values["controller"].ToString();
            string action = context.RouteData.Values["action"].ToString();

            //RECUPERAMOS LA VARIABLE ID PARA DETAILS
            var id = context.RouteData.Values["id"];
            //Es el tempdata de nuestra aplicacion,
            //gracias a que hemos puesto esto en program .AddSessionStateTempDataProvider();
            ITempDataProvider provider = context.HttpContext.RequestServices.GetService<ITempDataProvider>();

            var tempData = provider.LoadTempData(context.HttpContext);
            //ALMACENAMOS LA INFORMACION
            tempData["controller"] = controller;
            tempData["action"] = action;
            //SOLO FUNCIONA CON ID
            //DEBEMOS PREGUNTAR POR EL ID--
            if (id != null)
            {
                tempData["id"] = id.ToString();
            }
            else
            {
                //ELIMINAMOS EL ID SI ES QUE NO NOS LLEGA NINGUNO PARA QUE NO SE QUEDE ENTRE LAS PETICIONES
                tempData.Remove("id");
            }

                //REASIGNAMOS EL TEMPDATA PARA NUESTRA APP
                provider.SaveTempData(context.HttpContext, tempData);
            //Ahora vamos a managed




            if (user.Identity.IsAuthenticated == false)
            {
                context.Result = GetRoute("Managed", "Login");
            }

            //else//LO QUITAMOS PARA IMPLEMENTAR LO NUEVO
            //{
            //    //PARA EL NUEVO MÉTODO PARA CONTROLLAR QUE MÉTODOS PUEDE USAR EL USER DEPENDIENDO DE SU ROL
            //    //TENEMOS QUE TENER EN CUENTA MAYUSCULAS Y MINUSCULAS
            //    if (user.IsInRole("PRESIDENTE")==false && user.IsInRole("DIRECTOR")==false 
            //        && user.IsInRole("ANALISTA") == false)
            //    {//si no entran uno de estos 3 pues se le lleva ahí
            //        context.Result = this.GetRoute("Managed", "ErrorAcceso");
            //    }
            //}
            
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
