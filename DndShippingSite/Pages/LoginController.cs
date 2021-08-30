using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DndShippingSite.Pages
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {

        [Route("Login/login_redirect")]
        [HttpGet]
        public IActionResult Index()
        {
            return Ok();
        }
    }
}
