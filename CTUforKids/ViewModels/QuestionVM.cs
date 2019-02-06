
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTUforKids.Entities;
using System.Diagnostics;
using CTUforKids.Common;
using CTUforKids.UI;
using System.Net.Http;

namespace CTUforKids.ViewModels
{
    class QuestionVM : ViewModel
    {

        #region Properties
        private string questionNumber;
        public string QuestionNumber {
            get { return this.questionNumber;  }
            set {
                this.questionNumber = value;
                OnPropertyChanged(nameof(QuestionNumber));
                }
        }

        private string questionText { get; set; }
        public string QuestionText {
            get { return this.questionText; }
            set {
                this.questionText = value;
                OnPropertyChanged(nameof(QuestionText));
            }
        }

        private string answerInput { get; set; }
        public string AnswerInput {
            get { return this.answerInput; }
            set {
                this.answerInput = value;
                OnPropertyChanged(nameof(AnswerInput));
            }

        }

        #endregion

        public QuestionVM(INavigationService navigationservice)
        {
            //Bind up the navigation service and commands
            _navigationService = navigationservice;
            this.NextQuestionTest = new Command(nextQuestion, () => { return CurrentQuestion < CurrentQuestionList.Count()-1; });
            this.SubmitTest = new Command(SaveAsync, () => { return true; });

            //When the page loads, always increment the question number, and then show the user the question 
            //which will be in the CurrentQuestion object.
            this.QuestionNumber = (CurrentQuestion + 1).ToString();
            this.QuestionText = CurrentQuestionList[CurrentQuestion].QuestionText;
        }

        public void nextQuestion()
        {
            //Decide if the user got the question right, if so, increment his Score.
            if (answerInput == CurrentQuestionList[CurrentQuestion].AnswerText)
            {
                CurrentTest.Score++;
            }
            //Increment the CurrentQuestion object to load the next question when the page loads.
            CurrentQuestion++;
            //Relaod the page
            _navigationService.Navigate(typeof(QuestionPage));
        }
        
        private async void SaveAsync()
        {
            //Save the Test data to the database.

            //This method is called when the submit button is clicked on the last question, 
            //meaning you will still need to handle the last questions answer:
            if (answerInput == CurrentQuestionList[CurrentQuestion].AnswerText)
            {
                CurrentTest.Score++;
            }

            //Serialize the Test in the CurrentTest placeholder

            var serializedData = JsonConvert.SerializeObject(CurrentTest);
            StringContent content =
                new StringContent(serializedData, Encoding.UTF8, "text/json");

            var response =
                      await client.PostAsync("api/tests", content);
            
            if (response.IsSuccessStatusCode)
            {
                //Check the data was committed properly
                Uri RecordUri = response.Headers.Location;
                var newRecord = await this.client.GetAsync(RecordUri);

                if (newRecord.IsSuccessStatusCode)
                {
                    //If the user completed the test and the test was committed to the Database, 
                    //reset all the placeholder feilds and send the user back to the dashboard to view his score
                    CurrentTest = null;
                    CurrentQuestion = 0;
                    CurrentQuestionList = new List<Question>();

                    _navigationService.Navigate(typeof(DashBoard));
                }
                else
                {
                    //Handle get failure
                }
                
            }
        }

    }
}
