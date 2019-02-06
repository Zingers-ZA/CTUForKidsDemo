using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CTUforKids.Entities
{
    public class Test
    {
        public int TestId { get; set; }
        public Nullable<int> StudentId { get; set; }
        public string Difficulty { get; set; }
        public Nullable<int> Score { get; set; }
        public Nullable<int> QuestionAmount
        {
            get { return this.questionAmount; }
            set
            {
                this.questionAmount = value;
                this.ScoreToString = Score.ToString() + "/" + value.ToString();
            }

        }

        [JsonIgnore]
        public string ScoreToString { get; set; }
        [JsonIgnore]
        private Nullable<int> questionAmount;

    }
}
