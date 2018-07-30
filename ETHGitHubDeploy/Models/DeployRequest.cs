using System;
using System.ComponentModel;

namespace ETHGitHubDeploy.Models
{
    public class DeployRequest : DeployResult
    {
        public String Username { get; set; }
        
        public String Repo { get; set; }
        
        public String Branch { get; set; }
        
        public String Contract { get; set; }
        
        [DisplayName("Keystore / JSON File ")]
        public String KeyFile { get; set; }
        
        public String Password { get; set; }
        
        public String Compiler { get; set; }
        
        public String Network { get; set; }
        
        public String Node { get; set; }
        
        public DeployRequest()
        {
			this.Compiler = "0.4.23";
			this.Network = "Rinkeby";
			this.Node = "https://infura.io";
        }
    }
}