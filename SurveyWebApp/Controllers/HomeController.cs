using EFDataAccessLibrary;
using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SurveyWebApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly SurveyContext _db;
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(SurveyContext db, ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor)
        {
            this._db = db;
            _logger = logger;
            this._httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            Seed.SeedData(_db, _httpContextAccessor);
            ViewData.Model = _db.Surveys.Where(x => x.CreatedBy == User.Identity.Name);
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Survey ajaxSurvey)
        {

            var survey = _db.Surveys.Find(ajaxSurvey.Id);

            if (survey != null)
            {
                survey.Title = ajaxSurvey.Title;
                survey.Description = ajaxSurvey.Description;
                survey.ExpiresOn = ajaxSurvey.ExpiresOn;
                survey.Questions = ajaxSurvey.Questions;
            }
            else
            {
                survey = new Survey();
                survey.Title = ajaxSurvey.Title;
                survey.Description = ajaxSurvey.Description;
                survey.CreatedBy = User.Identity.Name;
                survey.ExpiresOn = ajaxSurvey.ExpiresOn;
                survey.CreatedOn = DateTime.Now;
                survey.Questions = ajaxSurvey.Questions;

                _db.Surveys.Add(survey);
            }

            _db.SaveChanges();
            return Ok(survey);
        }

        public IActionResult Edit(int? id)
        {
            Survey survey = new Survey();

            if (id != null)
            {
                survey = _db.Surveys
                .Include(a => a.Questions)
                .ThenInclude(x => x.Choices)
                .FirstOrDefault(y => y.Id == id);
                return View(survey);
            }
            else
            {
                return View();
            }


        }

        [HttpPost]
        public IActionResult Edit(Survey FormSurvey)
        {
            var surveyFromdb = _db.Surveys.Include(a => a.Questions)
                .ThenInclude(x => x.Choices)
                .FirstOrDefault(y => y.Id == FormSurvey.Id);

            if (surveyFromdb != null)
            {
                surveyFromdb.Title = FormSurvey.Title;
                surveyFromdb.Description = FormSurvey.Description;

                foreach (Question que in FormSurvey.Questions)
                {
                    var existingQues = surveyFromdb.Questions.Where(x => x.Id == que.Id).FirstOrDefault();
                    if (existingQues != null)
                    {
                        existingQues.QuestionText = que.QuestionText;
                        foreach (var choice in que.Choices)
                        {
                            var existingChoice = existingQues.Choices.Where(x => x.Id == choice.Id).FirstOrDefault();
                            if (existingChoice != null)
                            {
                                existingChoice.ChoiceText = choice.ChoiceText;
                            }
                        }
                    }
                }
            }
            else
            {
                _db.Surveys.Add(FormSurvey);
            }

            _db.SaveChanges();

            var s = _db.Surveys
                .Include(a => a.Questions)
                .ThenInclude(x => x.Choices)
                .FirstOrDefault(y => y.Id == FormSurvey.Id);
            return View(s);
        }

        [HttpPost]
        public IActionResult DeleteQuestion(int id)
        {
            var questionToDelete = _db.Questions
                .Include(a => a.Choices)
                .FirstOrDefault(x => x.Id == id);
            foreach (var choice in questionToDelete.Choices)
            {
                _db.Choices.Remove(choice);
            }

            _db.Questions.Remove(questionToDelete);
            _db.SaveChanges();

            return Ok(questionToDelete);
        }
        public IActionResult Results(int surveyId)
        {
            surveyId = 45;
            List<SurveyResponse> srs = _db.Responses
                .Include(y=>y.Choice)
                .Include(a=>a.Survey)
                .ThenInclude(x=>x.Questions)
                .Where(x => x.Survey.Id == surveyId).ToList();
            ResultViewModel vm = new ResultViewModel();
            vm.Survey = srs.FirstOrDefault().Survey;
            vm.Responses = srs;

            vm.ResponseChartData = new List<ResponseChartData>();
            
            foreach (var ques in vm.Survey.Questions)
            {
                var rcd = new ResponseChartData();
                rcd.ChartRows = new List<ChoiceToCount>();
                rcd.QuestionText = ques.QuestionText;
                var responseChoices = vm.Responses.Where(x => x.Question.Id == ques.Id);
                var choiceToCountList = from r in responseChoices
                         group r by r.Choice into grp
                         select new ChoiceToCount() { ChoiceText = grp.Key.ChoiceText, Count = grp.Count() };

                rcd.ChartRows.AddRange(choiceToCountList);
                vm.ResponseChartData.Add(rcd);
            }

            

            return View(vm);
        }

        [HttpGet]
        public IActionResult UserResponse(int surveyId, string employeeId)
        {
            Random gen = new Random();
            employeeId = gen.Next(0, 999999).ToString();
            ResponseViewModel vm = new ResponseViewModel();
            vm.Survey = _db.Surveys.AsNoTracking()
                .Include(a => a.Questions)
                .ThenInclude(x => x.Choices)
                .FirstOrDefault(y => y.Id == surveyId);

            foreach (var item in vm.Survey.Questions)
            {
                vm.Responses.Add(new SurveyResponse() {  EmployeeId = employeeId});
            }

            return View(vm);
        }
        [HttpPost]
        public IActionResult UserResponse(ResponseViewModel responseViewModel)
        {
            foreach (var response in responseViewModel.Responses)
            {
                response.Choice = _db.Choices.Find(response.Choice.Id);
                response.Survey = _db.Surveys.Find(response.Survey.Id);
                response.Question = _db.Questions.Find(response.Question.Id);
                _db.Responses.Add(response);
                
            }
            _db.SaveChanges();

            return View("ResponseRecieved");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
