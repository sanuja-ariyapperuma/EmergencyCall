using System.ComponentModel.DataAnnotations;

namespace Ambulance.Models.ViewModels
{
    public class VM_IncidentDetail
    {
        public string Id { get; set; }
        public string PatientName { get; set; }
        public int PatientAge { get; set; }
        public string PatientAddress { get; set; }
        public string PatientContact { get; set; }
        public string Description { get; set; }
        public DateTime DischargedTime { get; set; }
        public string ImportantNotes { get; set; }
        public DateTime PickupTime { get; set; }
        public DateTime CompletedTime { get; set; }
        public int PatientStatus { get; set; }
        [Range(0,1)]
        public int Gender { get; set; }
        public string GardianName { get; set; }
        public string GardianContact { get; set; }


    }
}
