using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UlmApi.Domain.Dtos;
using UlmApi.Domain.Entities;
using UlmApi.Domain.Interfaces;
using UlmApi.Domain.Models.Queries;

namespace UlmApi.Infra.Data.Repository
{
    public class ApplicationRepository : BaseRepository<Application, int>, IApplicationRepository
    {
        public ApplicationRepository(Context context) : base(context) { }

        public override async Task Insert(Application application)
        {
            await _context.Applications.AddAsync(application);
            await Save();
        }

        public async Task<ApplicationListDto> GetApplicationWithPagination(GetApplicationPaginationQuery queryParams)
        {
            return await GetApplications(queryParams);
        }

        public async Task<List<ApplicationDto>> GetApplications()
        {
            var application = await _context.Applications
                    .Include(s => s.Licenses)
                    .Include(s => s.RequestLicenses).ToListAsync();

            return application.Select(s => new ApplicationDto(s)).ToList();
        }

        public async Task<List<ApplicationDto>> GetApplications(int limit)
        {
            var application = await _context.Applications
                    .Include(s => s.Licenses)
                    .Include(s => s.RequestLicenses)
                    .OrderByDescending(s => s.Licenses.Count)
                    .Take(limit)
                    .ToListAsync();

            return application.Select(s => new ApplicationDto(s)).ToList();
        }

        private async Task<ApplicationListDto> GetApplications(GetApplicationPaginationQuery queryParams)
        {
            var query = _context.Applications
                    .Include(r => r.Licenses)
                    .Include(r => r.RequestLicenses)
                    .AsQueryable();

            if (!String.IsNullOrEmpty(queryParams.Term))
            {
                query = query.Where(r => r.Name.ToLower().Contains(queryParams.Term.ToLower()));
            }

            var totalPages = Math.Ceiling((decimal)query.Count() / queryParams.Limit);

            var applications = await ApplyOrdering(query, queryParams.OrderBy)
                    .Skip(queryParams.Page * queryParams.Limit)
                    .Take(queryParams.Limit)
                    .ToListAsync();

            return new ApplicationListDto
            {
                CurrentPage = queryParams.Page,
                TotalPages = (int)totalPages,
                ItemsPerPage = queryParams.Limit,
                Data = applications.Select(r => new ApplicationDto(r)).ToList()
            };
        }

        private IQueryable<Application> ApplyOrdering(IQueryable<Application> query, string orderBy)
        {
            switch (orderBy)
            {
                case "applicationName_desc":
                    query = query.OrderByDescending(l => l.Name);
                    break;

                case "applicationName":
                    query = query.OrderBy(l => l.Name);
                    break;

                case "creationDate_desc":
                    query = query.OrderByDescending(l => l.CreationDate);
                    break;

                case "creationDate":
                    query = query.OrderBy(l => l.CreationDate);
                    break;

                default:
                    query = query.OrderBy(s => s.Id);
                    break;
            }

            return query;
        }

        public async Task<ApplicationDto> GetApplicationByName(string applicatioName)
        {
            var application = await _context.Applications
                    .Where(a => a.Name.ToLower().Trim() == applicatioName.ToLower().Trim())
                    .FirstOrDefaultAsync();

            if (application == null)
                return null;
            
            return new ApplicationDto(application);
        }
    }
}
