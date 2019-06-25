using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TemplateProject.EntityModels
{
    public class Employee
    {
        [Key]
        [StringLength(450)]
        public string Id { get; set; }

        [StringLength(120)]
        public string Name { get; set; }

        [StringLength(10)]
        public string Gender { get; set; }

        [Column(TypeName = "Date")]
        public DateTime JoinDate { get; set; }

        public long BranchId { get; set; }

        public Branch Branch { get; set; }

        [StringLength(450)]
        public string CreatedByUserId { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        [StringLength(450)]
        public string UpdatedByUserId { get; set; }

        public DateTime? UpdatedDateTime { get; set; }

        public string Department { get; set; }
    }
}
