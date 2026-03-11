using Microsoft.EntityFrameworkCore;
using NetCoreSeguridadEmpleados.Data;
using NetCoreSeguridadEmpleados.Models;

namespace NetCoreSeguridadEmpleados.Repositories
{
    public class RepositoryHospital
    {
        private HospitalContext context;

        public RepositoryHospital(HospitalContext context)
        {
            this.context = context;
        }

        public async Task<List<Empleado>> GetAllEmpleadoAsync()
        {

            return await this.context.empleados.ToListAsync();
        }

        public async Task<Empleado> FindEmpleadoAsync(int idemp)
        {
            //se puede usar find si lo que buscas es sobre la primary key
            return await this.context.empleados.Where(z=>z.IdEmpleado==idemp).FirstOrDefaultAsync();
        }
        public async Task<List<Empleado>> GetEmpleadosDepartamento(int deptno)
        {

            return await this.context.empleados.Where(z=>z.IdDept==deptno).ToListAsync();
        }

        public async Task UpdateSalariosEmpleadosDepartamentoAsync(int deptno,int incremento)
        {
            List<Empleado> empleados = await this.GetEmpleadosDepartamento(deptno);
            foreach (Empleado emp in empleados)
            {
                emp.Salario += incremento;
            }
            await this.context.SaveChangesAsync();
        }

        public async Task<Empleado> LogInEmpleadoAsync(string apellido,int idempleado)
        {
            Empleado empleado = await this.context.empleados.FirstOrDefaultAsync(z=>z.Apellido==apellido && z.IdEmpleado==idempleado);
            return empleado;
        }

    }
}
