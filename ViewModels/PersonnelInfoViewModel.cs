using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;
using Dapper;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;

namespace GAMERS_TECH
{
    public class PersonnelInfoViewModel
    {
        public List<PersonnelData> List { get; private set; }
        public PersonnelInfoViewModel()
        { 
            List =  GetData();

        }
        public List<PersonnelData> GetData()
        {
            HttpClient client = new HttpClient();
            var result="";
            Task.Run(async delegate
            {
                var response = await client.GetAsync(Environment.GetEnvironmentVariable("GamersServerUri")+"/personnelinfo");
                result = await response.Content.ReadAsStringAsync();
            });

            var person = JsonConvert.DeserializeObject<List<PersonnelData>>(result) ?? null ;

            return person;
        }
        
    }
}
