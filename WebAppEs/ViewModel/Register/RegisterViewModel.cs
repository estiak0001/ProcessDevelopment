using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebAppEs.Models;

namespace WebAppEs.ViewModel.Register
{
    public class RegisterViewModel
    {
        
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and confirmation password not match.")]
        public string ConfirmPassword { get; set; }

        //extra
        [Required]
        public string Department { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string EmployeeID { get; set; }

        public RoleViewModel[] Roles { get; set; }
    }
}
