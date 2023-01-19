using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UlmApi.Application.Extensions;
using UlmApi.Domain.Entities.Enums;
using UlmApi.Domain.Interfaces;

namespace UlmApi.Application.Controllers
{
    [Authorize(Policy = Policies.ADMIN_OR_OWNER)]
    [Route("api/v1/dashboard")]
    [ApiController]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;
        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet, Route("expiring")]
        public async Task<IActionResult> GetCountLicensesExpiring()
        {
            return Ok(await _dashboardService.GetCountLicensesExpiring());
        }

        [HttpGet, Route("pending-aproval")]
        public async Task<IActionResult> GetCountRequestsPedingAproval()
        {
            return Ok(await _dashboardService.GetCountRequestsPendingAproval());
        }

        [HttpGet, Route("in-use")]
        public async Task<IActionResult> GetCountLicensesInUse()
        {
            return Ok(await _dashboardService.GetCountLicensesInUse());
        }

        [HttpGet, Route("applications-license-usage")]
        public async Task<IActionResult> GetApplicationsLicenseUsage([FromQuery] int limit = 5)
        {
            if (limit < 1)
                return BadRequest("the limit must be greater than 0.");

            return Ok(await _dashboardService.GetApplicationsLicenseUsage(limit));
        }

        [HttpGet, Route("licenses-per-solution")]
        public async Task<IActionResult> GetLicensesPerSolution([FromQuery] int limit = 5)
        {
            if (limit < 1)
                return BadRequest("the limit must be greater than 0.");

            return Ok(await _dashboardService.GetLicensesPerSolution(limit));
        }

        [HttpGet, Route("costs-per-month")]
        public async Task<IActionResult> GetLicenseCost()
        {
            return Ok(await _dashboardService.GetCostsPerMonth());
        }
    }
}
