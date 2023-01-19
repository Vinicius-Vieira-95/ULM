using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using UlmApi.Domain.Dtos;
using UlmApi.Domain.Entities;
using UlmApi.Domain.Entities.Enums;
using UlmApi.Domain.Interfaces;
using UlmApi.Domain.Models.Queries;
using UlmApi.Infra.CrossCutting.Logger;

namespace UlmApi.Service.Services
{
    public class LicenseService : BaseService<License, int>, ILicenseService
    {
        private readonly ILicenseRepository _licenseRepository;
        private readonly IMapper _mapper;
        private readonly AuthenticatedUserService _authenticatedUser;
        private readonly ISolutionRepository _solutionRepository;
        private readonly IBaseRepository<Domain.Entities.Application, int> _baseApplicationRepository;
        private readonly IBaseRepository<Solution, int> _baseSolutionRepository;

        public LicenseService(ILicenseRepository licenseRepository,
                IMapper mapper,
                IBaseRepository<Domain.Entities.Application, int> baseApplicationRepository,
                AuthenticatedUserService authenticatedUser,
                ISolutionRepository solutionRepository,
                IBaseRepository<Solution, int> baseSolutionRepository) : base(licenseRepository, mapper)
        {
            _licenseRepository = licenseRepository;
            _mapper = mapper;
            _baseApplicationRepository = baseApplicationRepository;
            _baseSolutionRepository = baseSolutionRepository;
            _authenticatedUser = authenticatedUser;
            _solutionRepository = solutionRepository;

        }

        public async Task<LicenseDto> Create<TInputModel, TValidator>(TInputModel inputModel)
            where TInputModel : class
            where TValidator : AbstractValidator<License>
        {
            var license = _mapper.Map<License>(inputModel);

            Validate(license, Activator.CreateInstance<TValidator>());
            await ValidateIfRelationshipsExist(license);

            license.Status = license.ApplicationId == null ? LicenseStatus.STAND_BY : LicenseStatus.IN_USE;
            await _licenseRepository.Insert(license);

            return new LicenseDto(await _licenseRepository.Select(license.Id));
        }

        public async Task<LicensesListDto> GetLicensesWithPagination(GetLicensesPaginationQuery queryParams)
        {   
            LicensesListDto licenses;
            switch (_authenticatedUser.Role)
            {
                case "REQUESTER":
                    licenses = await _licenseRepository.GetLicensesWithPagination(queryParams, _authenticatedUser.Id);
                    break;

                case "OWNER":
                    var solution = await _solutionRepository.GetSolutionByOwnerId(_authenticatedUser.Id);
                    licenses = await _licenseRepository.GetLicensesWithPagination(queryParams, null, solution?.Id ?? 0);
                    break;

                case "ADMIN":
                    licenses = await _licenseRepository.GetLicensesWithPagination(queryParams);
                    break;

                default:
                    licenses = new LicensesListDto();
                    break;
            }

            return licenses;
        }

        public async Task<LicenseDto> GetById(int id)
        {
            var license = await _licenseRepository.Select(id);
            if (license == null) return null;

            return new LicenseDto(license);
        }

        public async Task<License> Archived(License license)
        {
            license.Archived = true;
            await _licenseRepository.Save();
            return license;
        }

        public async Task<License> Unarchived(License license)
        {
            license.Archived = false;
            await _licenseRepository.Save();
            return license;
        }

        public async Task<License> DisassociateApplication(License license)
        {
            license.Application = null;
            license.Status = LicenseStatus.STAND_BY;
            await _licenseRepository.Save();
            return license;
        }

        private async Task ValidateIfRelationshipsExist(License license)
        {
            var solution = await _baseSolutionRepository.Select(license.SolutionId);
            if (solution == null)
            {
                throw new InvalidOperationException("Solution not found.");
            }

            if (license.ApplicationId != null)
            {
                var application = await _baseApplicationRepository.Select((int) license.ApplicationId);
                if (application == null)
                {
                    throw new InvalidOperationException("Application not found.");
                }
            }
        }

        public async Task<List<LicenseDto>> GetLicensesBySolutionId(int solutionId, GetLicensesBySolutionQuery queryParams)
        {
            var licenses = await _licenseRepository.GetLicensesBySolutionId(solutionId, queryParams);
            if (licenses.Any())
                return licenses.Select(l => new LicenseDto(l)).ToList();

            return new List<LicenseDto>();
        }

        public async Task<License> PlaceInUse(License license)
        {
            license.Status = LicenseStatus.IN_USE;
            await _licenseRepository.Save();
            return license;
        }
    }
}
