using System.Collections.Generic;
using UlmApi.Domain.Entities.Enums;

namespace UlmApi.Domain.Models.Queries
{
    public class GetLicensesBySolutionQuery
    {
        public List<LicenseStatus> Status { get; set; }
        public bool? IsExpired { get; set; }

        public GetLicensesBySolutionQuery()
        {
            Status = new List<LicenseStatus>();
        }
    }
}