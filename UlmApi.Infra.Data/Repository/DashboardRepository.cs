using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UlmApi.Domain.Dtos;
using UlmApi.Domain.Entities;
using UlmApi.Domain.Entities.Enums;
using UlmApi.Domain.Interfaces;

namespace UlmApi.Infra.Data.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly Context _context;
        
        public DashboardRepository(Context context)
        {
            _context = context;
        }

        public async Task<CountLicensesExpiringDto> GetCountLicensesExpiring(int? solutionId = null)
        {
            var licensesQuery = _context.Licenses.AsQueryable();

            if (solutionId != null)
                licensesQuery = licensesQuery.Where(l => l.SolutionId == solutionId);

            var licenses = await licensesQuery.ToListAsync();
            var total = licenses.Count;
            var expiring = licenses.Where(l => l.ExpirationDate >= DateTime.Now && l.ExpirationDate <= DateTime.Now.AddDays(30)).Count();

            return new CountLicensesExpiringDto(expiring, total);
        } 

        public async Task<CountRequestsPendingAprovalDto> GetCountRequestsPendingAproval(int? solutionId = null)
        {
            var requestsQuery = _context.RequestLicenses.AsQueryable();
            
            if (solutionId != null)
                requestsQuery = requestsQuery.Where(r => r.SolutionId == solutionId);

            var requests = await requestsQuery.ToListAsync();
            var total = requests.Count;
            var pedingAproval = requests.Where(r => r.Status == RequestLicenseStatus.CREATED).Count();

            return new CountRequestsPendingAprovalDto(pedingAproval, total);
        }

        public async Task<CountLicensesDto> GetCountLicensesInUse(int? solutionId = null)
        {
            var licensesQuery = _context.Licenses.AsQueryable();

            if (solutionId != null)
                licensesQuery = licensesQuery.Where(l => l.SolutionId == solutionId);

            var licenses = await licensesQuery.ToListAsync();
            var total = licenses.Count;
            var inUse = licenses.Where(l => l.Status == LicenseStatus.IN_USE).Count();
            return new CountLicensesDto(inUse, total);
        }

        public async Task<int> GetCountLicensesByApplicationId(int applicationId)
        {
            return await _context.Licenses.Where(l => l.ApplicationId == applicationId).CountAsync();
        }

        public async Task<int> GetCountLicensesBySolutionId(int solutionId, LicenseStatus status)
        {
            return await _context.Licenses.Where(l => l.SolutionId == solutionId && l.Status == status).CountAsync();
        }

        public async Task<double> GetCostPerYearMoth(int year, int month)
        {
            return (double) await _context.Licenses
                                    .Where(l => l.AquisitionDate.Year == year  && l.AquisitionDate.Month == month)
                                    .Select(l => l.Price)
                                    .SumAsync();
        }

    }
}
