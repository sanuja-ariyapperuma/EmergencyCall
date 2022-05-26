using System.ComponentModel.DataAnnotations;

namespace Ambulance.Models.ViewModels
{
    public class VM_Treatment
    {
        [Required(ErrorMessage ="No Description Found")]
        public string Description { get; set; }
        [Required(ErrorMessage = "No Incident Identification Found")]
        public string IncidentId { get; set; }
        public string? Doctor { get; set; }
        public DateTime? TreatmentTime { get; set; }
    }
}
