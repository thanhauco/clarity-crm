using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Clarity.Core.Interfaces;
using Clarity.Core.Models;

namespace Clarity.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetAll([FromQuery] string search = null)
        {
            if (!string.IsNullOrEmpty(search))
            {
                var results = await _customerService.SearchCustomersAsync(search);
                return Ok(results);
            }

            var customers = await _customerService.GetAllCustomersAsync();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetById(int id)
        {
            var customer = await _customerService.GetCustomerAsync(id);
            if (customer == null)
                return NotFound();

            return Ok(customer);
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> Create([FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _customerService.CreateCustomerAsync(customer);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Customer>> Update(int id, [FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updated = await _customerService.UpdateCustomerAsync(id, customer);
                return Ok(updated);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _customerService.DeleteCustomerAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpGet("stats")]
        public async Task<ActionResult<CustomerStats>> GetStats()
        {
            var stats = await _customerService.GetStatsAsync();
            return Ok(stats);
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LeadsController : ControllerBase
    {
        private readonly ILeadService _leadService;

        public LeadsController(ILeadService leadService)
        {
            _leadService = leadService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lead>>> GetAll([FromQuery] LeadStatus? status = null)
        {
            if (status.HasValue)
            {
                var results = await _leadService.GetLeadsByStatusAsync(status.Value);
                return Ok(results);
            }

            var leads = await _leadService.GetAllLeadsAsync();
            return Ok(leads);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Lead>> GetById(int id)
        {
            var lead = await _leadService.GetLeadAsync(id);
            if (lead == null)
                return NotFound();

            return Ok(lead);
        }

        [HttpPost]
        public async Task<ActionResult<Lead>> Create([FromBody] Lead lead)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _leadService.CreateLeadAsync(lead);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Lead>> Update(int id, [FromBody] Lead lead)
        {
            try
            {
                var updated = await _leadService.UpdateLeadAsync(id, lead);
                return Ok(updated);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPatch("{id}/status")]
        public async Task<ActionResult<Lead>> UpdateStatus(int id, [FromBody] LeadStatus status)
        {
            try
            {
                var updated = await _leadService.UpdateStatusAsync(id, status);
                return Ok(updated);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("{id}/convert")]
        public async Task<ActionResult<Customer>> ConvertToCustomer(int id)
        {
            try
            {
                var customer = await _leadService.ConvertToCustomerAsync(id);
                return Ok(customer);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("stats")]
        public async Task<ActionResult<LeadStats>> GetStats()
        {
            var stats = await _leadService.GetStatsAsync();
            return Ok(stats);
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ActivitiesController : ControllerBase
    {
        private readonly IActivityService _activityService;

        public ActivitiesController(IActivityService activityService)
        {
            _activityService = activityService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Activity>>> GetPending()
        {
            var activities = await _activityService.GetPendingActivitiesAsync();
            return Ok(activities);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Activity>> GetById(int id)
        {
            var activity = await _activityService.GetActivityAsync(id);
            if (activity == null)
                return NotFound();

            return Ok(activity);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<Activity>>> GetByCustomer(int customerId)
        {
            var activities = await _activityService.GetActivitiesByCustomerAsync(customerId);
            return Ok(activities);
        }

        [HttpPost]
        public async Task<ActionResult<Activity>> Create([FromBody] Activity activity)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _activityService.CreateActivityAsync(activity);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Activity>> Update(int id, [FromBody] Activity activity)
        {
            try
            {
                var updated = await _activityService.UpdateActivityAsync(id, activity);
                return Ok(updated);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("{id}/complete")]
        public async Task<ActionResult<Activity>> Complete(int id)
        {
            try
            {
                var completed = await _activityService.CompleteActivityAsync(id);
                return Ok(completed);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _activityService.DeleteActivityAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResult>> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.LoginAsync(request.Username, request.Password);
            if (!result.Success)
                return Unauthorized(result);

            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResult>> Register([FromBody] RegisterRequest request)
        {
            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Role = UserRole.SalesRep
            };

            var result = await _authService.RegisterAsync(user, request.Password);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
