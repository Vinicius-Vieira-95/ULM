using System;
using System.Collections.Generic;

namespace UlmApi.Domain.Entities
{
    public class Application : BaseEntity<int>
    {
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public List<RequestLicense> RequestLicenses { get; set; }
        public List<License> Licenses { get; set; }
    }
}
