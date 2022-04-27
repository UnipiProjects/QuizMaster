using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaster.Controllers
{
    public class PlayerController : Controller
    {
        public IActionResult StartQuiz()
        {
            return View();
        }
        public IActionResult ViewRank()
        {
            return View();
        }
        public IActionResult UpgradePremium()
        {
            return View();
        }
    }
}
