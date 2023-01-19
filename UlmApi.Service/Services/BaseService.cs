using AutoMapper;
using FluentValidation;
using UlmApi.Domain.Entities;
using UlmApi.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UlmApi.Service.Services
{
    public class BaseService<TEntity, TType> : IBaseService<TEntity, TType> where TEntity : class
    {
        private readonly IBaseRepository<TEntity, TType> _baseRepository;
        private readonly IMapper _mapper;

        public BaseService(IBaseRepository<TEntity, TType> baseRepository, IMapper mapper)
        {
            _baseRepository = baseRepository;
            _mapper = mapper;
        }

        public TOutputModel Add<TInputModel, TOutputModel, TValidator>(TInputModel inputModel)
            where TValidator : AbstractValidator<TEntity>
            where TInputModel : class
            where TOutputModel : class
        {
            TEntity entity = _mapper.Map<TEntity>(inputModel);

            Validate(entity, Activator.CreateInstance<TValidator>());
            _baseRepository.Insert(entity);

            TOutputModel outputModel = _mapper.Map<TOutputModel>(entity);

            return outputModel;
        }

        public void Delete(TType id) => _baseRepository.Delete(id);

        public async Task<IEnumerable<TOutputModel>> Get<TOutputModel>() where TOutputModel : class
        {
            var entities = await _baseRepository.Select();

            var outputModels = entities.Select(s => _mapper.Map<TOutputModel>(s));

            return outputModels;
        }

        public async Task<TOutputModel> GetById<TOutputModel>(TType id) where TOutputModel : class
        {
            var entity = await _baseRepository.Select(id);

            var outputModel = _mapper.Map<TOutputModel>(entity);

            return outputModel;
        }

        public TOutputModel Update<TInputModel, TOutputModel, TValidator>(TInputModel inputModel)
            where TValidator : AbstractValidator<TEntity>
            where TInputModel : class
            where TOutputModel : class
        {
            TEntity entity = _mapper.Map<TEntity>(inputModel);

            Validate(entity, Activator.CreateInstance<TValidator>());
            _baseRepository.Update(entity);

            TOutputModel outputModel = _mapper.Map<TOutputModel>(entity);

            return outputModel;
        }

        public async Task Save()
        {
            await _baseRepository.Save();
        }

        protected void Validate(TEntity obj, AbstractValidator<TEntity> validator)
        {
            if (obj == null)
                throw new Exception("Registros não detectados!");

            validator.ValidateAndThrow(obj);
        }
    }
}
