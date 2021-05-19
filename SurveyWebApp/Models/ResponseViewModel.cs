using EFDataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyWebApp.Models
{
    public class ResponseViewModel
    {
        private string employeeId;

        public string EmployeeId { 
            get => employeeId?.Trim(); 
            set => employeeId = value; 
        }
        public Survey Survey { get; set; }
        public List<SurveyResponse> Responses { get; set; } = new List<SurveyResponse>();
    }
}
