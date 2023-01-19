using System;
using System.Collections.Generic;
using UlmApi.Domain.Entities.Enums;

namespace UlmApi.Domain.Entities
{
    public class RequestLicense : BaseEntity<int>
    {
        public DateTime RegistrationDate { get; set; }
        public RequestLicenseUsageTime UsageTime { get; set; }
        public int Quantity { get; set; }
        public RequestLicenseStatus Status { get; set; }
        public RequisitionReason Reason { get; set; }
        public string Justification { get; set; }
        public string JustificationForDeny { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string RequesterId { get; set; }
        public string RequesterName { get; set; }
        public int ApplicationId { get; set; }
        public Application Application { get; set; }
        public int SolutionId { get; set; }
        public Solution Solution { get; set; }
        public int? LicenseId { get; set; }
        public License License { get; set; }
        public int? Percentage { get; set; }
        public string Prediction { get; set; }
        public string Message { get; set; }
    }
}
