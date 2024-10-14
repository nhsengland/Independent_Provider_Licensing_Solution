using AngleSharp.Dom;
using Okta.Sdk.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licensing.Automation.Functions
{
    public class OktaGroup
    {       
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Modified {  get; set; }
        public DateTimeOffset MembershipUpdated {  get; set; }

        
        public OktaGroup(Group group)
        {
            Id = group.Id;
            Name = group.Profile.Name;
            Description = group.Profile.Description;
            Created = group.Created;
            Modified = group.LastUpdated;
            MembershipUpdated = group.LastMembershipUpdated;
        }
        
    }
}
