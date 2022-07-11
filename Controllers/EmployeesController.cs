using BusinessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly DatabaseContext databaseContext;

        public EmployeesController(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        [HttpGet]
        [Route("GetAllEmployees")]
        public async Task<IActionResult> Get()
        {
            List<Employee> employees = databaseContext.GetEmployees();
            return Ok(employees);
        }

        [AllowAnonymous]
        [Route("GetEmployeeById")]
        [HttpGet]
        public async Task<IActionResult> GetEmployeeById(int Id)
        {
            List<Employee> employees = databaseContext.GetEmployees();
            Employee employee = employees.Where(a => a.EmployeeId == Id).FirstOrDefault();
            return Ok(employee);
        }
    }
}
