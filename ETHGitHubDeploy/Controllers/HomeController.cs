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

namespace ETHGitHubDeploy.Controllers
{
    public class HomeController : Controller
    {
    
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
    
        public IActionResult Index()
        {
			DeployRequest model = new DeployRequest() 
            { 
                Username = "OpenZeppelin", 
                Repo = "openzeppelin-solidity", 
                Branch = "master",
                ContractPath = "contracts/LimitBalance.sol", 
                Contract = "LimitBalance",
                Password = "whip venture public clip similar debris minimum mandate despair govern rotate swim",
                //Node = "https://rinkeby.infura.io",
                Node = "https://rinkeby.infura.io/v3/eaf5e0b4a01042a48211762c8d4eec44",
                Network = "Rinkeby"
            };
             
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Deploy(DeployRequest model)
        {

			try
			{
				String[] paths = model.ContractPath.Split(',');
				String[] srcs = new string[paths.Length];

				int i = 0;
				//for (int i = 0; i < paths.Length; i++)
				//{
				String temp = Path.GetTempPath();
				String[] contracts = paths[i].Split('/');
				String contract = contracts[contracts.Length - 1];

				//https://raw.githubusercontent.com/BlockClick/eth-contracts/master/src/contracts/Media.sol?token=AIBZDlnVn934pdoiGZY4P4n-Ekr34LmDks5be32kwA%3D%3D
				String url = String.Format("https://raw.githubusercontent.com/{0}/{1}/{2}/{3}?token=AIBZDjwzPI3OkfsSXNgUVRaJO0KDjYs6ks5bYVpxwA%3D%3D", model.Username, model.Repo, model.Branch, paths[i]);

				using (var client = new HttpClient())
				{
					var stream = await client.GetStreamAsync(url);

					using (var fileStream = System.IO.File.Create(temp + contract))
					using (var reader = new StreamReader(stream))
					{
						stream.CopyTo(fileStream);
						fileStream.Flush();
					}
				}

				String hash = CheckHash(temp + contract);
				srcs[i] = temp + contract;
				//}

				var solcLib = SolcLib.Create("");
				var compiled = solcLib.Compile(srcs, outputSelection);

				var output = compiled.Contracts[temp + contract][model.Contract];

				DeployResult result = new DeployResult()
				{
					JSON = output.AbiJsonString,
					ABI = output.AbiJsonString,
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
    }
}
