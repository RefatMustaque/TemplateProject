using TemplateProject.Repository.Base;
using TemplateProject.Repository;
using TemplateProject.EntityModels;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TemplateProject.Data;

namespace TemplateProject.Repository.Repositories
{
    public class BranchRepository : Repository<Branch>
    {
        private TemplateProjectDbContext _dbContext { get; set; }

        public BranchRepository(TemplateProjectDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Branch> GetAllWithEmployee()
        {
            return Table.Include(x => x.Employees).ToList();
        }
    }
}
