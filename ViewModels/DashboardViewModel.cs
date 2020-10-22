using System;
using System.Collections.Generic;
using System.Text;

namespace GAMERS_TECH
{
    public class DashboardViewModel
    {
        List<AgentsModel> Users;
        public DashboardViewModel()
        {
            Users = Helpers.LoadAgents().Result;
        }
        
        public List<AgentsModel> Agents
        {
            get
            {
                return Users;
            }
            set { Users = value; }
        }
    }
}
