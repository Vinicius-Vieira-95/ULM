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
    [Route("api/v1/applications")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService _applicationService;

        public ApplicationController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpGet, Route("")]
        public async Task<IActionResult> GetAllApplications()
        {
            return Ok(await _applicationService.GetAllApplications());
        }

        [HttpGet, Route("paginated")]
        public async Task<IActionResult> GetAllApplications([FromQuery] GetApplicationPaginationQuery queryParams)
        {
            var applications = await _applicationService.GetApplicationWithPagination(queryParams);
            return Ok(applications);
        }

        [HttpGet, Route("{id}")]
        public async Task<IActionResult> GetApplication(int id)
        {
            var application = await _applicationService.GetApplicationById(id);

            if (application == null)
                return NotFound("Application not found");

            return Ok(application);
        }

        [Authorize(Policy = Policies.ADMIN)]
        [HttpPost, Route("")]
        public IActionResult Create([FromBody] CreateApplicationModel model)
        {
            try
            {
                var application = _applicationService.Create<CreateApplicationModel, ApplicationValidator>(model).Result;
                return CreatedAtAction(nameof(GetApplication), new { id = application.Id }, application);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.Message);
            }
        }
    }
}
