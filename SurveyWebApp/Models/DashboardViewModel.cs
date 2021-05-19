using EFDataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyWebApp.Models
{
    public class DashboardViewModel
    {
        public List<SurveyDetails> SurveyDetails { get; set; } = new List<SurveyDetails>();

    }

    public class SurveyDetails
    {
        public Survey Survey { get; set; }
        public int ResponseCount { get; set; }
    }
}
