using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace EFDataAccessLibrary
{
    public class Seed
    {
        public static void SeedData(SurveyContext _db, IHttpContextAccessor httpContextAccessor)
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
                survey1.CreatedBy = httpContextAccessor.HttpContext.User.Identity.Name;
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
                survey2.CreatedBy = httpContextAccessor.HttpContext.User.Identity.Name;
                survey2.CreatedOn = DateTime.Now;
                survey2.ExpiresOn = DateTime.Now.AddDays(15);

                surveys.Add(survey1);
                surveys.Add(survey2);

                _db.AddRange(surveys);
                _db.SaveChanges();
            }
        }

        public static List<SurveyResponse> SeedResponses()
        {
            List<SurveyResponse> srs = new List<SurveyResponse>();
            SurveyResponse sr1 = new SurveyResponse();
            sr1.Question = new Question() { Id = 1, QuestionText = "Question1 Test here for the user." };
            sr1.Survey = new Survey() { 
                Id = 1, Title = "Survey Title",
                Description = "Survey Description",
                Questions = new List<Question>() {
                    new Question() { Id = 1 , QuestionText = "Question from seed data ?"}, 
                    new Question() { Id = 2 }, 
                    new Question() { Id = 3 }, 
                    new Question() { Id = 4} } };
            sr1.Choice = new Choice() { Id = 45, ChoiceText = "Selected choice" };
            srs.Add(sr1);

            SurveyResponse sr7 = new SurveyResponse();
            sr7.Question = new Question() { Id = 1, QuestionText = "Question1 Test here for the user." };
            sr7.Survey = new Survey() { Id = 1, Title = "Survey Title", Description = "Survey Description" };
            sr7.Choice = new Choice() { Id = 46, ChoiceText = "Selected choice2 kanfkjsanv ais vi ashvihaovh ah vasovhohasv shv hovhahvoha vohuahvohavoh avh avhihv haodvh oahv ohvoh aovh oahv ioah vohv vih adivh iahv h vi hvihhasi odvi vihh iadf hasd fi" };
            srs.Add(sr7);

            SurveyResponse sr8 = new SurveyResponse();
            sr8.Question = new Question() { Id = 1, QuestionText = "Question1 Test here for the user." };
            sr8.Survey = new Survey() { Id = 1, Title = "Survey Title", Description = "Survey Description" };
            sr8.Choice = new Choice() { Id = 47, ChoiceText = "Selected choice3" };
            srs.Add(sr8);

            SurveyResponse sr9 = new SurveyResponse();
            sr9.Question = new Question() { Id = 1, QuestionText = "Question1 Test here for the user." };
            sr9.Survey = new Survey() { Id = 1, Title = "Survey Title", Description = "Survey Description" };
            sr9.Choice = new Choice() { Id = 48, ChoiceText = "Selected choice4" };
            srs.Add(sr9);

            SurveyResponse sr10 = new SurveyResponse();
            sr10.Question = new Question() { Id = 1, QuestionText = "Question1 Test here for the user." };
            sr10.Survey = new Survey() { Id = 1, Title = "Survey Title", Description = "Survey Description" };
            sr10.Choice = new Choice() { Id = 48, ChoiceText = "Selected choice4" };
            srs.Add(sr10);

            SurveyResponse sr2 = new SurveyResponse();
            sr2.Question = new Question() { Id = 1, QuestionText = "Question2 Test here for the user." };
            sr2.Survey = new Survey() { Id = 1, Title = "Survey Title", Description = "Survey Description" };
            sr2.Choice = new Choice() { Id = 45, ChoiceText = "Selected choice" };
            srs.Add(sr2);

            SurveyResponse sr3 = new SurveyResponse();
            sr3.Question = new Question() { Id = 1, QuestionText = "Question3 Test here for the user." };
            sr3.Survey = new Survey() { Id = 1, Title = "Survey Title", Description = "Survey Description" };
            sr3.Choice = new Choice() { Id = 45, ChoiceText = "Selected choice" };
            srs.Add(sr3);

            SurveyResponse sr4 = new SurveyResponse();
            sr4.Question = new Question() { Id = 1, QuestionText = "Question4 Test here for the user." };
            sr4.Survey = new Survey() { Id = 1, Title = "Survey Title", Description = "Survey Description" };
            sr4.Choice = new Choice() { Id = 45, ChoiceText = "Selected choice" };
            srs.Add(sr4);

            SurveyResponse sr5 = new SurveyResponse();
            sr5.Question = new Question() { Id = 1, QuestionText = "Question5 Test here for the user." };
            sr5.Survey = new Survey() { Id = 1, Title = "Survey Title", Description = "Survey Description" };
            sr5.Choice = new Choice() { Id = 45, ChoiceText = "Selected choice" };
            srs.Add(sr5);

            SurveyResponse sr6 = new SurveyResponse();
            sr6.Question = new Question() { Id = 1, QuestionText = "Question6 Test here for the user." };
            sr6.Survey = new Survey() { Id = 1, Title = "Survey Title", Description = "Survey Description" };
            sr6.Choice = new Choice() { Id = 45, ChoiceText = "Selected choice" };
            srs.Add(sr6);

            return srs;
        }
    }
}
