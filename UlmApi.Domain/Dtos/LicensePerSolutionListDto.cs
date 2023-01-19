using System.Collections.Generic;

namespace UlmApi.Domain.Dtos
{
    public class LicensePerSolutionListDto
    {
        public int TotalLicenses { get; set; }
        public List<LicensePerSolutionDto> Data { get; set; }
    }
}
