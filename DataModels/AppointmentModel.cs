using System;
using System.Collections.Generic;
using System.Text;

namespace GAMERS_TECH
{
    public class AppointmentModel
    {
        public string CaseId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime Start { get; set; }
        public string Location { get; set; }
    }
}
