using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UlmApi.Application.Extensions;
using UlmApi.Application.Models;
using UlmApi.Domain.Entities;
using UlmApi.Domain.Entities.Enums;
using UlmApi.Domain.Interfaces;
using UlmApi.Domain.Models.Queries;
using UlmApi.Service.Validators;

namespace UlmApi.Application.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/requests")]
    public class ResquestLicenseController : ControllerBase
    {
        private readonly IRequestLicenseService _requestLicenseService;
        private readonly ILicenseService _licenseService;

        public ResquestLicenseController(IRequestLicenseService requestLicenseService, ILicenseService licenseService)
        {
            _requestLicenseService = requestLicenseService;
            _licenseService = licenseService;
        }

        [Authorize(Policy = Policies.ADMIN_OR_OWNER)]
        [HttpPut, Route("{id}/analyze")]
        public async Task<IActionResult> AnalyzeRequest([FromRoute] int id)
        {
            var requestLicense = await _requestLicenseService.GetById<RequestLicense>(id);
            if(requestLicense == null)
                return NotFound();

            await _requestLicenseService.ChangeStatusRequest(requestLicense, RequestLicenseStatus.IN_ANALYSIS);
            return NoContent();
        }

        [Authorize(Policy = Policies.ADMIN_OR_OWNER)]
        [HttpPut, Route("{id}/approve")]
        public async Task<IActionResult> ApproveRequest([FromRoute]int id, [FromBody] LicenseModel licenseId)
        {
            var requestLicense = await _requestLicenseService.GetById<RequestLicense>(id);
            
            if(requestLicense == null)
                return NotFound();

            var license = await _licenseService.GetById<License>(licenseId.LicenseId);
            if (license == null) return NotFound();
            if (license.Status != LicenseStatus.STAND_BY) return BadRequest();

            await _requestLicenseService.ChangeStatusRequest(requestLicense, license, RequestLicenseStatus.ACCEPTED);

            return NoContent();
        }

        [Authorize(Policy = Policies.ADMIN_OR_OWNER)]
        [HttpPut, Route("{id}/refused")]
        public async Task<IActionResult> RefusedRequest([FromRoute] int id, [FromBody] JustificationForDenyModel justificationForDeny)
        {
            var requestLicense = await _requestLicenseService.GetById<RequestLicense>(id);

            if (requestLicense == null)
                return NotFound();

            if (String.IsNullOrEmpty(justificationForDeny.Justification))
                return BadRequest();

            await _requestLicenseService.ChangeStatusRequest(requestLicense, RequestLicenseStatus.REFUSED, justificationForDeny.Justification);

            return NoContent();
        }

        [HttpGet, Route("")]
        public async Task<IActionResult> Get([FromQuery] GetRequestLicensesPaginationQuery queryParams)
        {
            var requestsLicenses = await _requestLicenseService.GetRequestLicensesWithPagination(queryParams);
            return Ok(requestsLicenses);
        }

        [HttpGet, Route("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var requestLicense = await _requestLicenseService.GetById(id);
            if (requestLicense == null)
                return NotFound();

            return Ok(requestLicense);
        }

        [HttpGet, Route("usage-time")]
        public IActionResult GetUsageTime()
        {
            var allUsageTime = _requestLicenseService.GetUsageTimeEnum();

            return Ok(allUsageTime);
        }

        [HttpGet("requisition-reasons")]
        public IActionResult GetRequisitionReason()
        {
            var reasons = _requestLicenseService.GetRequisitionReasons();

            return Ok(reasons);
        }
        
        [HttpPost, Route("")]
        public IActionResult Create([FromBody] CreateRequestLicenseModel model)
        {
            return Execute(() => _requestLicenseService.Create<CreateRequestLicenseModel, RequestLicenseValidator>(model).Result);
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