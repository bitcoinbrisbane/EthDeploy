using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

namespace ETHGitHubDeploy.Models
{
    public class Step1ViewModel
    {
        public String Username { get; set;}

        public String Repo { get; set; }

        public String Branch { get; set; }

        public IEnumerable<String> Branches { get; set; }

        // public Step1ViewModel(String username, String repo)
        // {
        //     this.Username = username;
        //     this.Repo = repo;
        //     this.Branches = new List<String>();
        // }
    }
}