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
        [AuthorizeEmpleados]

        public async Task<IActionResult> Details(int id)//SOLO FUNCIONA CON ID
        {
            Empleado emp = await this.repo.FindEmpleadoAsync(id);
            return View(emp);
        }

        [AuthorizeEmpleados]//el attribute
        public async Task<IActionResult> Perfil()
        {
            return View();
        }

        [AuthorizeEmpleados(Policy ="SOLOJEFES")]
        public async Task<IActionResult> Compis()
        {
            //RECUPERAMOS EL CLAIM DEL USUARIIOS VALIDADO(DEPARTAMENTO)
            string dato = HttpContext.User.FindFirstValue("Departamento");

            int idDepartamento = int.Parse(dato);

            List<Empleado> empleados = await this.repo.GetEmpleadosDepartamento(idDepartamento);

            return View(empleados);
        }

        [AuthorizeEmpleados]//aunque lo tenga en la vista es recomendable ponerlo en el post tmb
        [HttpPost]
        public async Task<IActionResult> Compis(int incremento)
        {

            //RECUPERAMOS EL CLAIM Departamento DEL USUARIO VALIDADO(DEPARTAMENTO)
            string dato = HttpContext.User.FindFirstValue("Departamento");

            int idDepartamento = int.Parse(dato);
            await this.repo.UpdateSalariosEmpleadosDepartamentoAsync(idDepartamento,incremento);

            List<Empleado> empleados = await this.repo.GetEmpleadosDepartamento(idDepartamento);

            return View(empleados);
        }


        [AuthorizeEmpleados(Policy ="AdminOnly")]
        public IActionResult AdminEmpleados()
        {
            return View();
        }

        //PARA EL NUEVO POLICY QUE HEMOS CREADO
        [AuthorizeEmpleados(Policy = "SOLOMASONES")]
        public IActionResult ZonaMasones()
        {
            return View();
        }






    }
}
