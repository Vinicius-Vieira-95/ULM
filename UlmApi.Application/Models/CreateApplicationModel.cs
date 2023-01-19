using System;

namespace UlmApi.Application.Models
{
    public class CreateApplicationModel
    {
        public string Name { get; set; }
        public DateTime CreationDate { get => DateTime.Now; }
    }
}
