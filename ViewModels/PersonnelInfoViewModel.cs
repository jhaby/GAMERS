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
            var sql = @"SELECT b.UserId,b.Firstname,b.Surname,b.Role,b.Email,b.Phone,b.PhotoPath,t.Status
                    FROM bio_data b INNER JOIN authentication_table t ON b.UserId = t.UserId ";
            using (var Conn = new MySqlConnection(Helpers.connectionString))
            {
                 person =  Conn.Query<PersonnelData>(sql).ToList();

            }

            return person;
        }
        public async void SendMessageAsync(string message)
        {
            await Task.Run(() => Helpers.SendMessage(message));
        }
    }
}
