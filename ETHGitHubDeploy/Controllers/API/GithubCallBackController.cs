using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Octokit;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ETHGitHubDeploy.Controllers.API
{
    [Route("api/[controller]")]
    public class GithubCallBackController : Controller
    {
        private readonly String github_client_id = "3061f3362d3bf92ab105";

        [HttpPost]
        public void Post(Models.GitHub model)
        {
            HttpContext.Session.SetString("githubtoken", model.client_secret);
        }
    }
}