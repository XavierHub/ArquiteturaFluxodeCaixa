using AutoMapper;
using EmpXpo.Accounting.Domain;
using EmpXpo.Accounting.Domain.Abstractions.Application;
using Microsoft.AspNetCore.Mvc;

namespace EmpXpo.Accounting.CashFlowApi.Controllers
{
    [Route("api/cashFlowReports")]
    [ApiController]
    public class CashFlowReportController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IReportApplication _cashFlowReportApplication;

        public CashFlowReportController(IMapper mapper,
                                        IReportApplication cashFlowReportApplication
                                       )
        {
            _mapper = mapper;
            _cashFlowReportApplication = cashFlowReportApplication;
        }

        /// <summary>
        /// Lists available dates for the report
        /// </summary>        
        /// <response code="200">Dates found</response>
        /// <response code="400">Dates has missing/invalid values</response>
        /// <response code="500">Can't create your seller right now</response>        
        [ProducesResponseType(typeof(IEnumerable<DateTime>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(500)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _cashFlowReportApplication.ListDates();
            if (result.Count() == 0)
                return NotFound();

            return Ok(await _cashFlowReportApplication.ListDates());
        }

        /// <summary>
        /// Generates the daily consolidated report
        /// </summary>        
        /// <response code="200">Report generated successfully</response>
        /// <response code="400">Report has missing/invalid values</response>
        /// <response code="500">Can't Generates the daily consolidated report right now</response>        
        [ProducesResponseType(typeof(ReportModel), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(500)]
        [HttpGet]
        [Route("{date}")]
        public async Task<IActionResult> Get(DateTime date)
        {
            if (!ModelState.IsValid || date == DateTime.MinValue)
                return BadRequest();

            var result = await _cashFlowReportApplication.Report(date);

            return Ok(_mapper.Map<ReportModel>(result.FirstOrDefault()));
        }
    }
}
