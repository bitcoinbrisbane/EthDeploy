using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nethereum.HdWallet;

namespace ETHGitHubDeploy.Controllers.API
{
    [Route("api/[controller]")]
    public class HDWalletController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get(String id, int max = 10)
        {
			var wallet = new Wallet(id, null);
			List<String> accounts = new List<string>();

			for (int i = 0; i < max; i++)
            {
				accounts.Add(wallet.GetAccount(i).Address);
            }
            
			return accounts;
        }
    }
}
