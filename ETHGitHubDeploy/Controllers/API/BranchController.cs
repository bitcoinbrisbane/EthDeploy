using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ETHGitHubDeploy.Controllers.API
{
    [Route("api/[controller]")]
    public class BranchController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get(String user, String repo)
        {
            return new string[] { "master", "release" };
        }
    }
}