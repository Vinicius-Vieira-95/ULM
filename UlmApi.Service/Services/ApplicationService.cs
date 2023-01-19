using AutoMapper;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UlmApi.Domain.Dtos;
using UlmApi.Domain.Entities;
using UlmApi.Domain.Interfaces;
using UlmApi.Domain.Models.Queries;
using UlmApi.Infra.CrossCutting.Logger;

namespace UlmApi.Service.Services
{
    public class ApplicationService : BaseService<Application, int>, IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IMapper _mapper;

        public ApplicationService(IApplicationRepository applicationRepository, IMapper mapper) : base(applicationRepository, mapper)
        {
            _applicationRepository = applicationRepository;
            _mapper = mapper;
        }

        public async Task<ApplicationDto> Create<TInputModel, TValidator>(TInputModel inputModel)
            where TInputModel : class
            where TValidator : AbstractValidator<Application>
        {
            var application = _mapper.Map<Application>(inputModel);

            Validate(application, Activator.CreateInstance<TValidator>());

            await ValidateIfApplicationIsValid(application);

            await _applicationRepository.Insert(application);

            return new ApplicationDto(await _applicationRepository.Select(application.Id));
        }

        public async Task<List<ApplicationDto>> GetAllApplications()
        {
            return await _applicationRepository.GetApplications();
        }

        public async Task<ApplicationDto> GetApplicationById(int id)
        {
            var application = await _applicationRepository.Select(id);
            if (application == null) return null;

            return new ApplicationDto(application);
        }

        public async Task<ApplicationListDto> GetApplicationWithPagination(GetApplicationPaginationQuery queryParams)
        {
            return await _applicationRepository.GetApplicationWithPagination(queryParams);
        }

        private async Task ValidateIfApplicationIsValid(Application application)
        {
            var applicationName = await _applicationRepository.GetApplicationByName(application.Name);
            if (applicationName != null)
            {
                throw new InvalidOperationException("Application already exists to this name.");
            }
        }
    }
}
