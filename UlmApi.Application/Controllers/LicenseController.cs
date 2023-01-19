using UlmApi.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UlmApi.Domain.Models.Queries;
using UlmApi.Application.Models;
using System;
using UlmApi.Service.Validators;
using UlmApi.Domain.Entities;
using UlmApi.Domain.Entities.Enums;
using UlmApi.Application.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace UlmApi.Application.Controllers
{
    [Authorize]
    [Route("api/v1/licenses")]
    [ApiController]
    public class LicenseController : Controller
    {
        private readonly ILicenseService _licenseService;
        private readonly IBaseService<Solution, int> _baseSolutionService;

        public LicenseController(ILicenseService licenseService, IBaseService<Solution, int> baseSolutionService)
        {
            _licenseService = licenseService;
            _baseSolutionService = baseSolutionService;
        }

        [HttpGet, Route("")]
        public async Task<IActionResult> Get([FromQuery] GetLicensesPaginationQuery queryParams)
        {
            var licenses = await _licenseService.GetLicensesWithPagination(queryParams);
            return Ok(licenses);
        }

        [HttpGet, Route("solutions/{id}")]
        public async Task<IActionResult> GetLicensesBySolutionId([FromRoute] int id, [FromQuery] GetLicensesBySolutionQuery queryParams)
        {
            var solution = await _baseSolutionService.GetById<SolutionModel>(id);
            if (solution == null)
                return BadRequest("Solution not found.");

            var licenses = await _licenseService.GetLicensesBySolutionId(id, queryParams);
            return Ok(licenses);
        }

        [HttpGet, Route("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var license = await _licenseService.GetById(id);
            return Ok(license);
        }

        [Authorize(Policy = Policies.ADMIN_OR_OWNER)]
        [HttpPut, Route("{id}/archived")]
        public async Task<IActionResult> Archived([FromRoute] int id)
        {
            var license = await _licenseService.GetById<License>(id);
            if(license == null)
                return NotFound("License not found");

            await _licenseService.Archived(license);
            return NoContent();
        }

        [Authorize(Policy = Policies.ADMIN_OR_OWNER)]
        [HttpPut, Route("{id}/unarchived")]
        public async Task<IActionResult> Unarchived([FromRoute] int id)
        {
            var license = await _licenseService.GetById<License>(id);
            if(license == null)
                return NotFound("License not found");
            
            await _licenseService.Unarchived(license);
            return NoContent();
        }

        [Authorize(Policy = Policies.ADMIN_OR_OWNER)]
        [HttpPut, Route("{id}/place-in-use")]
        public async Task<IActionResult> PlaceLicenseInUse([FromRoute] int id)
        {
            var license = await _licenseService.GetById<License>(id);
            
            if (license == null)
                return NotFound("License not found");

            if (license.Status == LicenseStatus.IN_USE)
                return BadRequest("License already is in use ");

            await _licenseService.PlaceInUse(license);
            return NoContent();
        }

        [Authorize(Policy = Policies.ADMIN_OR_OWNER)]
        [HttpPut, Route("{id}/disassociated")]
        public async Task<IActionResult> DisassociateApplication([FromRoute] int id)
        {
            var license = await _licenseService.GetById<License>(id);
            if(license == null)
                return NotFound("License not found");
                
            await _licenseService.DisassociateApplication(license);
            return NoContent();
        }

        [Authorize(Policy = Policies.ADMIN_OR_OWNER)]
        [HttpPost, Route("")]
        public IActionResult Create([FromBody] CreateLicenseModel model)
        {
            return Execute(() => _licenseService.Create<CreateLicenseModel, LicenseValidator>(model).Result);
        }

        private IActionResult Execute(Func<object> func)
        {
            try
            {
                var result = func();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
