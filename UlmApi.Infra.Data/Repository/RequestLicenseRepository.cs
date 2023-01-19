using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UlmApi.Domain.Dtos;
using UlmApi.Domain.Entities;
using UlmApi.Domain.Interfaces;
using UlmApi.Domain.Models.Queries;

namespace UlmApi.Infra.Data.Repository
{
    public class RequestLicenseRepository : BaseRepository<RequestLicense,int> , IRequestLicenseRepository
    {   
        public RequestLicenseRepository(Context context) : base(context) { }

        public override async Task Insert(RequestLicense request)
        {
            await _context.RequestLicenses.AddAsync(request);
            await Save();
        }

        public override async Task<RequestLicense> Select(int id)
        {
            return await _context.RequestLicenses
                .Include(r => r.Product)
                .Include(r => r.Application)
                .Include(r => r.Solution)
                .Where(r => r.Id == id).FirstOrDefaultAsync();
        }

        public async Task<RequestLicensesListDto> GetRequestLicensesBySolutionId(GetRequestLicensesPaginationQuery queryParams, int solutionId)
        {
            return await GetRequests(queryParams, null, solutionId);
        }

        public async Task<RequestLicensesListDto> GetRequestLicensesByRequesterId(GetRequestLicensesPaginationQuery queryParams, string requesterId)
        {
            return await GetRequests(queryParams, requesterId);
        }

        public async Task<RequestLicensesListDto> GetAllRequests(GetRequestLicensesPaginationQuery queryParams)
        {
            return await GetRequests(queryParams);
        }

        private async Task<RequestLicensesListDto> GetRequests(GetRequestLicensesPaginationQuery queryParams, string requesterId = null, int? solutionId = null)
        {
            var query = _context.RequestLicenses
                    .Include(r => r.Application)
                    .Include(r => r.Solution)
                    .Include(r => r.Product)
                    .AsQueryable();

            query = query.Where(r => r.RegistrationDate.Date >= queryParams.InitialDate.Date
                && r.RegistrationDate.Date <= queryParams.FinalDate.Date
                || (r.RegistrationDate.Date >= queryParams.FinalDate.Date
                && r.RegistrationDate.Date <= queryParams.InitialDate.Date));

            if (queryParams.Status != null)
                query = query.Where(r => queryParams.Status.Contains(r.Status));

            if (!String.IsNullOrEmpty(requesterId))
                query = query.Where(r => r.RequesterId == requesterId);

            if (solutionId != null)
                query = query.Where(r => r.SolutionId == solutionId);

            if (!String.IsNullOrEmpty(queryParams.Term))
            {
                query = query.Where(r => r.RequesterName.ToLower().Contains(queryParams.Term.ToLower())
                || r.Application.Name.ToLower().Contains(queryParams.Term.ToLower()));
            }

            var totalPages = Math.Ceiling((decimal)query.Count() / queryParams.Limit);

            var requests = await ApplyOrdering(query, queryParams.OrderBy)
                    .Skip(queryParams.Page * queryParams.Limit)
                    .Take(queryParams.Limit)
                    .ToListAsync();

            return new RequestLicensesListDto
            {
                CurrentPage = queryParams.Page,
                TotalPages = (int)totalPages,
                ItemsPerPage = queryParams.Limit,
                Data = requests.Select(r => new RequestLicenseDto(r)).ToList()
            };
        }

        private IQueryable<RequestLicense> ApplyOrdering(IQueryable<RequestLicense> query, string orderBy)
        {
            switch (orderBy)
            {
                case "requesterName_desc":
                    query = query.OrderByDescending(l => l.RequesterName);
                    break;
                
                case "requesterName":
                    query = query.OrderBy(l => l.RequesterName);
                    break;

                case "registrationDate_desc":
                    query = query.OrderByDescending(l => l.RegistrationDate);
                    break;
                
                case "registrationDate":
                    query = query.OrderBy(l => l.RegistrationDate);
                    break;

                case "applicationName_desc":
                    query = query.OrderByDescending(l => l.Application.Name);
                    break;
                
                case "applicationName":
                    query = query.OrderBy(l => l.Application.Name);
                    break;

                case "status_desc":
                    query = query.OrderByDescending(l => l.Status);
                    break;
                
                case "status":
                    query = query.OrderBy(l => l.Status);
                    break;

                default:
                    query = query.OrderBy(s => s.Id);
                    break;
            }

            return query;
        }

    }
}