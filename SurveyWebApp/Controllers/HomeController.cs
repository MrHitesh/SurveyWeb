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
        public IActionResult Create(Survey survey)
        {
            return View(survey);
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
                            else
                            {
                                existingQues.Choices.Add(choice);
                            }
                        }
                    }
                    else
                    {
                        surveyFromdb.Questions.Add(que);
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

        public IActionResult Results()
        {
            return View();
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
