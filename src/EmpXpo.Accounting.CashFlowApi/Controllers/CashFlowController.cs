using AutoMapper;
using EmpXpo.Accounting.Domain;
using EmpXpo.Accounting.Domain.Abstractions.Application;
using Microsoft.AspNetCore.Mvc;

namespace EmpXpo.Accounting.CashFlowApi.Controllers
{
    [Route("api/cashFlows")]
    [ApiController]
    public class CashFlowController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICashFlowApplication _cashFlowApplication;

        public CashFlowController(IMapper mapper,
                                  ICashFlowApplication cashFlowApplication
                                 )
        {
            _mapper = mapper;
            _cashFlowApplication = cashFlowApplication;
        }

        /// <summary>
        /// Create a specific CashFlow
        /// </summary>        
        /// <response code="201">CashFlow created</response>
        /// <response code="404">CashFlow has missing/invalid values</response>
        /// <response code="500">Can't create your cashFlow right now</response>
        [ProducesResponseType(typeof(CashFlowModel), 201)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(500)]
        [HttpPost]
        public async Task<IActionResult> Create(CashFlowModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _cashFlowApplication.Create(_mapper.Map<CashFlow>(model));

            if (result?.Id == 0)
                return BadRequest();

            var uri = Url.Action("Get", new { id = result.Id });

            return Created(uri, _mapper.Map<CashFlowModel>(result));
        }
    }
}
