using System.Collections.Generic;

namespace UlmApi.Domain.Dtos
{
    public abstract class GenericListDto<T>
    {
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public List<T> Data { get; set; }
    }
}
