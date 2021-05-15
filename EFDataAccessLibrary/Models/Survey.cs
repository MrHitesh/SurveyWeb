using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccessLibrary.Models
{
    public class Survey
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Question> Questions { get; set; } = new List<Question>();
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        [Required]
        public DateTime ExpiresOn { get; set; }
    }
    public class Question
    {
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public List<Choice> Choices { get; set; } = new List<Choice>();
    }

    public class Choice
    {
        public int Id { get; set; }
        public string ChoiceText { get; set; }

        public override bool Equals(object obj)
        {
            var choice = (Choice)obj;
            //Check for null and compare run-time types.
            if ((obj == null) || (this.Id != choice.Id))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public override int GetHashCode()
        {
            return (this.Id.GetHashCode() << 2) ^ this.Id.GetHashCode();
        }
    }

    public class SurveyResponse
    {
        public int Id { get; set; }
        [Required]
        public Survey Survey { get; set; }
        [Required]
        public Question Question { get; set; }
        [Required]
        public Choice Choice { get; set; }
        [Required]
        [MaxLength(10)]
        [Column(TypeName="varchar(10)")]
        public string EmployeeId { get; set; }
        [Required]
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        
    }
}
