using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TemplateProject.Helpers
{
    public class DateTimeHelper
    {
        public DateTime BDDateTime()
        {
            DateTime utcTime = DateTime.UtcNow;
            TimeZoneInfo BdZone = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
            DateTime localDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, BdZone);
            return localDateTime;
        }
    }
}
