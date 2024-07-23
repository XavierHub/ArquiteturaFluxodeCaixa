using AutoMapper;
using EmpXpo.Accounting.Domain;
using EmpXpo.Accounting.Domain.Abstractions.Application;
using Microsoft.AspNetCore.Mvc;

namespace EmpXpo.Accounting.CashFlowSellerApi.Controllers
{
    [Route("api/Sellers")]
    [ApiController]
    public class CashFlowSellerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISellerApplication _sellerApplication;

        public CashFlowSellerController(IMapper mapper,
                                ISellerApplication sellerApplication
                               )
        {
            _mapper = mapper;
            _sellerApplication = sellerApplication;
        }

        /// <summary>
        /// Create a specific seller
        /// </summary>        
        /// <response code="201">Seller created</response>
        /// <response code="404">Seller has missing/invalid values</response>
        /// <response code="500">Can't create your seller right now</response>        
        [ProducesResponseType(typeof(SellerModel), 201)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(500)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SellerModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _sellerApplication.Create(_mapper.Map<Seller>(model));

            var uri = Url.Action("Get", new { id = model.Id });

            return Created(uri, _mapper.Map<SellerModel>(result));
        }

        /// <summary>
        /// Update a specific seller by Id
        /// </summary>        
        /// <response code="200">Seller Updated</response>
        /// <response code="404">Seller has missing/invalid values</response>
        /// <response code="500">Can't update your seller right now</response>        
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(500)]
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SellerModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _sellerApplication.Update(id, _mapper.Map<Seller>(model));

            return result ? Ok() : NotFound();
        }

        /// <summary>
        /// Get a specific seller by Id
        /// </summary>        
        /// <response code="200">Seller found</response>
        /// <response code="404">Seller has missing/invalid values</response>
        /// <response code="500">Can't get your seller right now</response>        
        [ProducesResponseType(typeof(SellerModel), 200)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(500)]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _sellerApplication.Get(id);

            if (result.Id == 0)
                return NotFound();

            return Ok(_mapper.Map<SellerModel>(result));
        }

        /// <summary>
        /// Get all sellers
        /// </summary>        
        /// <response code="200">Sellers found</response>        
        /// <response code="404">Seller has missing/invalid values</response>
        /// <response code="500">Unable to reach sellers at this time</response>        
        [ProducesResponseType(typeof(IEnumerable<SellerModel>), 200)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(500)]
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _sellerApplication.GetAll();

            if (result.Count() == 0)
                return NotFound();

            return Ok(_mapper.Map<IEnumerable<SellerModel>>(result));
        }

        /// <summary>
        /// Delete a specific seller by Id
        /// </summary>        
        /// <response code="200">Sellers deleted</response>
        /// <response code="404">Seller has missing/invalid values</response>
        /// <response code="500">Can't delete your seller right now</response>        
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(500)]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _sellerApplication.Delete(id);

            return result ? Ok() : NotFound();
        }
    }
}
