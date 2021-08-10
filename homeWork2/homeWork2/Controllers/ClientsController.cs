using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timesheets.Controllers
{
    public class ClientsController : Controller
    {
        //register
        [HttpGet("register")]
        public IActionResult RegisterClient()
        {
            return Ok();
        }

        //update
        [HttpPut("update/{id}")]
        public IActionResult UpdateClient([FromRoute] int id)//+ другие параметры
        {
            return Ok();
        }

        //delete
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteClient([FromRoute] int id)
        {
            return Ok();
        }

        //get by id
        [HttpGet("{id}")]
        public IActionResult GetClient([FromRoute] int id)
        {
            return Ok();
        }

        //get all
        [HttpGet("all")]
        public IActionResult GetClients()
        {
            return Ok();
        }
    }
}
