using System;
using System.Collections.Generic;
using UlmApi.Domain.Entities.Enums;

namespace UlmApi.Domain.Models.Queries
{
    public class GetRequestLicensesPaginationQuery : GenericQuery
    {
        public List<RequestLicenseStatus> Status { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime FinalDate { get; set; }

        public GetRequestLicensesPaginationQuery()
        {   
            InitialDate = DateTime.MinValue;
            FinalDate = DateTime.MaxValue;
            Limit = 10;
            Page = 0;
        }
    }
}