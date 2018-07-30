using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ETHGitHubDeploy.Models;
using Octokit;
using System.Net.Http;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using SolcNet;
using SolcNet.CompileErrors;
using SolcNet.DataDescription.Input;
using SolcNet.NativeLib;

namespace ETHGitHubDeploy.Controllers
{
    public class HomeController : Controller
    {
    
        OutputType[] outputSelection = new[] {
                    OutputType.Abi,
                    OutputType.Ast,
                    OutputType.EvmMethodIdentifiers,
                    OutputType.EvmAssembly,
                    OutputType.EvmBytecode,
                    OutputType.IR,
                    OutputType.DevDoc,
                    OutputType.UserDoc,
                    OutputType.Metadata
                };
    
        public IActionResult Index()
        {
			Models.DeployRequest model = new DeployRequest() 
            { 
                Username = "OpenZeppelin", 
                Repo = "openzeppelin-solidity", 
                Branch = "master", 
                Contract = "LimitBalance",
                Node = "https://rinkeby.infura.io",
                Network = "Rinkeby"
            };
             
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Deploy(Models.DeployRequest model)
        {
            String temp = Path.GetTempFileName();
			Guid id = Guid.NewGuid();
			String url = String.Format("https://raw.githubusercontent.com/{0}/{1}/{2}/contracts/{3}.sol?token=AIBZDjwzPI3OkfsSXNgUVRaJO0KDjYs6ks5bYVpxwA%3D%3D", model.Username, model.Repo, model.Branch, model.Contract);

            using (var client = new HttpClient())
            {
                var stream = await client.GetStreamAsync(url);

                using (var fileStream = System.IO.File.Create(temp))
                using (var reader = new StreamReader(stream))
                {
                    stream.CopyTo(fileStream);
                    fileStream.Flush();
                }
            }

			String hash = CheckHash(temp);
            String[] srcs = new String[] { temp };

            var solcLib = SolcLib.Create("");
            var compiled = solcLib.Compile(srcs, outputSelection);

			Models.DeployResult result = new DeployResult()
			{
				JSON = compiled.RawJsonOutput
			};
            
            return View(result);
        }
        
        //
        public String CheckHash(String file)
        {
			using (FileStream fs = new FileStream(file, System.IO.FileMode.Open))
			{
				using (BufferedStream bs = new BufferedStream(fs))
				{
					using (SHA1Managed sha1 = new SHA1Managed())
					{
						byte[] hash = sha1.ComputeHash(bs);
						StringBuilder formatted = new StringBuilder(2 * hash.Length);
						foreach (byte b in hash)
						{
							formatted.AppendFormat("{0:X2}", b);
						}

						return formatted.ToString();
					}
				}
			}
        }
    }
}
