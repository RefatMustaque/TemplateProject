using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TemplateProject.EntityModels
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            LoginHistoryList = new List<LoginHistory>();
        }

        public string Type { get; set; }

        public string UserTypeId { get; set; }

        public string RegisteredById { get; set; }

        public DateTime? RegistrationDateTime { get; set; }

        private List<LoginHistory> LoginHistoryList { get; set; }

        public string ParentUserId { get; set; }
    }
}