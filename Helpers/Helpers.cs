using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace GAMERS_TECH
{
    public class Helpers
    {
        
        public static string connectionString = "Server="+StaticHelpers.ServerBaseAddress+"; user=root;database=gamers_356;port=3306;password=255Admin";

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

            
                return await DataAccess.LoadData<UserData, dynamic>(SqlString, new { username = user, password = pass }, connectionString);
            
            
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
            string sql = "SELECT * FROM recent_activities";
                        
            var result =  await DataAccess.LoadDataList<HistoryModel,dynamic>(sql,history, connectionString);
            if (result != null)
            {
                foreach(var i in result)
                {
                    int index = result.FindIndex(rs => rs.CaseId.Equals(i.CaseId, StringComparison.Ordinal));

                    string vhtinfo = $"SELECT * FROM vht_info WHERE VHTCode='{i.VHTCode}'";
                    var vht = await DataAccess.LoadData<ResponseViewModel, dynamic>(vhtinfo, new { }, connectionString);
                    result[index].VHTName = vht.Fullname;
                    result[index].VHTPhone = vht.Phone;

                    string emtinfo = $"SELECT TeamId,Transporter,Phone,Type FROM emergency_teams WHERE TeamId='{i.EMTCode}'";
                    var emt = await DataAccess.LoadData<EMTInfo, dynamic>(emtinfo, new { }, connectionString);
                    if(emt != null)
                    {
                        result[index].EMTName = emt.Transporter;
                        result[index].EMTPhone = emt.Phone;
                    }

                    string medicalinfo = $"SELECT ID,Phone,Fullname,Status,Category FROM medical_officers WHERE ID='{i.CHWCode}'";
                    var chw = await DataAccess.LoadData<MedicalInfo, dynamic>(sql, new { }, connectionString);
                    if(chw != null)
                    {
                        result[index].CHWName = chw.Fullname;
                        result[index].CHWPhone = chw.Phone;
                    }


                }
            }
            

            return result;
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

        public static async Task<ResponseViewModel> LoadVHT(string code)
        {
            string sql = "SELECT Fullname,Phone,Kin_phone,Village FROM vht_info WHERE VHTCode = @Code";
            return await DataAccess.LoadData<ResponseViewModel, dynamic>(sql, new { Code = code }, connectionString);
        }
        public static async Task<EMTInfo> LoadEMT(string location)
        {
                string sql = "SELECT TeamId,Transporter,Phone,Type FROM emergency_teams WHERE Region=@Code";
                return await DataAccess.LoadData<EMTInfo, dynamic>(sql, new { Code = location }, connectionString);
            
        }
        public static async Task<MedicalInfo> LoadMedical(string location)
        {
            string sql = "SELECT ID,Phone,Fullname,Status,Category FROM medical_officers WHERE Region=@Code";
            return await DataAccess.LoadData<MedicalInfo, dynamic>(sql, new { Code = location }, connectionString);

        }
        public static async Task<int> ReinstateCase(CasesModel alert)
        {
            string CaseID = alert.CaseId.Split(": ")[1];
            string village = alert.Village.Split(": ")[1];
            string description = alert.Description.Split(": ")[1];

            string sql = @$"INSERT INTO active_cases(CaseId,Location,DateTime,VHTCode,Status,Village,Description)
                                VALUES(@CaseId,@Location,@DateTime,@VHTCode,'pending',
                        @Village,@Description) ";
            string update = @$"UPDATE recent_activities SET Status='Reinstated' WHERE CaseId=@CaseId ";

            try
            {
                await DataAccess.SaveData<dynamic>(sql, new { CaseID, alert.Location, alert.DateTime, alert.VHTCode, village, description }, connectionString);

            }
            catch (Exception)
            {
                string sql2 = $"UPDATE active_cases SET Status='pending' WHERE CaseId='{CaseID}' ";
                await DataAccess.SaveData<dynamic>(sql2, new { }, connectionString);
            }
            return  await DataAccess.SaveData<dynamic>(update, new { CaseID }, connectionString);
            
        }

    }
}
