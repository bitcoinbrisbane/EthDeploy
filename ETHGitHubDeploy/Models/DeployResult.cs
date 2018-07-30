using System;
namespace ETHGitHubDeploy.Models
{
    public class DeployResult : ContractDetails
    {
        public String Address { get; set; }
        
        public String TxID { get; set; }
        
        public DeployResult()
        {
        }
    }
}
