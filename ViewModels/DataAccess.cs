using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;

using MySql.Data.MySqlClient;
using Dapper;
using MySql.Data;
using System.Data.SqlClient;

namespace GAMERS_TECH
{
   class DataAccess
    {
        public static async Task<UserData> LoadData(string sql,string user, string pass, string connectionString)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    var row = await connection.QueryAsync<UserData>(sql,new {Username= user, Password=pass } );
                    return row.FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       

        public static async Task SaveData<U>(string sql, U parameters, string connectionString)
        {
            using (IDbConnection connection = new MySqlConnection(connectionString))
            {
                 await connection.ExecuteAsync(sql, parameters);
                
            }
        }

    }
}
