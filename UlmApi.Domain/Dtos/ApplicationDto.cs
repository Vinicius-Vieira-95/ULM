using System;
using UlmApi.Domain.Entities;

namespace UlmApi.Domain.Dtos
{
    public class ApplicationDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }

        public ApplicationDto(Application application)
        {
            Id = application.Id;
            Name = application.Name;
            CreationDate = application.CreationDate;
        }
    }
}
