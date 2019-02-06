using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTUforKids.Entities;
using Newtonsoft.Json;
using System.Diagnostics;

namespace CTUforKids.ViewModels
{
    class TestVM : ViewModel
    {
        public List<Test> Tests { get; private set; }

        public async Task GetDataAsync()
        {
            //Get test data from the database
            try
            {
                var response = await this.client.GetAsync("api/tests");
                if (response.IsSuccessStatusCode)
                {
                    var testData =
                        await response.Content.ReadAsStringAsync();
                    this.Tests =
                        JsonConvert.DeserializeObject<List<Test>>(testData);
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

        private void Add()
        {
            //This method serves to add a new test object to the list of tests, 
            //used to update the dashboard page with a users new tests
            Test newTest = new Test { TestId = 0 };
            this.Tests.Add(newTest);
        }
    }
}
