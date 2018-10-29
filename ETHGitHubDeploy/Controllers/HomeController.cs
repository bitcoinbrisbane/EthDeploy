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
using Nethereum.Web3.Accounts; 
using Nethereum.Util; 
using Nethereum.Hex.HexConvertors.Extensions; 
using Nethereum.HdWallet;
using Microsoft.AspNetCore.Http;

namespace ETHGitHubDeploy.Controllers
{
    public class HomeController : Controller
    {
		private readonly String client_id = "3061f3362d3bf92ab105";
		
		private String token = "";
		
        OutputType[] outputSelection = new[] {
                    OutputType.Abi,
                    //OutputType.Ast,
                    //OutputType.EvmMethodIdentifiers,
                    OutputType.EvmAssembly,
                    OutputType.EvmBytecode,
                    //OutputType.IR,
                    //OutputType.DevDoc,
                    //OutputType.UserDoc,
                    //OutputType.Metadata
                };
    
        public async Task<IActionResult> Index(String code, String username)
        {
			if (!String.IsNullOrEmpty(code))
			{
				token = await GetToken(client_id, "91c4df350fa87192ffc1d53468c1b2e4311c1d7f", code);
				HttpContext.Session.SetString("githubtoken", token);
			}
			else
			{
				Response.Redirect("https://github.com/login/oauth/authorize?client_id=" + client_id);
			}

			DeployRequest model = new DeployRequest() 
            { 
                Username = "ConsenSys", 
                Repo = "mythril-classic", 
                Branch = "develop",
                ContractPath = "solidity_examples", 
                Contract = "BecToken",
                Password = "whip venture public clip similar debris minimum mandate despair govern rotate swim",
                //Node = "https://rinkeby.infura.io",
                Node = "http://121.210.77.231:8545",
                Network = "Rinkeby"
            };
             
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Deploy(DeployRequest model)
        {
			try
			{
            	var gitHubClient = new GitHubClient(new ProductHeaderValue(client_id));
            	gitHubClient.Credentials = new Credentials(HttpContext.Session.GetString("githubtoken")); 

				IReadOnlyList<RepositoryContent> contents = await gitHubClient.Repository.Content.GetAllContentsByRef(model.Username, model.Repo, model.ContractPath, model.Branch);
				
				String[] srcs;
				String temp = Path.GetTempPath();
				
				foreach(RepositoryContent content in contents.Where(c => c.Name.EndsWith(".sol")))
				{
					using (var client = new HttpClient())
					{
						var stream = await client.GetStreamAsync(content.DownloadUrl);

						using (var fileStream = System.IO.File.Create(temp + content.Name))
						using (var reader = new StreamReader(stream))
						{
							stream.CopyTo(fileStream);
							fileStream.Flush();
						}
					}

					//String hash = CheckHash(temp + content.Name);
				}

				var solcLib = SolcLib.Create("");
				var compiled = solcLib.Compile(temp + model.Contract + ".sol", outputSelection);

				var output = compiled.Contracts[temp + model.Contract + ".sol"][model.Contract];

				DeployResult result = new DeployResult()
				{
					JSON = output.AbiJsonString,
					ABI = "",
					Bin = BitConverter.ToString(output.Evm.Bytecode.ObjectBytes).Replace("-", String.Empty)
				};

				var account = new Wallet(model.Password, null).GetAccount(0);
				var web3 = new Nethereum.Web3.Web3(account, model.Node);

				result.TxID = await web3.Eth.DeployContract.SendRequestAsync(result.Bin, model.KeyFile, new Nethereum.Hex.HexTypes.HexBigInteger(model.Gas));

				return View(result);
			}
            catch (Exception ex)
            {
				Console.Write(ex.Message);
				throw ex;
            }
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

		private async Task<String> GetToken(String clientId, String secret, String code)
		{
			IList<KeyValuePair<string, string>> nameValueCollection = new List<KeyValuePair<string, string>> {
				{ new KeyValuePair<string, string>("client_id", clientId) },
				{ new KeyValuePair<string, string>("client_secret", secret) },
				{ new KeyValuePair<string, string>("code", code) },
			};

			using (var client = new HttpClient())
			{
				var result = await client.PostAsync("https://github.com/login/oauth/access_token", new FormUrlEncodedContent(nameValueCollection));
				var response = await result.Content.ReadAsStringAsync();

				if (response.Contains("access_token"))
				{
					String token = response.Split("&")[0];
					return token.Split("=")[1];
				}
				else
				{
					throw new Exception();
				}
			}
		}
    }
}
