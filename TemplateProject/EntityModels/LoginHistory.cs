using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TemplateProject.EntityModels
{
    public class LoginHistory
    {
        public long Id { get; set; }

        [StringLength(45)]
        public string IpAddress { get; set; }

        [StringLength(40)]
        public string MacAddress { get; set; }

        [StringLength(450)]
        public string DeviceInfo { get; set; }

        public DateTime LoginDateTime { get; set; }

        [StringLength(450)]
        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}
