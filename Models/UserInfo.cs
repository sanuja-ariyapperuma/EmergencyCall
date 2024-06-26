﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Ambulance.Models
{
    public class UserInfo
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string Email { get; set; }
        [MaxLength(10,ErrorMessage = "Contact number should be 10 digit")]
        public string? Contact { get; set; }
        //[IgnoreDataMember]
        public string? Password { get; set; }
        public Boolean Is_active { get; set; }
        public int User_role { get; set; }
        [ForeignKey(nameof(User_role))]
        public virtual UserRole? UserRole { get; set; }
        public List<IncidentDetail>? Ambulances { get; set; }
        public List<IncidentDetail>? Doctors { get; set; }
        public List<IncidentDetail>? DischargedDoctors { get; set; }
        [InverseProperty("UserInfo")]
        public List<Treatment>? Treatments { get; set; }
    }
}
