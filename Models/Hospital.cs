using System.ComponentModel.DataAnnotations.Schema;

namespace Ambulance.Models
{
    public class Hospital
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        [InverseProperty("Hospital")]
        public List<IncidentDetail>? Incidents { get; set; }
    }
}
