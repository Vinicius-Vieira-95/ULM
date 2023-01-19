using System;
using System.Collections.Generic;
using UlmApi.Domain.Entities.Enums;

namespace UlmApi.Domain.Models.Queries
{
    public class GetLicensesPaginationQuery : GenericQuery
    {
        public List<LicenseStatus> Status { get; set; }
        public bool Archived { get; set; }
        public bool? IsExpired { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime FinalDate { get; set; }

        public GetLicensesPaginationQuery()
        {
            InitialDate = DateTime.MinValue;
            FinalDate = DateTime.MaxValue;
            Limit = 10;
            Page = 0;
        }
    }
}