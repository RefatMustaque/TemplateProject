using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TemplateProject.EntityModels;
using TemplateProject.Models.EmployeeViewModels;
using System.Globalization;

namespace TemplateProject.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //EmployeeController
            CreateMap<EmployeeCreateVm, Employee>();

            CreateMap<Employee, EmployeeGridVm>();

            CreateMap<Employee, EmployeeEditVm>();

            CreateMap<EmployeeEditVm, Employee>();
        }
    }
}
