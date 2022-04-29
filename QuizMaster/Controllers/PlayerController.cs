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

namespace QuizMaster.Controllers
{
    public class PlayerController : Controller
    {        
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
            return View();
        }
        public IActionResult CheckAnswer(PlayerViewModel playerVM)
        {
            if(playerVM.InputAnswer == playerVM.Question.answer)
            {
                TempData["Answer"] = "Correct Answer +1 point";
            }
            else
            {
                TempData["Answer"] = "Wrong Answer -2 points";
            }
            return RedirectToAction("StartQuiz");
        }
        public IActionResult UpgradePremium()
        {
            return View();
        }
    }
}
