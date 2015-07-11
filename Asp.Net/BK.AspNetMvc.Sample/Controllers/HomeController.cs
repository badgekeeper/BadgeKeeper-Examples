/**
* Copyright Badge Keeper 2015
* Licensed under The MIT License (http://www.opensource.org/licenses/mit-license.php)
*/
using System.Web.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BK.HMACAuthentication;
using BK.AspNetMvc.Sample.Models;

namespace BK.AspNetMvc.Sample.Controllers
{
    public class HomeController : Controller
    {
        private BadgeKeeperWebClient webClient = null;
        private string bkServiceUrl = "https://api.badgekeeper.net/";
        // Recommendation: read these parameters from web.config or app.config to make it secure
        private string appId = "";
        private string appKey = "";

        public HomeController()
        {
            webClient = new BadgeKeeperWebClient(appId, appKey);
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ContentResult> GetProjectAchievements(string isLoadIcons)
        {
            string path = bkServiceUrl + "projects/" + appId + "/" + isLoadIcons;
            string response = await webClient.MakeGetRequest(path);
            return new ContentResult { Content = response, ContentType = "application/json" };
        }

        [HttpGet]
        public async Task<ContentResult> GetUserAchievements(string userId, string isLoadIcons)
        {
            string path = bkServiceUrl + "projects/" + appId
                                       + "/users/" + userId
                                       + "/?isLoadIcons=" + isLoadIcons;
            string response = await webClient.MakeGetRequest(path);
            return new ContentResult { Content = response, ContentType = "application/json" };
        }

        [HttpPost]
        public async Task<ContentResult> PostVariables(string userId, List<PostVariables> model)
        {
            string path = bkServiceUrl + "projects/" + appId
                                       + "/users/" + userId + "/";
            string json = JsonConvert.SerializeObject(model);
            string response = await webClient.MakePostRequest(path, json);
            return new ContentResult { Content = response, ContentType = "application/json" };
        }
    }
}