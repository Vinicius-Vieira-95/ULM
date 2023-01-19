using FluentValidation;
using UlmApi.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UlmApi.Domain.Interfaces
{
    public interface IBaseService<TEntity, TType> where TEntity : class
    {
        TOutputModel Add<TInputModel, TOutputModel, TValidator>(TInputModel inputModel)
            where TValidator : AbstractValidator<TEntity>
            where TInputModel : class
            where TOutputModel : class;

        void Delete(TType id);

        Task<IEnumerable<TOutputModel>> Get<TOutputModel>() where TOutputModel : class;

        Task<TOutputModel> GetById<TOutputModel>(TType id) where TOutputModel : class;

        TOutputModel Update<TInputModel, TOutputModel, TValidator>(TInputModel inputModel)
            where TValidator : AbstractValidator<TEntity>
            where TInputModel : class
            where TOutputModel : class;
            
        Task Save();
    }
}
