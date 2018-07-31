using System;
namespace ETHGitHubDeploy.Models
{
    public class ContractDetails
    {
        public String Bin { get; set; }
        
        public String ABI { get; set; }
        
        public String JSON { get; set; }
        
        public ContractDetails()
        {
        }
    }
}