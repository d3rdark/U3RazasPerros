using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using U3RazasPerros.Models;

namespace U3RazasPerros.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public HomeController(perrosContext context)
        {
            Context = context;
        }

        public perrosContext Context { get; }

        [Route("admin/Home")]
        [Route("admin/Home/Index")]
        [Route("admin/")]
        public IActionResult Index()
        {
            //var razas = Context.Razas.OrderBy(x => x.Nombre);
            return View();
        }
    }
}
