using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TemplateProject.Data;
using TemplateProject.EntityModels;
using TemplateProject.Helpers;
using TemplateProject.Manager.Managers;
using TemplateProject.Repository.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace TemplateProject.UnitOfWorks
{
    public class UnitOfWork
    {
        private TemplateProjectDbContext _dbContext;

        private FileHelper _fileHelper;

        private DropdownHelper _dropdownHelper;

        private DateTimeHelper _dateTimeHelper;

        private UserManager<ApplicationUser> _userManager;

        private SignInManager<ApplicationUser> _signInManager;

        private RoleManager<IdentityRole> _roleManager;

        private BranchManager _BranchManager;

        private EmployeeManager _employeeManager;

        private LoginHistoryManager _loginHistoryManager;

        private ClaimHelper _claimHelper;

        private CryptographyHelper _cryptographyHelper;

        private NetworkHelper _networkHelper;

        private StringGeneratorHelper _stringGeneratorHelper;


        public UnitOfWork(TemplateProjectDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public TemplateProjectDbContext DbContext
        {
            get
            {
                return _dbContext;
            }
        }

        //Helper class
        public FileHelper FileHelper(IHostingEnvironment hostingEnvironment, DateTimeHelper dateTimeHelper)
        {
            _fileHelper = _fileHelper == null ? new FileHelper(hostingEnvironment, dateTimeHelper) : _fileHelper;

            return _fileHelper;
        }

        public CryptographyHelper CryptographyHelper
        {
            get
            {
                _cryptographyHelper = _cryptographyHelper == null ? new CryptographyHelper() : _cryptographyHelper;

                return _cryptographyHelper;
            }
        }

        public NetworkHelper NetworkHelper
        {
            get
            {
                if (_networkHelper == null)
                {
                    _networkHelper = new NetworkHelper();
                }

                return _networkHelper;
            }
        }

        public StringGeneratorHelper StringGeneratorHelper
        {
            get
            {
                if(_stringGeneratorHelper == null)
                {
                    _stringGeneratorHelper = new StringGeneratorHelper();
                }

                return _stringGeneratorHelper;
            }
        }

        public ClaimHelper ClaimHelper
        {
            get
            {
                if (_claimHelper == null)
                {
                    _claimHelper = new ClaimHelper();
                }

                return _claimHelper;
            }
        }

        public DropdownHelper DropdownHelper
        {
            get
            {
                if(_dropdownHelper == null)
                {
                    _dropdownHelper = new DropdownHelper();
                }

                return _dropdownHelper;
            }
        }

        public DateTimeHelper DateTimeHelper
        {
            get
            {
                if (_dateTimeHelper == null)
                {
                    _dateTimeHelper = new DateTimeHelper();
                }

                return _dateTimeHelper;
            }
        }

        //Identity Model
        public UserManager<ApplicationUser> UserManager(IServiceProvider serviceProvider)
        {
            if (_userManager == null)
            {
                _userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            }

            return _userManager;
        }

        public SignInManager<ApplicationUser> SignInManager(IServiceProvider serviceProvider)
        {
            if (_signInManager == null)
            {
                _signInManager = serviceProvider.GetRequiredService<SignInManager<ApplicationUser>>();
            }

            return _signInManager;
        }

        public RoleManager<IdentityRole> RoleManager(IServiceProvider serviceProvider)
        {
            if (_roleManager == null)
            {
                _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            }

            return _roleManager;
        }

        //Entity Model Manager

        public BranchManager BranchManager
        {
            get
            {
                if (_BranchManager == null)
                {
                    _BranchManager = new BranchManager(new BranchRepository(_dbContext));
                }

                return _BranchManager;
            }
        }

        public EmployeeManager EmployeeManager
        {
            get
            {
                if (_employeeManager == null)
                {
                    _employeeManager = new EmployeeManager(new EmployeeRepository(_dbContext));
                }

                return _employeeManager;
            }
        }

        public LoginHistoryManager LoginHistoryManager
        {
            get
            {
                if (_loginHistoryManager == null)
                {
                    _loginHistoryManager = new LoginHistoryManager(new LoginHistoryRepository(_dbContext));
                }

                return _loginHistoryManager;
            }
        }

    }
}
