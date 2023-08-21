using System.ComponentModel.DataAnnotations;

namespace ChatApplication.Application.DTOs.Agent
{
    public class Create_Update_Agent_DTO
    {
        [Required(ErrorMessage = "Please enter Agent Name.")]
        public string Name { get; set; }

        [Range(0, 1, ErrorMessage = "Please enter Seniority between 0 to 1.")]
        public double Seniority { get; set; }

        [Required(ErrorMessage = "Please specify whether Agent member of Overflow Team.")]
        public bool IsOverFlowMember { get; set; }

        [Range(0, 23, ErrorMessage = "Please enter Start Hour between 0 to 23.")]
        public int ShiftStartHour { get; set; }
    }
}
