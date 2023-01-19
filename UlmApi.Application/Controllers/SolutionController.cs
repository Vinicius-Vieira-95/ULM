using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UlmApi.Application.Extensions;
using UlmApi.Application.Models;
using UlmApi.Domain.Interfaces;
using UlmApi.Domain.Models.Queries;
using UlmApi.Service.Validators;

namespace UlmApi.Application.Controllers
{
    [Authorize]
    [Route("api/v1/solutions")]
    [ApiController]
    public class SolutionController : Controller
    {
        private readonly ISolutionService _solutionService;
        public SolutionController(ISolutionService solutionService)
        {
            _solutionService = solutionService;
        }

        [HttpGet, Route("")]
        public async Task<IActionResult> GetSolutions()
        {
            return Ok(await _solutionService.GetSolutions());
        }

        [HttpGet, Route("paginated")]
        public async Task<IActionResult> GetSolutions([FromQuery] GetSolutionsQuery query)
        {
            return Ok(await _solutionService.GetSolutionsWithPagination(query));
        }

        [Authorize(Policy = Policies.ADMIN)]
        [HttpPost, Route("")]
        public IActionResult Create([FromBody] CreateSolutionModel model)
        {
            try
            {
                var solution = _solutionService.Create<CreateSolutionModel, SolutionValidator>(model).Result;
                return CreatedAtAction(nameof(GetSolution), new { id = solution.Id }, solution);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.Message);
            }
        }

        [HttpGet, Route("{id}")]
        public async Task<IActionResult> GetSolution(int id)
        {
            var solution = await _solutionService.GetSolutionById(id);
            if (solution == null)
                return NotFound("Solution not found");
            
            return Ok(solution);
        }

    }
}
