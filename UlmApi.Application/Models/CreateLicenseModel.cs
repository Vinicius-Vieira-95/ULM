using System;

namespace UlmApi.Application.Models
{
    public class CreateLicenseModel
    {
        public string Label { get; set; }
        public int? ApplicationId { get; set; }
        public int SolutionId { get; set; }
        public int Quantity { get; set; }
        public double? Price { get; set; }
        public DateTime AquisitionDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Key { get; set; }
        public string Justification { get; set; }
    }
}
