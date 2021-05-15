using EFDataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyWebApp.Models
{
    public class ResultViewModel
    {
        public Survey Survey { get; set; }
        public List<SurveyResponse> Responses { get; set; }
        public List<ResponseChartData> ResponseChartData { get; set; }
    }

    public class ResponseChartData
    {
        public string QuestionText { get; set; }
        public List<ChoiceToCount> ChartRows { get; set; } = new List<ChoiceToCount>();
    }

    public class ChoiceToCount
    {
        public string ChoiceText { get; set; }
        public int Count { get; set; }
    }
}
