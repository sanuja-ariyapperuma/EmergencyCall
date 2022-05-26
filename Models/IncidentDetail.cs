using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ambulance.Models
{
    public class IncidentDetail
    {
        public string Id { get; set; }
        public string PatientName { get; set; }
        public int PatientAge { get; set; }
        public string PatientAddress { get; set; }
        [StringLength(10, ErrorMessage = "Patient contact number should be 10 digits")]
        public string PatientContact { get; set; }
        public string Description { get; set; }
        public DateTime DischargedTime { get; set; }
        public string ImportantNotes { get; set; }
        public DateTime PickupTime { get; set; }
        public DateTime DropOffTime { get; set; }
        public int PatientStatus { get; set; }
        [Range(0, 1)]
        public int Gender { get; set; }
        public string GardianName { get; set; }
        [StringLength(10, ErrorMessage = "Gardian contact number should be 10 digits")]
        public string GardianContact { get; set; }
        public int HospitalId { get; set; }
        [ForeignKey(nameof(HospitalId))]
        public virtual Hospital? Hospital { get; set; }
        public int AmbulanceId { get; set; }
        [ForeignKey(nameof(AmbulanceId))]
        public virtual UserInfo? Ambulance { get; set; }
        public int? DoctorId { get; set; }
        [ForeignKey(nameof(DoctorId))]
        public virtual UserInfo? Doctor { get; set; }
        public int? DischargedDoctorId { get; set; }
        [ForeignKey(nameof(DischargedDoctorId))]
        public virtual UserInfo? DischargedDoctor { get; set; }

    }
}
