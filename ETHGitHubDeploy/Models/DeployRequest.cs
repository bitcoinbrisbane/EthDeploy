using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace ETHGitHubDeploy.Models
{
    public class DeployRequest
    {
        public String Username { get; set; }
        
        public String Repo { get; set; }
        
        public String Branch { get; set; }
        
        [DisplayName("Contract Path")]
        public String ContractPath { get; set; }

        public List<ContractViewModel> Contracts { get; private set; }
        
        [DisplayName("Contract")]
        public String Contract { get; set; }
        
        [DisplayName("Account Index")]
        public String KeyFile { get; set; }
        
        [DisplayName("Mnemonic")]
        public String Password { get; set; }
        
        public String Compiler { get; set; }
        
        public String Network { get; set; }
        
        public String Node { get; set; }
        
        public Int64 Gas { get; set; }
        
        public DeployRequest()
        {
			this.Compiler = "0.4.24";
			this.Network = "Rinkeby";
			this.Node = "https://infura.io";
			this.Password = "whip venture public clip similar debris minimum mandate despair govern rotate swim";
			this.KeyFile = "0";
			this.Gas = 2000000;

            this.Contracts = new List<ContractViewModel>();
        }
    }
}