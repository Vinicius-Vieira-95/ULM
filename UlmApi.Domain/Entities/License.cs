using System;
using UlmApi.Domain.Entities.Enums;

namespace UlmApi.Domain.Entities
{
    public class License : BaseEntity<int>
    {
        public string Label { get; set; }
        public string Key { get; set; }
        public int Quantity { get; set; }
        public double? Price { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime AquisitionDate { get; set; }
        public string Justification { get; set; }
        public LicenseStatus Status { get; set; }
        public int SolutionId { get; set; }
        public Solution Solution { get; set; }
        public int? ApplicationId { get; set; }
        public Application Application { get; set; }
        public RequestLicense Request { get; set; }
        public bool Archived { get; set; }
    }
}
