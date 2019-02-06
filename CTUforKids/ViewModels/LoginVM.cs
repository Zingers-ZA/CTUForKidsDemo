using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTUforKids.Entities;
using Newtonsoft.Json;
using CTUforKids.Common;
using System.Diagnostics;
using CTUforKids.UI;

namespace CTUforKids.ViewModels
{
    class LoginVM : ViewModel
    {
        
        #region Properties
        private List<Student> students { get; set; }
        public List<Student> Students {
            get { return this.students;  }
            set {
                this.students = value;
                this.OnPropertyChanged(nameof(Students));
            }
        }
        private string emailinput;
        public string EmailInput
        {
            get { return this.emailinput; }
            set
            {
                this.emailinput = value;
                this.OnPropertyChanged(nameof(EmailInput));
            }
        }

        private string passwordinput;
        public string PasswordInput
        {
            get { return this.passwordinput; }
            set
            {
                this.passwordinput = value;
                this.OnPropertyChanged(nameof(PasswordInput));
            }
        }
        #endregion
        
        public LoginVM(INavigationService navigationService)
        {
            _navigationService = navigationService;
            this.Login = new Command(LogUserIn, () => { return true; });
            this.SignUp = new Command(() => { _navigationService.Navigate(typeof(RegisterPage)); }, () => { return true; });
        }

        public async void LogUserIn()
        {
            //Wait for all data to be pulled from Database
            await GetDataAsync();


            //Check if the users credentials are correct
            foreach (var student in Students)
            {
                //Important! - Windows has a service which will handle login for you 
                //which is much more secure than storing passwords as text. Don't do this in next project. 
                if (student.Email == emailinput && student.Password == passwordinput)
                {
                    //Break if you find a match
                    CurrentUser = student;
                    _navigationService.Navigate(typeof(DashBoard));
                    break;
                }
            }

            //If it didnt find a match, blank out the UI fields.
            if (CurrentUser == null)
            {
                EmailInput = "";
                PasswordInput = "";
            }
        }

        public async Task GetDataAsync()
        {
            //Pull all student data from the database via the HttpClient
            try
            {
                var response = await this.client.GetAsync("api/students");
                if (response.IsSuccessStatusCode)
                {
                    var studentData =
                        await response.Content.ReadAsStringAsync();
                    this.Students =
                        JsonConvert.DeserializeObject<List<Student>>(studentData);
                }
                else
                {
                    // TODO: Handle GET failure
                }
            }
            catch (Exception e)
            {
                // TODO: Handle exceptions
            }
        }

        
    }
}
