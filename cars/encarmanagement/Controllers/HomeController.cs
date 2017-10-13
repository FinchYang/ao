using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using mvc104.models;
using Microsoft.Extensions.Logging;
using encm.cars;

namespace mvc104.Controllers
{
    public class HomeController : Controller
    {
        public readonly ILogger<HomeController> _log;
        public HomeController(ILogger<HomeController> log)
        {
            _log = log;
        }
       
        public IActionResult chart()
        {
            //   ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
