using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTUforKids.Entities
{
    public class Question
    {
        public int QuestionId { get; set; }
        public string Difficulty { get; set; }
        public string QuestionText { get; set; }
        public string AnswerText { get; set; }
    }
}
