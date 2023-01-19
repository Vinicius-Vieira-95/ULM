using System;
using UlmApi.Domain.Entities;
using UlmApi.Domain.Entities.Enums;

namespace UlmApi.Domain.Dtos
{
    public class LicenseDto
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public string Key { get; set; }
        public string ExpirationDate { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public string OwnerName { get; set; }
        public string AquisitionDate { get; set; }
        public string Justification { get; set; }
        public string ApplicationName { get; set; }
        public string Solution { get; set; }
        public bool Archived { get; set; }
        public bool IsExpired { get; set; }
        public double? Price { get; set; }

        public LicenseDto(License license)
        {
            Id = license.Id;
            Label = license.Label;
            Key = license.Key;
            ExpirationDate = license.ExpirationDate.ToString("yyyy-MM-dd HH':'mm':'ss");
            Status = Enum.GetName(typeof(LicenseStatus), license.Status);
            Solution = license.Solution.Name;
            AquisitionDate = license.AquisitionDate.ToString("yyyy-MM-dd HH':'mm':'ss");
            Justification = license.Justification;
            Quantity = license.Quantity;
            Price = license?.Price;
            OwnerName = license.Solution?.OwnerName;
            ApplicationName = license?.Application?.Name;
            IsExpired = DateTime.Now > license.ExpirationDate;
            Archived = license.Archived;
        }
    }
}
