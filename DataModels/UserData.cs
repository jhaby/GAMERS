using System;
using System.Collections.Generic;
using System.Text;

namespace GAMERS_TECH
{
    public class UserData
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Photo { get; set; }
        public string AccessType { get; set; }
        public int TotalAlerts { get; set; }
        public int HandledAlerts { get; set; }
        public int MissedAlerts { get; set; }
    }

    public class PersonelInfo
    {
        public string UserId { get; set; }
        public string Fullname { get; set; }
        public string Photo { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string Status { get; set; }
        public string Role { get; set; }
        public string Village { get; set; }
        public string Kin { get; set; }
    }
}
