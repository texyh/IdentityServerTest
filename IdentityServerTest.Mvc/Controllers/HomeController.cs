﻿using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using IdentityServerTest.Mvc.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace IdentityServerTest.Mvc.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult About()
        {
            //ViewData["Message"] = "Your application description page.";

            return View((User as ClaimsPrincipal).Claims);
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        public async Task Logout()
        {
            //Request.GetOwinContext().Authentication.SignOut();
            //return RedirectToAction("Index");
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");
        }

        public async Task<IActionResult> CheckingForAuthorization()
        {
            if (User.Identity.IsAuthenticated)
            {
                string accessToken = await HttpContext.GetTokenAsync("access_token");
                string idToken = await HttpContext.GetTokenAsync("id_token");

                var client = new HttpClient();
                //client.SetBearerToken(accessToken);
                //    var content = await client.GetStringAsync("http://localhost:6001/api/identity");
                //    ViewBag.Json = JArray.Parse(content).ToString();
                //    return View("Contact");

                // Now you can use them. For more info on when and how to use the 
                // access_token and id_token, see https://auth0.com/docs/tokens

            }
            return View("Contact");

        }

        public async Task<IActionResult> CallApiUsingUserAccessToken()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var client = new HttpClient();
            client.SetBearerToken(accessToken);
            var content = await client.GetStringAsync("https://localhost:44338/");
            ViewBag.Json = JArray.Parse(content).ToString();
            return View("json");
        }
    }
}
