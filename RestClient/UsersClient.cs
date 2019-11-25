using RestSharp;
using System;

namespace UserConnector
{
    public class UsersClient
    {
        public string GetFirstClient(string url)
        {
            RestClient restClient = new RestClient();
            RestRequest restRequest = new RestRequest("https://reqres.in/api/users", Method.GET);
            IRestResponse <UserDetails> response = restClient.Execute<UserDetails>(restRequest);

            UserDetails userDetails = response.Data;
            string retval = "No data found";
            if (userDetails.Data.Count > 1)
            {
                retval = userDetails.Data[0].Email;
            }

            return retval;
        }
    }
}
