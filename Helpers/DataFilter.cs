using System;
using System.Collections.Generic;
using System.Text;

namespace GAMERS_TECH
{
    public class DataFilter
    {
        public static string GenerateSqlQuery(int filter,string condition, string param1, string param2)
        {
            string sql = "";
            switch (filter)
            {
               
                case 1:
                    sql = String.Format("SELECT * FROM patient_data WHERE {0} {1} '{2}'",condition,param1,param2);
                    break;
                case 2:
                    sql = String.Format("SELECT * FROM patient_data WHERE {0} BETWEEN '{1}' AND '{2}'", condition, param1, param2);
                    break;
            }

            return sql;
        }
    }
}
