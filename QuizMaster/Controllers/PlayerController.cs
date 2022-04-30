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

namespace QuizMaster.Controllers
{
    [Authorize(Roles = "Player")]
    public class PlayerController : Controller
    {
        private readonly IPlayerRepository _playerRepository;
        public PlayerController(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }
        public IActionResult StartQuiz()
        {
            PlayerViewModel playerVM = new PlayerViewModel();
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
            return View(playerVM);
        }
        public IActionResult ViewRank()
        {
            TempData["Name"] = User.FindFirstValue(ClaimTypes.Name);
            var players = _playerRepository.GetAllPlayers();
            var playersByDescending = players.OrderByDescending(x=>x.Score);
            return View(playersByDescending);
        }
        public IActionResult CheckAnswer(PlayerViewModel playerVM)
        {
            //Smart conversion to prevent case sensitive and whitespace that result to wrong answer
            string inputUpper, questionAnswerUpper, input, questionAnswer;
            inputUpper = playerVM.InputAnswer.ToUpper();
            questionAnswerUpper = playerVM.Question.answer.ToUpper();
            input = String.Concat(inputUpper.Where(c => !Char.IsWhiteSpace(c)));
            questionAnswer = String.Concat(questionAnswerUpper.Where(c => !Char.IsWhiteSpace(c)));
            Debug.WriteLine(input, questionAnswer);

            //Adds 1 point for correct answer
            //Subracts 2 points for wrong answer
            //Subtracts 1 question of the daily questions
            if (input == questionAnswer)
            {
                TempData["Message"] = "Correct Answer +1 point";                
                var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                Player player = _playerRepository.GetPlayer(id);                
                player.Questions = player.Questions - 1;
                player.Score = player.Score + 1;
                _playerRepository.Update(player);
            }
            else
            {
                TempData["Message"] = "Wrong Answer -2 points";
                var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                Player player = _playerRepository.GetPlayer(id);
                player.Questions = player.Questions - 1;
                if (player.Score > 2)
                {
                    player.Score = player.Score - 2;                    
                }
                _playerRepository.Update(player);
            }
            return RedirectToAction("StartQuiz");
        }
        public IActionResult SkipQuestion(PlayerViewModel playerVM)
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Player player = _playerRepository.GetPlayer(id);
            player.Questions = player.Questions - 1;
            _playerRepository.Update(player);
            TempData["Message"] = "Skipped answer -1 of daily questions";
            return RedirectToAction("StartQuiz");
        }
        public IActionResult UpgradePremium()
        {
            return View();
        }
    }
}
