using System.ComponentModel.DataAnnotations.Schema;

namespace Ambulance.Models
{
    public class UserRole
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [InverseProperty("UserRole")]
        public List<UserInfo> Users { get; set; }
    }
}
