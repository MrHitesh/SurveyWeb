using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly SurveyContext _db;

        public IndexModel(ILogger<IndexModel> logger, SurveyContext db)
        {
            _logger = logger;
            this._db = db;
        }

        public void OnGet()
        {
            LoadSampleData();

            var surveys = _db.Surveys
                .Include(a => a.Questions)
                .ThenInclude(a => a.Choices)
                .ToList();
        }

        private void LoadSampleData()
        {
            if (_db.Surveys.Count() == 0)
            {
                List<Survey> surveys = new List<Survey>();
                Survey survey1 = new Survey();
                survey1.Title = "Survey 1 Test Title";
                survey1.Description = "Survey 1 Description";
                survey1.Questions = new List<Question>()
                {
                    new Question() { 
                    QuestionText = "This is a question 1 of a test survey. Please enter your response below.",
                    Choices = new List<Choice>() {
                    new Choice() { ChoiceText = "This is choice 1 of question 1 of survey 1."},
                    new Choice() { ChoiceText = "This is choice 2 of question 1 of survey 1."},
                    new Choice() { ChoiceText = "This is choice 3 of question 1 of survey 1."},
                    new Choice() { ChoiceText = "This is choice 4 of question 1 of survey 1."},
                    }
                    },

                    new Question() {
                    QuestionText = "This is a question 2 of a test survey. Please enter your response below.",
                    Choices = new List<Choice>() {
                    new Choice() { ChoiceText = "This is choice 1 of question 2 of survey 1"},
                    new Choice() { ChoiceText = "This is choice 2 of question 2 of survey 1."},
                    new Choice() { ChoiceText = "This is choice 3 of question 2 of survey 1."},
                    new Choice() { ChoiceText = "This is choice 4 of question 2 of survey 1."},
                    }
                    },

                    new Question() {
                    QuestionText = "This is a question 3 of a test survey. Please enter your response below.",
                    Choices = new List<Choice>() {
                    new Choice() { ChoiceText = "This is choice 1 of question 3 of survey 1."},
                    new Choice() { ChoiceText = "This is choice 2 of question 3 of survey 1."},
                    new Choice() { ChoiceText = "This is choice 3 of question 3 of survey 1."},
                    new Choice() { ChoiceText = "This is choice 4 of question 3 of survey 1."},
                    }
                    },

                    new Question() {
                    QuestionText = "This is a question 4 of a test survey. Please enter your response below.",
                    Choices = new List<Choice>() {
                    new Choice() { ChoiceText = "This is choice 1 of question 4 of survey 1."},
                    new Choice() { ChoiceText = "This is choice 2 of question 4 of survey 1."},
                    new Choice() { ChoiceText = "This is choice 3 of question 4 of survey 1."},
                    new Choice() { ChoiceText = "This is choice 4 of question 4 of survey 1."},
                    }
                    }
                };
                survey1.CreatedBy = User.Identity.Name;
                survey1.CreatedOn = DateTime.Now;
                survey1.ExpiresOn = DateTime.Now.AddDays(15);


                Survey survey2 = new Survey();
                survey2.Title = "Survey 2 Test Title";
                survey2.Description = "Survey 2 Description";
                survey2.Questions = new List<Question>()
                {
                    new Question() {
                    QuestionText = "This is a question 1 of a test survey. Please enter your response below.",
                    Choices = new List<Choice>() {
                    new Choice() { ChoiceText = "This is choice 1 of question 1 of survey 2."},
                    new Choice() { ChoiceText = "This is choice 2 of question 1 of survey 2."},
                    new Choice() { ChoiceText = "This is choice 3 of question 1 of survey 2."},
                    new Choice() { ChoiceText = "This is choice 4 of question 1 of survey 2."},
                    }
                    },

                    new Question() {
                    QuestionText = "This is a question 2 of a test survey. Please enter your response below.",
                    Choices = new List<Choice>() {
                    new Choice() { ChoiceText = "This is choice 1 of question 2 of survey 2"},
                    new Choice() { ChoiceText = "This is choice 2 of question 2 of survey 2."},
                    new Choice() { ChoiceText = "This is choice 3 of question 2 of survey 2."},
                    new Choice() { ChoiceText = "This is choice 4 of question 2 of survey 2."},
                    }
                    },

                    new Question() {
                    QuestionText = "This is a question 3 of a test survey. Please enter your response below.",
                    Choices = new List<Choice>() {
                    new Choice() { ChoiceText = "This is choice 1 of question 3 of survey 2."},
                    new Choice() { ChoiceText = "This is choice 2 of question 3 of survey 2."},
                    new Choice() { ChoiceText = "This is choice 3 of question 3 of survey 2."},
                    new Choice() { ChoiceText = "This is choice 4 of question 3 of survey 2."},
                    }
                    },

                    new Question() {
                    QuestionText = "This is a question 4 of a test survey. Please enter your response below.",
                    Choices = new List<Choice>() {
                    new Choice() { ChoiceText = "This is choice 1 of question 4 of survey 2."},
                    new Choice() { ChoiceText = "This is choice 2 of question 4 of survey 2."},
                    new Choice() { ChoiceText = "This is choice 3 of question 4 of survey 2."},
                    new Choice() { ChoiceText = "This is choice 4 of question 4 of survey 2."},
                    }
                    }
                };
                survey2.CreatedBy = User.Identity.Name;
                survey2.CreatedOn = DateTime.Now;
                survey2.ExpiresOn = DateTime.Now.AddDays(15);

                surveys.Add(survey1);
                surveys.Add(survey2);

                _db.AddRange(surveys);
                _db.SaveChanges();
            }
        }
    }
}
