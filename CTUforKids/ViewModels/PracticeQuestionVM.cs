using CTUforKids.Common;
using CTUforKids.Entities;
using CTUforKids.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTUforKids.ViewModels
{
    class PracticeQuestionVM : ViewModel
    {
        #region Props
        private string questionNumber;
        public string QuestionNumber
        {
            get { return this.questionNumber; }
            set
            {
                this.questionNumber = value;
                OnPropertyChanged(nameof(QuestionNumber));
            }
        }

        private string questionText { get; set; }
        public string QuestionText
        {
            get { return this.questionText; }
            set
            {
                this.questionText = value;
                OnPropertyChanged(nameof(QuestionText));
            }
        }

        private string answerInput { get; set; }
        public string AnswerInput
        {
            get { return this.answerInput; }
            set
            {
                this.answerInput = value;
                OnPropertyChanged(nameof(AnswerInput));
            }

        }

        private string feedBack;
        public string FeedBack
        {
            get { return this.feedBack; }
            set {
                this.feedBack = value;
                this.OnPropertyChanged(nameof(FeedBack));
            }
        }
        #endregion

        public PracticeQuestionVM(INavigationService navigationservice)
        {
            //When the page is created blank out the feedBack section
            this.feedBack = "";

            //Bind up the nagaivationservice and commands for the buttons
            _navigationService = navigationservice;
            this.EndPractice = new Command(endPractice, () => { return CurrentQuestion < CurrentQuestionList.Count() - 1; });
            this.NextQuestionPractice = new Command(nextQuestion, () => { return CurrentQuestion < CurrentQuestionList.Count() - 1; });
            this.ShowAnswer = new Command(showAnswer, () => { return true; });

            //When the page loads, always increment the question number, and then show the user the question 
            //which will be in the CurrentQuestion object.
            this.QuestionNumber = (CurrentQuestion + 1).ToString();
            this.QuestionText = CurrentQuestionList[CurrentQuestion].QuestionText;
        }

        public void showAnswer()
        {
            //Display the answer if the user requests it
            var answer = CurrentQuestionList[CurrentQuestion].AnswerText;
            //Decide whether the user got it correct
            if (answerInput == answer)
            {
                this.FeedBack = "Correct!";
            }
            else
            {
                this.FeedBack = "Incorrect!\nAnswer="+answer;

            }

        }

        public void nextQuestion()
        {
            //Increment the CurrentQuestion object which will cause the next question to be loaded into the UI
            CurrentQuestion++;
            //Reload the page with the new question
            _navigationService.Navigate(typeof(PracticeQuestionPage));
        }


        public void endPractice()
        {
            //When submit is clicked reset all the fields that were used, 
            //don't save the test and just return the user to the home/dashboard page.
            CurrentTest = null;
            CurrentQuestion = 0;
            CurrentQuestionList = new List<Question>();

            _navigationService.Navigate(typeof(DashBoard));
        }


    }
}
