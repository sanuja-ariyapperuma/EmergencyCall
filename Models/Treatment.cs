using System.ComponentModel.DataAnnotations.Schema;

namespace Ambulance.Models
{
    public class Treatment
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int DoctorId { get; set; }
        [ForeignKey(nameof(DoctorId))]
        public virtual UserInfo UserInfo { get; set; }
        public string IncidentId { get; set; }
        [ForeignKey(nameof(IncidentId))]
        public virtual IncidentDetail IncidentDetail { get; set; }
        public DateTime TreatmentTime { get; set; }

    }
}
