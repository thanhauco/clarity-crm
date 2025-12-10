using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Clarity.Core.Models;
using Clarity.Services;

namespace Clarity.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OpportunitiesController : ControllerBase
    {
        private readonly IOpportunityService _service;

        public OpportunitiesController(IOpportunityService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Opportunity>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Opportunity>> GetById(int id)
        {
            var opportunity = await _service.GetByIdAsync(id);
            if (opportunity == null) return NotFound();
            return Ok(opportunity);
        }

        [HttpPost]
        public async Task<ActionResult<Opportunity>> Create(Opportunity opportunity)
        {
            var created = await _service.CreateAsync(opportunity);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Opportunity>> Update(int id, Opportunity opportunity)
        {
            try
            {
                return Ok(await _service.UpdateAsync(id, opportunity));
            }
            catch (System.Collections.Generic.KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPatch("{id}/stage")]
        public async Task<ActionResult<Opportunity>> UpdateStage(int id, [FromBody] OpportunityStage stage)
        {
            try
            {
                return Ok(await _service.UpdateStageAsync(id, stage));
            }
            catch (System.Collections.Generic.KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (await _service.DeleteAsync(id)) return NoContent();
            return NotFound();
        }
    }
}
