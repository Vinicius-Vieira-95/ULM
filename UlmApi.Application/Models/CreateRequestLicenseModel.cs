using System;
using UlmApi.Domain.Entities.Enums;

namespace UlmApi.Application.Models
{
    public class CreateRequestLicenseModel
    {
        public int ProductId { get; set; } 
        public int SolutionId { get; set; } 
        public int ApplicationId { get; set; }
        public RequisitionReason Reason { get; set; }
        public DateTime RegistrationDate { get; set; }
        public RequestLicenseUsageTime UsageTime { get; set; }
        public int Quantity { get; set; }
        public string Justification { get; set; }
    }
}