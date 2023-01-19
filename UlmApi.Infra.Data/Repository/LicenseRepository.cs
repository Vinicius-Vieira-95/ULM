using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UlmApi.Domain.Dtos;
using UlmApi.Domain.Entities;
using UlmApi.Domain.Entities.Enums;
using UlmApi.Domain.Interfaces;
using UlmApi.Domain.Models.Queries;

namespace UlmApi.Infra.Data.Repository
{
    public class LicenseRepository : BaseRepository<License, int>, ILicenseRepository
    {
        public LicenseRepository(Context context) : base(context) { }

        public async Task<LicensesListDto> GetLicensesWithPagination(GetLicensesPaginationQuery queryParams, string userId = null, int? solutionId = null)
        {
            var query = _context.Licenses
                    .Include(l => l.Request)
                    .Include(l => l.Solution)
                    .Include(l => l.Application)
                    .Where(l => l.Archived == queryParams.Archived)
                    .AsQueryable();

            if (queryParams.Status != null)
                query = query.Where(l => queryParams.Status.Contains(l.Status));
            
            if (!String.IsNullOrEmpty(userId))
                query = query.Where(l => l.Request.RequesterId == userId);
            
            if (solutionId != null)
                query = query.Where(l => l.SolutionId == solutionId);

            if (!String.IsNullOrEmpty(queryParams.Term))
            {
                query = query.Where(l => l.Label.ToLower()
                    .Contains(queryParams.Term.ToLower()) || l.Application.Name.ToLower()
                    .Contains(queryParams.Term.ToLower()));
            }

            query = query.Where(l => (l.ExpirationDate.Date >= queryParams.InitialDate.Date
                && l.ExpirationDate.Date <= queryParams.FinalDate.Date)
                || (l.ExpirationDate.Date >= queryParams.FinalDate.Date
                && l.ExpirationDate.Date <= queryParams.InitialDate.Date));

            if (queryParams.IsExpired != null)
            {
                if (queryParams.IsExpired == true)
                    query = query.Where(l => DateTime.Now > l.ExpirationDate);
                
                if (queryParams.IsExpired == false)
                    query = query.Where(l => DateTime.Now <= l.ExpirationDate);
            }

            var totalPages = Math.Ceiling((decimal)query.Count() / queryParams.Limit);
            
            var licenses = await ApllyOrdering(query, queryParams.OrderBy)
                     .Skip(queryParams.Page * queryParams.Limit)
                     .Take(queryParams.Limit)
                     .ToListAsync();
           
            return new LicensesListDto
            {
                CurrentPage = queryParams.Page,
                TotalPages = (int)totalPages,
                ItemsPerPage = queryParams.Limit,
                Data = licenses.Select(l => new LicenseDto(l)).ToList()
            };
        }

        public override async Task Insert(License license)
        {
            await _context.Licenses.AddAsync(license);
            await Save();
        }

        public override async Task<License> Select(int id)
        {
            return await _context.Licenses
                .Include(l => l.Solution)
                .Include(l => l.Request)
                .Include(l => l.Application)
                .Where(r => r.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<License>> GetLicensesBySolutionId(int solutionId, GetLicensesBySolutionQuery queryParams)
        {
            var query = _context.Licenses
                .Include(l => l.Solution)
                .Include(l => l.Request)
                .Include(l => l.Application)
                .AsQueryable();

            if (queryParams.Status.Any())
                query = query.Where(l => l.SolutionId == solutionId && queryParams.Status.Contains(l.Status));
            
            if (queryParams.IsExpired != null)
            {
                if (queryParams.IsExpired == true)
                    query = query.Where(l => DateTime.Now > l.ExpirationDate);

                if (queryParams.IsExpired == false)
                    query = query.Where(l => DateTime.Now <= l.ExpirationDate);
            }
            
            return await query.OrderBy(l => l.ExpirationDate)
                .AsNoTracking()
                .ToListAsync();
        }

        private IQueryable<License> ApllyOrdering(IQueryable<License> query, string orderBy)
        {
            switch(orderBy)
            {
                case "label": 
                    query = query.OrderBy(q => q.Label);
                    break;

                case "label_desc": 
                    query = query.OrderByDescending(q => q.Label);
                    break;

                case "solution": 
                    query = query.OrderBy(q => q.Solution.Name);
                    break;

                case "solutionName_desc": 
                    query = query.OrderByDescending(q => q.Solution.Name);
                    break;

                case "applicationName":
                    query = query.OrderBy(q => q.Application.Name);
                    break;

                case "applicationName_desc":
                    query = query.OrderByDescending(q => q.Application.Name);
                    break;

                case "expirationDate": 
                    query = query.OrderBy(q => q.ExpirationDate);
                    break;

                 case "expirationDate_desc": 
                    query = query.OrderByDescending(q => q.ExpirationDate);
                    break;

                case "quantity": 
                    query = query.OrderBy(q => q.Quantity);
                    break;

                case "quantity_desc": 
                    query = query.OrderByDescending(q => q.Quantity);
                    break;

                case "status": 
                    query = query.OrderBy(q => q.Status);
                    break;

                case "status_desc":
                    query = query.OrderByDescending(q => q.Status);
                    break;

                default: 
                    query = query.OrderBy(q => q.ExpirationDate);
                    break;
            }
            return query;
        }
    }
}
