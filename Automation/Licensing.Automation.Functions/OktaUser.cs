using AngleSharp.Dom;
using Okta.Sdk.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licensing.Automation.Functions
{
    public class OktaUser
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public string FirstName {  get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Organization { get; set; }
        public string PrimaryPhone {  get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset LastUpdated { get; set; }

        public OktaUser(User user)
        {
            Id = user.Id;
            Status = user.Status;
            Email = user.Profile.Email;
            Login = user.Profile.Login;
            FirstName = user.Profile.FirstName;
            LastName = user.Profile.LastName;
            Organization = user.Profile.Organization;
            PrimaryPhone = user.Profile.PrimaryPhone;
            Created = user.Created;
            LastUpdated = user.LastUpdated;
        }
        
    }
}
