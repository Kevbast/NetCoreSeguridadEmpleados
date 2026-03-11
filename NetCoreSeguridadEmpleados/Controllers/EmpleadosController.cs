using Microsoft.AspNetCore.Mvc;
using NetCoreSeguridadEmpleados.Filters;
using NetCoreSeguridadEmpleados.Models;
using NetCoreSeguridadEmpleados.Repositories;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NetCoreSeguridadEmpleados.Controllers
{
    public class EmpleadosController : Controller
    {
        private RepositoryHospital repo;

        public EmpleadosController(RepositoryHospital repo)
        {
            this.repo = repo;
        }

        
        public async Task<IActionResult> Index()
        {
            List<Empleado> empleados = await this.repo.GetAllEmpleadoAsync();
            return View(empleados);
        }

        public async Task<IActionResult> Details(int empno)
        {
            Empleado emp = await this.repo.FindEmpleadoAsync(empno);
            return View(emp);
        }

        [AuthorizeEmpleados]//el attribute
        public async Task<IActionResult> Perfil()
        {
            return View();
        }

        [AuthorizeEmpleados]
        public async Task<IActionResult> Compis()
        {
            //RECUPERAMOS EL CLAIM DEL USUARIIOS VALIDADO(DEPARTAMENTO)
            string dato = HttpContext.User.FindFirstValue("Departamento");

            int idDepartamento = int.Parse(dato);

            List<Empleado> empleados = await this.repo.GetEmpleadosDepartamento(idDepartamento);

            return View(empleados);
        }




    }
}
