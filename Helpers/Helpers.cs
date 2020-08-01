using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace GAMERS_TECH
{
    public class Helpers
    {   
        public static string connectionString = "Server=db4free.net,3306;Database=gamers_server;User Id=arnold; Password=Arn0ld!nsql";
        public static string connectionString2 = "Server=localhost;Database=gamers_356;Uid=root;Pwd=255Admin";

        /*Section for loading information from database tables*/
        UserData loginInfo;

        public async Task<UserData> LoadLoginInfo(string user, string pass)
        {
            try
            {
                loginInfo = new UserData();
                string sql = "SELECT UserId,Username,Photo,AccessType,TotalAlerts,MissedAlerts,HandledAlerts FROM LoginTable WHERE Username=@user and Password=@pass ";
                loginInfo = await DataAccess.LoadData(sql, user,pass, connectionString);
                return loginInfo;
            }
            catch(Exception ex)
            {
                UserData loginInfo = new UserData()
                {
                        UserId="none",
                        Username="none",
                        AccessType = "custom" +ex.Message + ex.Source+ex.TargetSite
              
                };
                return loginInfo;
            }
        }

       

        /*Section for adding information to the database */
        public async Task InsertLoginInfo()
        {
            string sql = "insert into LoginTable (FullName, AccessType, TotalAlerts, HandledAlerts, MissedAlerts, Language) values (@FullName, @AccessType, @TotalAlerts, @HandledAlerts, @MissedAlerts, @Language)";

            await DataAccess.SaveData(sql, new { FullName="", AccessType="", TotalAlerts=0, HandledAlerts=0, MissedAlerts=0, Language="English"}, connectionString);

        }

        public async Task InsertPersonnelInfo()
        {
            string sql = "insert into Personnel (Name, Role, Email, Phone, filepath) values (@Name, @Role, @Email, @Phone, @filepath)";

            await DataAccess.SaveData(sql, new { Name="", Role="", Email="", Phone="", filepath="" }, connectionString);

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
    }
}
