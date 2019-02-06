using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using CTUforKids.Entities;
using System.ComponentModel;
using CTUforKids.Common;

namespace CTUforKids
{
    //The command calls the Add() method which passes the newStudent object to the save changes method.

    class ViewModel : INotifyPropertyChanged
    {
        //Need a reference to a pages navigation service to be able to navigate from the ViewModel
        protected INavigationService   _navigationService;

        //Set a Url string and a HttpClient that will use that string
        private const string         ServerUrl = "http://LocalHost:50000/";
        protected HttpClient         client = null;
        
        //All placeholder properties
        public static Student        CurrentUser         { get; protected set; }
        public static Test           CurrentTest         { get; set; }
        public static List<Question> CurrentQuestionList { get; set; }
        public static int            CurrentQuestion     { get; set; }

        //All commands that will be requied for button bindings
        public Command addStudent           { get; protected set; }
        public Command addTest              { get; protected set; }
        public Command Login                { get; protected set; }
        public Command StartTest            { get; protected set; }
        public Command StartPractice        { get; protected set; }
        public Command NextQuestionTest     { get; protected set; }
        public Command NextQuestionPractice { get; protected set; }
        public Command SubmitTest           { get; protected set; }
        public Command EndPractice          { get; protected set; }
        public Command ShowAnswer           { get; protected set; }
        public Command SignUp               { get; protected set; }
        public Command Register             { get; protected set; }
        public Command CancelRegister       { get; protected set; }


        public ViewModel()
        {
            //Initialise the httpClient
            this.client = new HttpClient();
            this.client.BaseAddress = new Uri(ServerUrl);
            this.client.DefaultRequestHeaders.Accept.
                Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        //Property Changed event handler to update two-way bindings in the UI.

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
