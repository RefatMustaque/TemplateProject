using TemplateProject.Manager.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TemplateProject.EntityModels;
using TemplateProject.Repository.Repositories;

namespace TemplateProject.Manager.Managers
{
    public class LoginHistoryManager : Manager<LoginHistory>
    {
        private LoginHistoryRepository LoginHistoryRepository { get; set; }

        public LoginHistoryManager(LoginHistoryRepository repository) : base(repository)
        {
            LoginHistoryRepository = repository;
        }
    }
}
