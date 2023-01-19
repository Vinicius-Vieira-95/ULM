using System.Collections.Generic;

namespace UlmApi.Domain.Entities
{
    public class Product : BaseEntity<int>
    {
        public string Name { get; set; }
        public List<Solution> Solutions { get; set; }
        public List<RequestLicense> RequestLicenses { get; set; }
    }
}