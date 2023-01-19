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
using UlmApi.Domain.Models;
using UlmApi.Domain.Models.Queries;
using UlmApi.Infra.CrossCutting.Logger;

namespace UlmApi.Service.Services
{
    public class RequestLicenseService : BaseService<RequestLicense, int>, IRequestLicenseService
    {
        private readonly IRequestLicenseRepository _requestLicenseRepository;
        private readonly ILicenseRepository _licenseRepository;
        private readonly IMapper _mapper;
        private readonly IBaseRepository<Product, int> _baseProductRepository;
        private readonly IBaseRepository<Application, int> _baseApplicationRepository;
        private readonly ISolutionRepository _solutionRepository;
        private readonly AuthenticatedUserService _authenticatedUser;
        private readonly IUserService _userService; 

        public RequestLicenseService(IRequestLicenseRepository requestLicenseRepository, ILicenseRepository licenseRepository, IMapper mapper,
                IBaseRepository<Product, int> baseProductRepository,
                IBaseRepository<Application, int> baseApplicationRepository,
                AuthenticatedUserService authenticatedUser,
                IUserService userService,
                ISolutionRepository solutionRepository) : base(requestLicenseRepository, mapper)
        {
            _requestLicenseRepository = requestLicenseRepository;
            _licenseRepository = licenseRepository;
            _mapper = mapper;
            _baseProductRepository = baseProductRepository;
            _baseApplicationRepository = baseApplicationRepository;
            _solutionRepository = solutionRepository;
            _authenticatedUser = authenticatedUser;
            _userService = userService;
        }

        public async Task<RequestLicenseDto> ChangeStatusRequest(RequestLicense requestLicense, License license, RequestLicenseStatus status)
        {
            requestLicense.Status = status;
            requestLicense.LicenseId = license.Id;
            license.Status = LicenseStatus.IN_USE;
            license.ApplicationId = requestLicense.ApplicationId;

            await _requestLicenseRepository.Save();
            await _licenseRepository.Save();

            return new RequestLicenseDto(requestLicense);
        }

        public async Task<RequestLicenseDto> ChangeStatusRequest(RequestLicense requestLicense, RequestLicenseStatus status, string justificationForDeny)
        {
            requestLicense.Status = status;
            requestLicense.JustificationForDeny = justificationForDeny;
            await _requestLicenseRepository.Save();

            return new RequestLicenseDto(requestLicense);
        }

        public async Task<RequestLicenseDto> ChangeStatusRequest(RequestLicense requestLicense, RequestLicenseStatus status)
        {
            requestLicense.Status = status;
            await _requestLicenseRepository.Save();

            return new RequestLicenseDto(requestLicense);
        }

        public async Task<RequestLicensesListDto> GetRequestLicensesWithPagination(GetRequestLicensesPaginationQuery queryParams)
        {
            RequestLicensesListDto requests;
            switch (_authenticatedUser.Role)
            {
                case "REQUESTER":
                    requests = await _requestLicenseRepository.GetRequestLicensesByRequesterId(queryParams, _authenticatedUser.Id);
                    break;

                case "OWNER":
                    var solution = await _solutionRepository.GetSolutionByOwnerId(_authenticatedUser.Id);
                    requests = await _requestLicenseRepository.GetRequestLicensesBySolutionId(queryParams, solution?.Id ?? 0);
                    break;

                case "ADMIN":
                    requests = await _requestLicenseRepository.GetAllRequests(queryParams);
                    break;

                default:
                    requests = new RequestLicensesListDto();
                    break;
            }

            return requests;
        }

        public async Task<RequestLicenseDto> Create<TInputModel, TValidator>(TInputModel inputModel)
            where TInputModel : class
            where TValidator : AbstractValidator<RequestLicense>
        {
            var requestLicense = _mapper.Map<RequestLicense>(inputModel);
            requestLicense.RequesterId = _authenticatedUser.Id;
            requestLicense.RequesterName = _authenticatedUser.FullName;

            Validate(requestLicense, Activator.CreateInstance<TValidator>());
            await ValidateIfRelationshipsExist(requestLicense);

            await _requestLicenseRepository.Insert(requestLicense);

            return new RequestLicenseDto(await _requestLicenseRepository.Select(requestLicense.Id));
        }
        
        private async Task ValidateIfRelationshipsExist(RequestLicense requestLicense)
        {
            var product = await _baseProductRepository.Select(requestLicense.ProductId);
            if (product == null)
            {
                throw new InvalidOperationException("Product not found.");
            }

            var solution = await _solutionRepository.Select(requestLicense.SolutionId);
            if (solution == null)
            {   
                throw new InvalidOperationException("Solution not found.");
            }

            var application = await _baseApplicationRepository.Select(requestLicense.ApplicationId);
            if (application == null)
            {
                throw new InvalidOperationException("Application not found.");
            }
                
            var requester = await _userService.GetUserById(requestLicense.RequesterId);
            if (requester == null)
            {
                throw new InvalidOperationException("Requester not found.");
            }

        }

        public async Task<RequestLicenseDto> GetById(int id)
        {
            var request = await _requestLicenseRepository.Select(id);
            if (request == null) return null;

            return new RequestLicenseDto(request);
        }

        public List<GenericEnumModel> GetUsageTimeEnum()
        {
            var usageTimes = new List<GenericEnumModel>();
            var usageTimesValues = Enum.GetValues(typeof(RequestLicenseUsageTime)).OfType<RequestLicenseUsageTime>().ToList();
            int i = 0;

            foreach (var item in usageTimesValues)
            {
                usageTimes.Add(new GenericEnumModel
                {
                    Value = i,
                    ValueString = item.ToString()
                });
                i++;
            }

            return usageTimes;
        }

        public List<GenericEnumModel> GetRequisitionReasons()
        {
            var reasons = new List<GenericEnumModel>();
            var requisitionReasons = Enum.GetValues(typeof(RequisitionReason)).OfType<RequisitionReason>().ToList();
            int i = 0;

            foreach (var item in requisitionReasons)
            {
                reasons.Add(new GenericEnumModel
                {
                    Value = i,
                    ValueString = item.ToString()
                });
                i++;
            }

            return reasons;
        }

        public async Task<RequestLicense> UpdateIaFields(UpdateIaFieldsModel model)
        {
            var requestLicense = await _requestLicenseRepository.Select(model.RequestLicenseId);
            if (requestLicense != null)
            {
                requestLicense.Percentage = model.Percentage;
                requestLicense.Prediction = model.Prediction;
                requestLicense.Message = model.Message;
                await _requestLicenseRepository.Save();
            }
              
            return requestLicense; 
        }

    }
}
