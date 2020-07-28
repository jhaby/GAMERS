using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace GAMERS_TECH
{
    public class PersonnelInfoViewModel
    {
        public List<PersonnelData> list { get; private set; }

        public PersonnelInfoViewModel()
        {
            list = GetData();

        }
        public List<PersonnelData> GetData()
        {

            using(MySqlConnection Conn = new MySqlConnection(DBConnection.ConnString))
            {
                Conn.Open();
                string loadstring = "SELECT * FROM bio_data";
                MySqlCommand cmd = new MySqlCommand(loadstring, Conn);
                MySqlDataReader rd = cmd.ExecuteReader();
            }

            var person = new List<PersonnelData>
            {
                new PersonnelData
                {
                    Name = "Habizana Jeremiah",
                    Role = "Agent",
                    Email = "habizana@doctorsarch.org",
                    Phone = "0780161616",
                    Filepath = "pack://application:,,,/Resources/avatar2.png"

                }
            };
            return person;
        }
    }
}
