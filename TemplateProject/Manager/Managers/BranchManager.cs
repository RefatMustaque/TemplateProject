using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TemplateProject.EntityModels;
using TemplateProject.Manager.Base;
using TemplateProject.Repository.Repositories;

namespace TemplateProject.Manager.Managers
{
    public class BranchManager : Manager<Branch>
    {
        private BranchRepository _repository;

        public BranchManager(BranchRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public List<Branch> GetAllWithEmployee()
        {
            return _repository.GetAllWithEmployee();
        }
    }
}
