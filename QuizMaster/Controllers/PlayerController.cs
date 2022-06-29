using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using QuizMaster.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using QuizMaster.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using QuizMaster.Areas.Identity.Data;

namespace QuizMaster.Controllers
{
    [Authorize(Roles = "Player,PremiumPlayer")]
    public class PlayerController : Controller
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        public PlayerController(IPlayerRepository playerRepository, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {            
            _playerRepository = playerRepository;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        [HttpGet]
        public IActionResult StartQuiz()
        {
            PlayerViewModel playerVM = new PlayerViewModel();
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Player player = _playerRepository.GetPlayer(id);
            playerVM.Rank = player.Rank;
            playerVM.Score = player.Score;
            playerVM.Questions = player.Questions;

            string urlLink = "http://jservice.io/api/random";
            string strResponseValue = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlLink);
            request.Method = "GET";

            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                //Proecess the response stream
                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            strResponseValue = reader.ReadToEnd();
                            reader.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                strResponseValue = "{\"errorMessages\":[\"" + ex.Message.ToString() + "\"],\"errors\":{}}";
            }
            finally
            {
                if (response != null)
                {
                    ((IDisposable)response).Dispose();
                }
            }
            //Removing [ ] from json string
            strResponseValue = strResponseValue.Substring(1, strResponseValue.Length - 2);            
            try
            {                                                
                var jQuestion = JsonConvert.DeserializeObject<Question>(strResponseValue);
                playerVM.Question = jQuestion;                                             
            }
            catch(Exception ex)
            {   
                Debug.WriteLine(ex.Message.ToString());
                ViewBag.question = "An error occured !";
            }
            //If the request call fail it will reload the action
            if (playerVM.Question == null)
            {
                return RedirectToAction("StartQuiz");                
            }

            //Score clipboard            
            string min, max, width, color;
            double difference, minInt;
            int maxInt;
            if (player.Rank == "Bronze")
            {
                min = "0";
                max = "99";
                color = "danger";
                minInt = 0;
                maxInt = 99;
                difference = 100;
            }
            else if (player.Rank == "Silver")
            {
                min = "100";
                max = "199";
                color = "info";
                minInt = 100;
                maxInt = 199;
                difference = 100;
            }
            else if (player.Rank == "Gold")
            {
                min = "200";
                max = "499";
                color = "warning";
                minInt = 200;
                maxInt = 499;
                difference = 300;
            }
            else if (player.Rank == "Diamond")
            {
                min = "500";
                max = "699";
                color = "info";
                minInt = 500;
                maxInt = 699;
                difference = 200;
            }
            else if (player.Rank == "Master")
            {
                min = "700";
                max = "1000";
                color = "success";
                minInt = 700;
                maxInt = 1000;
                difference = 300;
            }
            else
            {
                min = "0";
                max = "99";
                color = "danger";
                minInt = 0;
                maxInt = 99;
                difference = 100;
            }
            double percentage = (player.Score - minInt) / difference * 100.0;
            width = percentage.ToString() + "%";
            int points = maxInt+1 - player.Score;
            TempData["min"] = min;
            TempData["max"] = max;
            TempData["width"] = width;
            TempData["color"] = color;
            TempData["points"] = points;

            return View(playerVM);
        }
        public IActionResult ViewRank()
        {
            TempData["Name"] = User.FindFirstValue(ClaimTypes.Name);
            var players = _playerRepository.GetAllPlayers().Where(p => p.Score > 0);            
            var playersByDescending = players.OrderByDescending(x=>x.Score);
            return View(playersByDescending);
        }
        [HttpPost]
        public IActionResult CheckAnswer(PlayerViewModel playerVM)
        {
            //Smart conversion to prevent case sensitive and whitespace that result to wrong answer
            string inputUpper=null, questionAnswerUpper=null, input="input", questionAnswer="answer";
            if (playerVM.InputAnswer != null)
            {                
                inputUpper = playerVM.InputAnswer.ToUpper();
                questionAnswerUpper = playerVM.Question.answer.ToUpper();
                input = String.Concat(inputUpper.Where(c => !Char.IsWhiteSpace(c)));
                questionAnswer = String.Concat(questionAnswerUpper.Where(c => !Char.IsWhiteSpace(c)));
                Debug.WriteLine(input, questionAnswer);
            }
            
            //Adds 1 point for correct answer
            //Subracts 2 points for wrong answer
            //Subtracts 1 question of the daily questions
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Player player = _playerRepository.GetPlayer(id);
            if (User.IsInRole("Player"))
            {
                player.Questions = player.Questions - 1;
            }
            
            if (input == questionAnswer)
            {
                TempData["Message"] = "Correct Answer +1 point";                                
                player.Score = player.Score + 1;                                
            }
            else
            {
                TempData["Message"] = "Wrong Answer -2 points";                
                if (player.Score > 2)
                {
                    player.Score = player.Score - 2;                    
                }                
            }

            //Ranking System
            if (player.Score > 0 && player.Score < 100)
            {
                player.Rank = "Bronze";

            }
            else if (player.Score >= 100 && player.Score < 200)
            {
                player.Rank = "Silver";

            }
            else if (player.Score >= 200 && player.Score < 500)
            {
                player.Rank = "Gold";

            }
            else if (player.Score >= 500 && player.Score < 700)
            {
                player.Rank = "Diamond";
            }
            else if (player.Score >= 700)
            {
                player.Rank = "Master";
            }
            _playerRepository.Update(player);

            return RedirectToAction("StartQuiz");
        }
        public IActionResult SkipQuestion(PlayerViewModel playerVM)
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Player player = _playerRepository.GetPlayer(id);
            if (User.IsInRole("Player"))
            {
                player.Questions = player.Questions - 1;
                _playerRepository.Update(player);
                TempData["Message"] = "Skipped answer -1 of daily questions";
            }                        
            return RedirectToAction("StartQuiz");
        }
        [HttpGet]
        public IActionResult UpgradePremium()
        {

            return View();
        }
        [HttpGet]
        public async Task<RedirectToActionResult> PremiumPlayer()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await userManager.FindByIdAsync(userId);
            await userManager.RemoveFromRoleAsync(user, "Player");
            await userManager.AddToRoleAsync(user, "PremiumPlayer");
            return RedirectToAction("StartQuiz");
        }
    }
}
