using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace GAMERS_TECH
{
    public class PersonnelInfoViewModel
    {
        public List<PersonnelData> List { get; private set; }

        public PersonnelInfoViewModel()
        {
            List = GetData();

        }
        public List<PersonnelData> GetData()
        {
            
            var person = new List<PersonnelData>();
            using (MySqlConnection Conn = new MySqlConnection(DBConnection.ConnString))
            {
                Conn.Open();
                string loadstring = "SELECT * FROM bio_data";
                MySqlCommand cmd = new MySqlCommand(loadstring, Conn);
                MySqlDataReader rd = cmd.ExecuteReader();

                if (rd.HasRows)
                {
                    while (rd.Read())
                    {
                        person.Add(new PersonnelData
                        {
                            Name = $"Name: {rd[1].ToString()} {rd[2].ToString()} {rd[3].ToString()}",
                            Role = $"Role: {rd[4].ToString()}",
                            Email = $"Email: {rd[8].ToString()}",
                            Phone = $"Phone: {rd[6].ToString()}",
                            Filepath = "pack://application:,,,/Resources/avatar2.png"

                        });
                    }
                }
            }

            
            return person;
        }
    }
}
