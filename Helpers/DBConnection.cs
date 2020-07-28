using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;


namespace GAMERS_TECH
{
    public class DBConnection
    {
        public static string ConnString = "server=localhost;user=root;database=gamers_356;port=3306;password=255Admin";
        private UserData Person;

        public UserData Login(string username, string password)
        {
             
            try
            {
                string SqlString = $"SELECT UserId,Username,Photo,Access_type,TotalAlerts,MissedAlerts,HandledAlerts FROM authentication_table WHERE Username='{username}' AND Password='{password}' AND Status='active' ";
                using (MySqlConnection Conn = new MySqlConnection(ConnString))
                {
                    Conn.Open();
                    MySqlCommand cmd = new MySqlCommand(SqlString, Conn);
                    MySqlDataReader rd = cmd.ExecuteReader();

                    if (!rd.HasRows)
                    {
                        Person = new UserData()
                        {
                            UserId = "none",
                            Username = "none",
                            Photo = "",
                            
                        };
                    }
                    else
                    {

                        while (rd.Read())
                        {
                            Person = new UserData()
                            {
                                UserId = rd[0].ToString(),
                                Username = rd[1].ToString(),
                                Photo = rd[2].ToString(),
                                Accesstype = rd[3].ToString(),
                                TotalAlerts = Convert.ToInt32(rd[4]),
                                MissedAlerts = Convert.ToInt32(rd[5]),
                                HandledAlerts = Convert.ToInt32(rd[6])

                            };
                        }
                    }
                    rd.Close();
                }
            }
            catch
            {
                Person = new UserData()
                {
                    UserId = "none",
                    Username = "none",
                    Photo = ""
                };
            }
            return Person;
                
        }

       
    }
}
