using CTUforKids.Common;
using CTUforKids.Entities;
using CTUforKids.UI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CTUforKids.ViewModels
{
    class RegisterVM : ViewModel
    {

        #region properties
        private string name;

        public string Name
        {
            get { return name; }
            set {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string grade;

        public string Grade
        {
            get { return grade; }
            set {
                grade = value;
                OnPropertyChanged(nameof(Grade));
            }
        }

        private string email;

        public string Email
        {
            get { return email; }
            set {
                email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        #endregion

        public RegisterVM(INavigationService navigationService)
        {
            //Bind up navigation service and commands
            _navigationService = navigationService;
            this.Register = new Command(register, () => { return true; });
            this.CancelRegister = new Command(cancelReg, ()=> { return true; });
        }
        
        public async void register()
        {
            //Register the user and commit the object to the database


            //Create a temporary user to add to the Database, input all the fields supplied by the user.
            Student newStudent = new Student {
                Name = this.name,
                Grade = this.grade,
                Email = this.email,
                Password = this.password
            };

            //Serialize the new student
            var serializedData = JsonConvert.SerializeObject(newStudent);
            StringContent content =
                new StringContent(serializedData, Encoding.UTF8, "text/json");

            //Post the new student to the database
            var response =
                      await client.PostAsync("api/students", content);

            if (response.IsSuccessStatusCode)
            {

                Uri RecordUri = response.Headers.Location;
                var newRecord = await this.client.GetAsync(RecordUri);

                if (newRecord.IsSuccessStatusCode)
                {
                    //Redirect the user to the login page to login with his new details
                    _navigationService.Navigate(typeof(LoginPage));
                }
                else
                {
                    //Handle get failure
                }
            }
        }

        public void cancelReg()
        {
            _navigationService.Navigate(typeof(LoginPage));
        }


    }
}
