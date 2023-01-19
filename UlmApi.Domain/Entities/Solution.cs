using System.Collections.Generic;

namespace UlmApi.Domain.Entities
{
    public class Solution : BaseEntity<int>
    {
        public string Name { get; set; }
        public string OwnerId { get; set; }
        public string OwnerName { get; set; }
        public int? ProductId { get; set; }
        public Product Product { get; set; }
        public List<RequestLicense> RequestLicenses { get; set; }
        public List<License> Licenses { get; set; }
    }
}