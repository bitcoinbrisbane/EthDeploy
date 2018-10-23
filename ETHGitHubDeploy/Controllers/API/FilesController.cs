using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Octokit;
using Microsoft.AspNetCore.Http;

namespace ETHGitHubDeploy.Controllers.API
{
    [Route("api/[controller]")]
    public class FilesController : Controller
    {
        private readonly String github_client_id = "3061f3362d3bf92ab105";

        public FilesController(IConfiguration configuration)
        {
        }

        // GET: api/files
        [HttpGet]
        public async Task<IEnumerable<string>> Get(String owner, String repo, String branch)
        {
            var client = new GitHubClient(new ProductHeaderValue(github_client_id));
            client.Credentials = new Credentials(HttpContext.Session.GetString("githubtoken")); 
            
            var content = await client.Repository.Content.GetAllContents(owner, repo);
            IEnumerable<String> files = content.Select(f => f.DownloadUrl);

            return files;
        }
    }
}