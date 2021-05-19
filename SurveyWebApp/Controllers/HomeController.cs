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
            //Seed.SeedData(_db, _httpContextAccessor);
            //ViewData.Model = _db.Surveys.Where(x => x.CreatedBy == User.Identity.Name).OrderByDescending(x=>x.CreatedOn);
            var request = _httpContextAccessor.HttpContext.Request;
            var domain = $"{request.Scheme}://{request.Host}";

            DashboardViewModel vm = new DashboardViewModel();

            ViewBag.BaseUrl = domain;
            var activeSurveys = _db.Surveys.Where(x => x.PublishedOn != null && x.ExpiresOn >= DateTime.Now).OrderByDescending(x => x.CreatedOn);

            var responses = _db.Responses
                .Include(x=>x.Survey)
                .Where(x => activeSurveys.Contains(x.Survey)).ToList();

            foreach (var s in activeSurveys)
            {
                var responseCount = responses.Where(x => x.Survey.Id == s.Id).Select(x => x.EmployeeId).Distinct().Count();
                vm.SurveyDetails.Add(new SurveyDetails()
                {
                    Survey = s,
                    ResponseCount = responseCount
                });
            }

            ViewData.Model = vm;
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("Edit");
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

        public IActionResult SurveyList()
        {
            ViewData.Model = _db.Surveys.OrderByDescending(x=>x.CreatedOn);
            return View();
        }

        public IActionResult Edit(int? id)
        {
            ViewBag.SurveysDropDown = _db.Surveys
                .Select(c => new Survey() { Id = c.Id, Title = c.Title, CreatedOn = c.CreatedOn })
                .ToList().OrderByDescending(x => x.CreatedOn);

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

            return RedirectToAction("PreviewAndPublish", new { surveyId = FormSurvey.Id });
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
            ViewBag.SurveysDropDown = _db.Surveys
                .Select(c => new Survey() {  Id = c.Id, Title = c.Title, CreatedOn = c.CreatedOn })
                .ToList().OrderByDescending(x=>x.CreatedOn);

            if (surveyId != 0)
            {


                List<SurveyResponse> srs = _db.Responses
                    .Include(y => y.Choice)
                    .Include(a => a.Survey)
                    .ThenInclude(x => x.Questions)
                    .Where(x => x.Survey.Id == surveyId).ToList();
                ResultViewModel vm = new ResultViewModel();

                if (srs == null || srs.Count() == 0)
                {
                    ViewBag.Message = "No Responses yet.";
                    return View();
                }
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
            return View();
        }

        [HttpGet]
        public IActionResult PreviewAndPublish(int surveyId)
        {
            ViewBag.SurveysDropDown = _db.Surveys
                .Select(c => new Survey() { Id = c.Id, Title = c.Title, CreatedOn = c.CreatedOn })
                .ToList().OrderByDescending(x => x.CreatedOn);

            if (surveyId == 0)
            {
                return View();
            }

            ResponseViewModel vm = new ResponseViewModel();
            vm.Survey = _db.Surveys.AsNoTracking()
                .Include(a => a.Questions)
                .ThenInclude(x => x.Choices)
                .FirstOrDefault(y => y.Id == surveyId);

            foreach (var item in vm.Survey.Questions)
            {
                vm.Responses.Add(new SurveyResponse());
            }

            return View(vm);
        }

        [HttpPost]
        public IActionResult PreviewAndPublish(ResponseViewModel responseViewModel)
        {
            Survey survey = _db.Surveys.Find(responseViewModel.Survey.Id);
            survey.PublishedOn = responseViewModel.Survey.PublishedOn;

            _db.SaveChanges();

            return PreviewAndPublish(responseViewModel.Survey.Id);
        }

        [HttpGet]
        public IActionResult MainSurvey(int surveyId, string employeeId)
        {
            ViewBag.SurveysDropDown = _db.Surveys
                .Select(c => new Survey() { Id = c.Id, Title = c.Title, CreatedOn = c.CreatedOn })
                .ToList().OrderByDescending(x => x.CreatedOn);

            if (surveyId == 0)
            {
                return View();
            }

            ResponseViewModel vm = new ResponseViewModel();
            vm.Survey = _db.Surveys.AsNoTracking()
                .Include(a => a.Questions)
                .ThenInclude(x => x.Choices)
                .FirstOrDefault(y => y.Id == surveyId);

            foreach (var item in vm.Survey.Questions)
            {
                vm.Responses.Add(new SurveyResponse() { });
            }
            vm.EmployeeId = employeeId;

            return View(vm);
        }
        [HttpPost]
        public IActionResult MainSurvey(ResponseViewModel responseViewModel)
        {

            var existing = _db.Responses.Where(x => x.EmployeeId == responseViewModel.EmployeeId && x.Survey.Id == responseViewModel.Survey.Id).ToList();
            if (existing.Count > 0)
            {
                _db.RemoveRange(existing);
            }

            foreach (var response in responseViewModel.Responses)
            {
                response.Choice = _db.Choices.Find(response.Choice.Id);
                response.Survey = _db.Surveys.Find(response.Survey.Id);
                response.Question = _db.Questions.Find(response.Question.Id);
                response.EmployeeId = responseViewModel.EmployeeId;
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
