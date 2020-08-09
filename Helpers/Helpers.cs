using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace GAMERS_TECH
{
    public class Helpers
    {   
        private static readonly string connectionString = "server=localhost;user=root;database=gamers_356;port=3306;password=255Admin";

        /*Section for loading information from database tables*/

        public static async Task<UserData> LoadLoginInfo(string user, string pass)
        {
            string SqlString = @"SELECT a.UserId,a.Username,a.Photo,
                                a.AccessType,a.TotalAlerts,a.MissedAlerts,
                                a.HandledAlerts, b.Firstname, b.Surname,b.Language, a.Status
                                FROM authentication_table a
                                INNER JOIN bio_data b
                                ON a.Username=@username AND a.Password=@password
                                AND a.UserId = b.UserId ";

            return await DataAccess.LoadData<UserData, dynamic>(SqlString, new { username = user, password = pass },connectionString);

        }


        /*Section for adding information to the database */
        public static async Task<int> InsertLoginInfo(CreateUserData userdata)
        {
            userdata.PhotoPath = "pack://application:,,,/Resources/avatar2.png";

            string sql = @"INSERT INTO bio_data(UserId,Firstname,Surname,Othername,Role,Phone,
                        Alt_phone,Email,Culture,Language,PhotoPath) VALUES(@PrevUid,@Firstname,@Surname,@Othername,@Role,
                         @Phone,@AltPhone,@Email,@Tribe,@Language,@PhotoPath) ";

            string sql2 = @"INSERT INTO authentication_table(UserId,Username,AccessType,Photo)
                            VALUES(@PrevUid,@Firstname,@Role,@PhotoPath) ";

           int resultcode = await DataAccess.SaveData(sql,userdata , connectionString);
           int result2 = await DataAccess.SaveData(sql2,new {PrevUid=userdata.PrevUid,Firstname=userdata.Firstname,Role=userdata.Role,PhotoPath=userdata.PhotoPath }, connectionString);
           return result2;
            //new { UserId = userdata.PrevUid, Firstname = userdata.Firstname, Role = userdata.Role, Photo = userdata.PhotoPath }
        }



        /*Section for updating the data in the database */
        public async Task UpdateLoginInfo()
        {
            string sql = "update LoginTable set FullName = @FullName where AccessType = @AccessType";

            await DataAccess.SaveData(sql, new { FullName="", AccessType="" }, connectionString);

        }

        public async Task UpdatePersonnelInfo()
        {
            string sql = "update Personnel set Name = @Name where Role=@Role";

            await DataAccess.SaveData(sql, new { Name="", Role="" }, connectionString);

        }

        /*Section for deleting the Data in the database */
        public async Task DeleteLoginInfo()
        {
            string sql = "delete from LoginTable where FullName = @FullName";

            await DataAccess.SaveData(sql, new { FullName="" }, connectionString);

        }

        public async Task DeletePersonnelInfo()
        {
            string sql = "delete from Personnel where Name = @Name";

            await DataAccess.SaveData(sql, new { Name=" " }, connectionString);

        }
        public static async Task SendMessage(string message)
        {
            string sql = "delete from Personnel where Name = @Name";

            await DataAccess.SaveData(sql, new { Name = " " }, connectionString);
        }

        public static async Task UpdateStatus(string status, string userid)
        {
            string sql = "UPDATE authentication_table SET Status=@Status WHERE UserId=@UserId";
            await DataAccess.SaveData(sql, new { Status = status, UserId = userid }, connectionString);
        }

        public static async Task<List<HistoryModel>> LoadHistoryAsync(string view)
        {
            HistoryModel history = new HistoryModel();
            string sql;
            if(view == "dashboard")
            {
               sql = "SELECT DateTime,CaseId,Village,Description FROM recent_activities";
            }
            else
            {
                sql = "SELECT * FROM recent_activities";
            }
            
            return await DataAccess.LoadDataList<HistoryModel,dynamic>(sql,history, connectionString);
        }
        public static async Task<List<AgentsModel>> LoadAgents()
        {
            string sql = "SELECT UserId,Username,Status,Photo FROM authentication_table WHERE AccessType=@Agent";
             return await DataAccess.LoadDataList<AgentsModel,dynamic>(sql, new { Agent="agent"}, connectionString);

            
        }

        public static async Task<List<CasesModel>> LoadCases()
        {
            string sql = "SELECT * FROM active_cases WHERE Status=@Status";
            return await DataAccess.LoadDataList<CasesModel, dynamic>(sql, new { Status="pending" }, connectionString);

        }
        
    }
}
