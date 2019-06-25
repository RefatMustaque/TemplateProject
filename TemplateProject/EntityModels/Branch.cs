using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TemplateProject.EntityModels
{
    public class Branch
    {
        public Branch()
        {
            Employees = new List<Employee>();
        }
        public long Id { get; set; }

        [StringLength(120)]
        public string Name { get; set; }

        public List<Employee> Employees { get; set; }
    }
}
