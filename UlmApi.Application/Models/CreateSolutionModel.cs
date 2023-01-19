using System;

namespace UlmApi.Application.Models
{
    public class CreateSolutionModel
    {
        public string Name { get; set; }
        public string OwnerId { get; set; }
        public int ProductId { get; set; }
    }
}
