using System;
using UlmApi.Domain.Entities;
using UlmApi.Domain.Entities.Enums;

namespace UlmApi.Domain.Dtos
{
    public class RequestLicenseDto
    {
        public int Id { get; set; }
        public string RequesterName { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string UsageTime { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
        public string ProductName { get; set; }
        public string ApplicationName { get; set; }
        public int SolutionId { get; set; }
        public string SolutionName { get; set; }
        public string Justification { get; set; }
        public string JustificationForDeny { get; set; }
        public string SuccessRate { get; set; }
        public string Recomendation { get; set; }
        
        public RequestLicenseDto(RequestLicense request)
        {
            Id = request.Id;
            RequesterName = request.RequesterName;
            RegistrationDate = request.RegistrationDate;
            UsageTime = Enum.GetName(typeof(RequestLicenseUsageTime), request.UsageTime);
            Quantity = request.Quantity;
            Status = Enum.GetName(typeof(RequestLicenseStatus), request.Status);
            ProductName = request.Product.Name;
            ApplicationName = request?.Application?.Name;
            SolutionId = request.Solution.Id;
            SolutionName = request.Solution.Name;
            Justification = request.Justification;
            JustificationForDeny = request.JustificationForDeny;
            Reason = Enum.GetName(typeof(RequisitionReason), request.Reason);
            SuccessRate = request?.Percentage == null? "Waiting for AI processing" : $"{request?.Percentage}% {request?.Prediction}";
            Recomendation = request.Message ?? "Waiting for AI processing";
        }
    }
}