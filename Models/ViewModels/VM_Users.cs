using System.ComponentModel.DataAnnotations;

namespace Ambulance.Models.ViewModels
{
    public class VM_Users
    {
        public int MyProperty { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        [MaxLength(10)]
        public string? Contact { get; set; }
        public Boolean Is_active { get; set; }
        public int User_role { get; set; }
    }
}
