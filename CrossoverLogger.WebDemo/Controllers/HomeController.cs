using CrossoverLogger.Api.Client.v1;
using CrossoverLogger.Api.Model.v1;
using CrossoverLogger.WebDemo.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CrossoverLogger.WebDemo.Controllers
{
    public class HomeController : Controller
    {
        private CrossoverLoggerClient client = new CrossoverLoggerClient("http://localhost:8080/");

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterV1Request request)
        {
            try
            {
                var response = this.client.Register(request);
                ViewBag.Result = JsonConvert.SerializeObject(response, Formatting.Indented);
            }
            catch(Exception ex)
            {
                ModelState.AddModelError(nameof(request.DisplayName), ex);
                return View(request);
            }
            return View("ResponseApiSuccess");
        }

        [HttpGet]
        public ActionResult Auth()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Auth(AuthRequest request)
        {
            try
            {
                var response = this.client.Auth(request.AppId, request.AppSecret);
                ViewBag.Result = JsonConvert.SerializeObject(response, Formatting.Indented);
                Session["Token"] = response.Token;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(nameof(request.AppId), ex);
                return View(request);
            }
            return View("ResponseApiSuccess");
        }

        [HttpGet]
        public ActionResult Log()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Log(LogV1Request request)
        {
            try
            {
                var token = Session["Token"]?.ToString();
                var response = this.client.Log(request, token);
                ViewBag.Result = JsonConvert.SerializeObject(response, Formatting.Indented);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(nameof(request.ApplicationId), ex);
                return View(request);
            }
            return View("ResponseApiSuccess");
        }
    }
}