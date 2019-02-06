using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTUforKids.Entities;
using Newtonsoft.Json;
using CTUforKids.UI;
using CTUforKids.Common;
using System.Diagnostics;

namespace CTUforKids.ViewModels
{
    class DashboardVM : ViewModel
    {
        #region Properties


        private List<Test> studentTests;
        public List<Test> StudentTests {
            get { return this.studentTests; }
            set
            {
                this.studentTests = value;
                this.OnPropertyChanged(nameof(StudentTests));
            }
        }

        private List<Test> tests;
        public List<Test> Tests {
            get { return this.tests; }
            set {
                this.tests = value;
                this.StudentTests = value.Where(x => x.StudentId == CurrentUser.StudentId).ToList();
                OnPropertyChanged(nameof(Tests));
            }
        }

        public List<Question> Questions { get; set; }


        private int chosenQuestionAmount;
        public int ChosenQuestionAmount {
            get { return this.chosenQuestionAmount; }
            set {
                this.chosenQuestionAmount = value;
                this.OnPropertyChanged(nameof(ChosenQuestionAmount));
            }
        }

        private string chosenDifficulty;
        public string ChosenDifficulty
        {
            get { return this.chosenDifficulty; }
            set
            {
                this.chosenDifficulty = value;
                this.OnPropertyChanged(nameof(ChosenDifficulty));
            }
        }


        #endregion

        public DashboardVM(INavigationService navigationservice) 
        {
            ///import pages navigation_service to allow page navigation from VM
            _navigationService = navigationservice;
            //Bind commands
            this.StartTest = new Command(this.CreateTest, ()=> { return true; });
            this.StartPractice = new Command(this.CreatePractice, () => { return true; });
        }

        public async Task GetDataAsync()
        {
            //Asyncronously pull the test and questions data from the SQL db using httpclient
            //Test data is used in a table display to the user so he can see past tests, question
            //data is loaded to be added to the first question when the user starts a test.
            try
            {
                var TestResponse     = await this.client.GetAsync("api/tests");
                var QuestionResponse = await this.client.GetAsync("api/questions");

                if (TestResponse.IsSuccessStatusCode && QuestionResponse.IsSuccessStatusCode)
                {
                    var testData =
                        await TestResponse.Content.ReadAsStringAsync();

                    this.Tests = JsonConvert.DeserializeObject<List<Test>>(testData);

                    //Reverse to display the most recent test at the top of the list in UI.
                    this.Tests.Reverse();

                    var questionData =
                        await QuestionResponse.Content.ReadAsStringAsync();
                    this.Questions =
                        JsonConvert.DeserializeObject<List<Question>>(questionData);
                    
                }
                else
                {
                    // TODO: Handle GET failure
                }
            }
            catch (Exception e)
            {
                // TODO: Handle exceptions
                Debug.WriteLine("///////////Error/////////");
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.InnerException);
                Debug.WriteLine(e.StackTrace);
                Debug.WriteLine("///////////Error/////////");
            }
        }

        public void CreatePractice()
        {
            //Create a new list of all the questions in the database that the user can use to practice

            CurrentQuestionList = this.Questions;

            _navigationService.Navigate(typeof(PracticeQuestionPage));

        }

        public void CreateTest()
        {

            Random rnd = new Random();
            List<Question> tempQuestionList = new List<Question>();


            //Create a temporary test object, with the users specifications
            //that can be used as a placeholder, 
            //and later updated with the users results.

            Test tempTest = new Test
            {
                Difficulty = this.chosenDifficulty,
                QuestionAmount = this.chosenQuestionAmount,
                StudentId = CurrentUser.StudentId,
                Score = 0
            };

            //Filter through all the questions and prepare a list of 
            //questions that match the users difficulty specification.

            List<Question> difficultyFiltered = this.Questions.Where(x => x.Difficulty == tempTest.Difficulty).ToList();
            int questionsCount = difficultyFiltered.Count();

            for (int i = 0; i < tempTest.QuestionAmount; i++)
            {
                tempQuestionList.Add(difficultyFiltered[rnd.Next(0, questionsCount-1)]);
            }

            //Swap the current test object to hold the placeholder, changes will now be made to the CurrentTest Object
            CurrentTest = tempTest;
            //Add the prepared list of questions to the list which will be shown to the user
            CurrentQuestionList = tempQuestionList;

            _navigationService.Navigate(typeof(QuestionPage));

        }


    }
}
