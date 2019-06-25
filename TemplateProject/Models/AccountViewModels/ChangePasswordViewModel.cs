using System.ComponentModel.DataAnnotations;

namespace TemplateProject.Models.AccountViewModels
{
    public class ChangePasswordViewModel
    {
        [Display(Name ="Old Password")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
