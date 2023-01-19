using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UlmApi.Domain.Dtos;
using UlmApi.Domain.Entities;
using UlmApi.Domain.Interfaces;
using UlmApi.Domain.Models.Queries;

namespace UlmApi.Infra.Data.Repository
{
    public class SolutionRepository : BaseRepository<Solution, int>, ISolutionRepository
    {
        public SolutionRepository(Context context) : base(context) { }

        public async Task<List<SolutionDto>> GetSolutions()
        {
            var solutions = await _context.Solutions
                .Include(s => s.Product)
                .ToListAsync();
            
            return solutions.Select(s => new SolutionDto(s)).ToList();
        }

        public async Task<List<SolutionDto>> GetSolutions(int limit)
        {
            var solutions = await _context.Solutions
                .Include(s => s.Product)
                .OrderByDescending(s => s.Licenses.Count)
                .Take(limit)
                .ToListAsync();
            
            return solutions.Select(s => new SolutionDto(s)).ToList();
        }

        public async Task<SolutionDto> GetSolutionByName(string solutionName)
        {
            var solution = await _context.Solutions
                    .Include(s => s.Product)
                    .Where(s => s.Name.ToLower().Trim() == solutionName.ToLower().Trim())
                    .FirstOrDefaultAsync();

            if (solution == null)
                return null;

            return new SolutionDto(solution);
        }

        public async Task<SolutionDto> GetSolutionByOwnerId(string ownerId)
        {
            var solution = await _context.Solutions
                    .Include(s => s.Product)
                    .Where(s => s.OwnerId == ownerId).FirstOrDefaultAsync();
            
            if (solution == null)
                return null;
            
            return new SolutionDto(solution);
        }

        public async Task<SolutionsListDto> GetSolutionsWithPagination(GetSolutionsQuery queryParams)
        {
            var query = _context.Solutions
                    .Include(s => s.Product)
                    .AsQueryable();

            if (!String.IsNullOrEmpty(queryParams.Term))
            {
                query = query.Where(s => s.Name.ToLower().Contains(queryParams.Term.ToLower()) 
                    || s.OwnerName.ToLower().Contains(queryParams.Term.ToLower()));
            }

            var totalPages = Math.Ceiling((decimal)query.Count() / queryParams.Limit);

            var licenses = await ApplyOrdering(query, queryParams.OrderBy)
                    .Skip(queryParams.Page * queryParams.Limit)
                    .Take(queryParams.Limit)
                    .ToListAsync();

            return new SolutionsListDto
            {
                CurrentPage = queryParams.Page,
                TotalPages = (int)totalPages,
                ItemsPerPage = queryParams.Limit,
                Data = licenses.Select(r => new SolutionDto(r)).ToList()
            };
        }

        public override async Task<Solution> Select(int id)
        {
            return await _context.Solutions
                .Include(r => r.Product)
                .Where(r => r.Id == id).FirstOrDefaultAsync();
        }

        private IQueryable<Solution> ApplyOrdering(IQueryable<Solution> query, string orderBy)
        {
            switch (orderBy)
            {
                case "ownerName_desc":
                    query = query.OrderByDescending(s => s.OwnerName);
                    break;
                
                case "ownerName":
                    query = query.OrderBy(s => s.OwnerName);
                    break;

                case "name_desc":
                    query = query.OrderByDescending(s => s.Name);
                    break;
                
                case "name":
                    query = query.OrderBy(s => s.Name);
                    break;

                default:
                    query = query.OrderBy(s => s.Id);
                    break;
            }

            return query;
        }

    }
}
