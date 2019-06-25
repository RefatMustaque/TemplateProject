using TemplateProject.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TemplateProject.Data;
using TemplateProject.EntityModels;

namespace TemplateProject.Repository.Repositories
{
    public class LoginHistoryRepository : Repository<LoginHistory>
    {
        private TemplateProjectDbContext TemplateProjectDbContext { get; set; }

        public LoginHistoryRepository(TemplateProjectDbContext dbContext) : base(dbContext)
        {
            TemplateProjectDbContext = dbContext;
        }
    }
}
