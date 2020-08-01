using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;
using Dapper;
using System.Threading.Tasks;
using System.Linq;

namespace GAMERS_TECH
{
    public class DBConnection
    {
        public static string ConnString = "server=localhost;user=root;database=gamers_356;port=3306;password=255Admin";
        

        public static async Task<UserData> Login(string user, string pass)
        {
            string SqlString = "SELECT UserId,Username,Photo,AccessType,TotalAlerts,MissedAlerts,HandledAlerts FROM authentication_table WHERE Username=@username AND Password=@password AND Status='active' ";
            try
            {
                using (var conn = new MySqlConnection(ConnString))
                {
                    var row = await conn.QueryAsync<UserData>(SqlString, new { username = user, password = pass });

                    return row.FirstOrDefault();
                }
            }
            catch(Exception)
            {
                UserData Person = new UserData()
                {
                    UserId = "none",
                    Username = "none",
                    Photo = ""
                };
                return Person;
            }

        }

    }
    
}
