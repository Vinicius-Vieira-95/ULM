namespace UlmApi.Domain.Dtos
{
    public class LicensePerSolutionDto
    {
        public string Solution { get; set; }
        public int InUse { get; set; }
        public int StandBy { get; set; }
        public int Total => InUse + StandBy;
    }
}