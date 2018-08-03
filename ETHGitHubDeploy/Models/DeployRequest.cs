using System;
using System.ComponentModel;

namespace ETHGitHubDeploy.Models
{
    public class DeployRequest
    {
        public String Username { get; set; }
        
        public String Repo { get; set; }
        
        public String Branch { get; set; }
        
        public String Contract { get; set; }
        
        [DisplayName("Keystore / JSON File ")]
        public String KeyFile { get; set; }
        
        [DisplayName("Mnemonic")]
        public String Password { get; set; }
        
        public String Compiler { get; set; }
        
        public String Network { get; set; }
        
        public String Node { get; set; }
        
        public Int64 Gas { get; set; }
        
        public DeployRequest()
        {
			this.Compiler = "0.4.23";
			this.Network = "Rinkeby";
			this.Node = "https://infura.io";
			this.Password = "whip venture public clip similar debris minimum mandate despair govern rotate swim";
			this.KeyFile = "0x1B70EA1E5f0fF005794AAA79465D4B7D2C664E36";
			this.Gas = 2000000;
        }
    }
}