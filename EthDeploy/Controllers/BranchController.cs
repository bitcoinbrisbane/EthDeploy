using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Octokit;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ETHGitHubDeploy.Controllers.API
{
    [Route("api/[controller]")]
    public class BranchController : Controller
    {
        private readonly String github_client_id = "3061f3362d3bf92ab105";

        public BranchController(IConfiguration configuration)
        {
        }

        // GET: api/values
        [HttpGet]
        public async Task<IEnumerable<string>> Get(String owner, String repo)
        {
            var client = new GitHubClient(new ProductHeaderValue(github_client_id));
            var tokenAuth = new Credentials(HttpContext.Session.GetString("githubtoken")); 
            client.Credentials = tokenAuth;

            var branches = await client.Repository.Branch.GetAll(owner, repo);
            IEnumerable<String> x = branches.Select(b => b.Name);

            return x;
        }
    }
}