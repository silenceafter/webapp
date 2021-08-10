using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timesheets.Controllers
{
    [Route("api/employee")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        //register
        [HttpGet("register")]
        public IActionResult RegisterEmployee()
        {
            return Ok(); 
        }

        //update
        [HttpPut("update/{id}")]
        public IActionResult UpdateEmployee([FromRoute] int id, double account)
        {
            return Ok();
        }

        //delete
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteEmployee([FromRoute] int id)
        {
            return Ok();
        }

        //get by id
        [HttpGet("{id}")]
        public IActionResult GetEmployee([FromRoute] int id)
        {
            return Ok();
        }

        //get all
        [HttpGet("all")]
        public IActionResult GetEmployees()
        {
            return Ok();
        }
    }
}