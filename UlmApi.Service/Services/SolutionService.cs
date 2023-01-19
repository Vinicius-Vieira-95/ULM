using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using UlmApi.Domain.Dtos;
using UlmApi.Domain.Entities;
using UlmApi.Domain.Interfaces;
using UlmApi.Domain.Models.Queries;
using UlmApi.Infra.CrossCutting.Logger;

namespace UlmApi.Service.Services
{
    public class SolutionService : BaseService<Solution, int>, ISolutionService
    {
        private readonly ISolutionRepository _solutionRepository;
        private readonly IMapper _mapper;
        private readonly IBaseService<Product, int> _baseProductService;
        private readonly IUserService _userService;
        private readonly AuthenticatedUserService _authenticatedUser;

        public SolutionService(ISolutionRepository solutionRepository, 
            IMapper mapper, 
            IUserService userService,
            AuthenticatedUserService authenticatedUser,
            IBaseService<Product, int> baseProductService) : base(solutionRepository, mapper)
        {
            _solutionRepository = solutionRepository;
            _mapper = mapper;
            _baseProductService = baseProductService;
            _userService = userService;
            _authenticatedUser = authenticatedUser;
        }

        public async Task<SolutionDto> Create<TInputModel, TValidator>(TInputModel inputModel)
            where TInputModel : class
            where TValidator : AbstractValidator<Solution>
        {
            var solution = _mapper.Map<Solution>(inputModel);

            Validate(solution, Activator.CreateInstance<TValidator>());
            await ValidateIfRelationshipsExistAndRoles(solution);

            solution.OwnerName = (await _userService.GetUserById(solution.OwnerId)).FullName;
            await _solutionRepository.Insert(solution);

            return new SolutionDto(await _solutionRepository.Select(solution.Id));
        }

        public async Task<SolutionDto> GetSolutionById(int id)
        {
            var solution = await _solutionRepository.Select(id);
            if (solution == null)
                return null;

            return new SolutionDto(solution);
        }

        public async Task<List<SolutionDto>> GetSolutions()
        {
            if (_authenticatedUser.Role == "OWNER")
            {
                var solution = await _solutionRepository.GetSolutionByOwnerId(_authenticatedUser.Id);
                return new List<SolutionDto> {solution};
            }

            return await _solutionRepository.GetSolutions();
        }

        public async Task<SolutionsListDto> GetSolutionsWithPagination(GetSolutionsQuery queryParams)
        {
            return await _solutionRepository.GetSolutionsWithPagination(queryParams);
        }

        private async Task ValidateIfRelationshipsExistAndRoles(Solution solution)
        {
            var solutionUser = await _solutionRepository.GetSolutionByOwnerId(solution.OwnerId);
            var user = await _userService.GetUserById(solution.OwnerId);
            if (user == null)
            {
                throw new InvalidOperationException("Owner not found.");
            }

            if (user.Role != "OWNER")
            {
                throw new InvalidOperationException("The user must have an owner level to be associated with a solution.");
            }

            if (solutionUser != null)
            {
                throw new InvalidOperationException("This user already owns a solution.");
            }

            var product = await _baseProductService.GetById<Product>((int) solution.ProductId);
            if (product == null)
            {   
                throw new InvalidOperationException("Product not found.");
            }

            var solutionByName = await _solutionRepository.GetSolutionByName(solution.Name);
            if (solutionByName != null)
            {
                throw new InvalidOperationException("A solution already exists to this name.");
            }
                
        }
    }
}
