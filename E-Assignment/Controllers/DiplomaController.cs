using E_Assignment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Assignment.Controllers
{
    public class DiplomaController : Controller
    {
        [HttpGet]
        [Authorize(Roles = "Admin, Teacher")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Teacher")]
        public IActionResult Create(Diploma diploma)
        {
            return View();
        }
    }
}
