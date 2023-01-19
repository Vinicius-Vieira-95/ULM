using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using UlmApi.Domain.Dtos;
using UlmApi.Domain.Entities.Enums;
using UlmApi.Domain.Interfaces;

namespace UlmApi.Service.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IApplicationRepository _applicationRepository;
        private readonly ISolutionRepository _solutionRepository;
        private readonly AuthenticatedUserService _authenticatedUser;

        public DashboardService(IDashboardRepository dashboardRepository, 
            IApplicationRepository applicationRepository,
            AuthenticatedUserService authenticatedUser,
            ISolutionRepository solutionRepository)
        {
            _dashboardRepository = dashboardRepository;
            _applicationRepository = applicationRepository;
            _solutionRepository = solutionRepository;
            _authenticatedUser = authenticatedUser;
        }

        public async Task<CountLicensesExpiringDto> GetCountLicensesExpiring()
        {
            if (_authenticatedUser.Role == "OWNER")
            {
                var solution = await _solutionRepository.GetSolutionByOwnerId(_authenticatedUser.Id);
                if (solution != null)
                    return await _dashboardRepository.GetCountLicensesExpiring(solution.Id);

                return new CountLicensesExpiringDto(0, 0);
            }

            return await _dashboardRepository.GetCountLicensesExpiring();
        }

        public async Task<CountRequestsPendingAprovalDto> GetCountRequestsPendingAproval()
        {
            if (_authenticatedUser.Role == "OWNER")
            {
                var solution = await _solutionRepository.GetSolutionByOwnerId(_authenticatedUser.Id);
                if (solution != null)
                    return await _dashboardRepository.GetCountRequestsPendingAproval(solution.Id);

                return new CountRequestsPendingAprovalDto(0, 0);
            }

            return await _dashboardRepository.GetCountRequestsPendingAproval();
        }

        public async Task<CountLicensesDto> GetCountLicensesInUse()
        {
            if (_authenticatedUser.Role == "OWNER")
            {
                var solution = await _solutionRepository.GetSolutionByOwnerId(_authenticatedUser.Id);
                if (solution != null)
                    return await _dashboardRepository.GetCountLicensesInUse(solution.Id);

                return new CountLicensesDto(0, 0);
            }

            return await _dashboardRepository.GetCountLicensesInUse();
        }

        public async Task<List<ApplicationLicenseUsageDto>> GetApplicationsLicenseUsage(int limit)
        {
            var applications = await _applicationRepository.GetApplications(limit);
            var applicationsLicenseUsage = new List<ApplicationLicenseUsageDto>();

            foreach (var application in applications)
            {
                applicationsLicenseUsage.Add(new ApplicationLicenseUsageDto 
                {
                    Application = application.Name,
                    Value = await _dashboardRepository.GetCountLicensesByApplicationId(application.Id)
                });
            }

            return applicationsLicenseUsage;
        }

        public async Task<LicensePerSolutionListDto> GetLicensesPerSolution(int limit)
        {
            var solutions = await _solutionRepository.GetSolutions(limit);
            var licensesPerSolution = new List<LicensePerSolutionDto>();

            foreach (var solution in solutions)
            {
                licensesPerSolution.Add(new LicensePerSolutionDto
                {
                    Solution = solution.Name,
                    InUse = await _dashboardRepository.GetCountLicensesBySolutionId(solution.Id, LicenseStatus.IN_USE),
                    StandBy = await _dashboardRepository.GetCountLicensesBySolutionId(solution.Id, LicenseStatus.STAND_BY)
                });
            }

            return new LicensePerSolutionListDto
            {
                TotalLicenses = licensesPerSolution.Select(s => s.Total).Sum(),
                Data = licensesPerSolution
            };
        }

        public async Task<List<CostLicensePerMothDto>> GetCostsPerMonth()
        {
            var months = CultureInfo.GetCultureInfo("en-US").DateTimeFormat.MonthNames;
            var costs = new List<CostLicensePerMothDto>();

            for (int i = 0; i < months.Count() -1; i++)
            {
                var totalPerMonth = await _dashboardRepository.GetCostPerYearMoth(DateTime.Now.Year, i + 1);
                costs.Add(new CostLicensePerMothDto(months[i], totalPerMonth));
            }

            return costs;
        }
        
    }
}
