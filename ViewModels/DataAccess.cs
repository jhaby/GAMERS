using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;

using MySql.Data.MySqlClient;
using Dapper;

namespace GAMERS_TECH
{
   public class DataAccess
    {
        public static async Task<List<T>> LoadDataList<T,U>(string sql, U parameter, string connectionString)
        {
            
                using (var connection = new MySqlConnection(connectionString))
                {
                    var row = await connection.QueryAsync<T>(sql, parameter);
                    return row.ToList();
                }
            
        }

        public static async Task<T> LoadData<T, U>(string sql, U parameter, string connectionString)
        {

            using (var connection = new MySqlConnection(connectionString))
            {
                var row = await connection.QueryAsync<T>(sql, parameter);
                return row.FirstOrDefault();

            }

        }


        public static async Task<int> SaveData<U>(string sql, U parameters, string connectionString)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                int x = await conn.ExecuteAsync(sql, parameters);
                return x;
            }
        }

       

    }
}
