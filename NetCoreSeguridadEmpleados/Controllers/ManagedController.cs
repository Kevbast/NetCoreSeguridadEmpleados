using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using NetCoreSeguridadEmpleados.Models;
using NetCoreSeguridadEmpleados.Repositories;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NetCoreSeguridadEmpleados.Controllers
{
    public class ManagedController : Controller
    {
        private RepositoryHospital repo;

        public ManagedController(RepositoryHospital repo)
        {
            this.repo = repo;
        }

        public IActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LogIn(string username,string password)
        {
            int idEmpleado = int.Parse(password);
            Empleado empleado = await this.repo.LogInEmpleadoAsync(username, idEmpleado);

            if (empleado != null)
            {
                ClaimsIdentity identity = new ClaimsIdentity(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    ClaimTypes.Name, ClaimTypes.Role);

                Claim claimName = new Claim(ClaimTypes.Name , username);
                identity.AddClaim(claimName);

                //AÑADIMOS otros Claims más
                Claim claimId =
                    new(ClaimTypes.NameIdentifier, empleado.IdEmpleado.ToString());
                identity.AddClaim(claimId);
                //AHORA COMO ROL USAREMOS SU OFICIO
                Claim claimRole =
                    new(ClaimTypes.Role, empleado.Oficio);
                identity.AddClaim(claimRole);
                //Y UNO DE SALARIO
                Claim claimSalario =
                    new("Salario", empleado.Salario.ToString());
                identity.AddClaim(claimSalario);
                //Y UNO DE deptno
                Claim claimDept =
                    new("Departamento", empleado.IdDept.ToString());
                identity.AddClaim(claimDept);


                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(identity);
                //VALIDAMOS EL SCHEMA Y USER
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);
                //POR AHORA LO ENVIAMOS A UNA VISTA QUE HAREMOS EN BREVE
                return RedirectToAction("Perfil", "Empleados");//MIRAR BIEN EL NOMBRE
            }

            else
            {
                ViewData["MENSAJE"] = "CREDENCIALES INCORRECTAS";
                return View();
            }

        }//final LogIn

        public async Task<IActionResult> LogOut()
        {//sin vista
            await HttpContext.SignOutAsync
                (CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");

        }

        //COMENZAMOS CON UN NUEVO MÉTODO

        public IActionResult ErrorAcceso()
        {
            return View();
        }



    }
}
