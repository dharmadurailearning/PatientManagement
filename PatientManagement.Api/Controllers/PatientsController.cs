using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PatientManagement.Api.DTOs;
using PatientManagement.Api.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace PatientManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _service;
        public PatientsController(IPatientService service) => _service = service;

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PatientResponseDto>> GetById(Guid id)
        {
            var p = await _service.GetAsync(id);
            if (p == null) return NotFound();
            return Ok(ToDto(p));
        }

        [HttpGet("search")]
        public async Task<ActionResult<PagedResult<PatientResponseDto>>> Search([FromQuery] SearchPatientsRequest req)
        {
            // ModelState will be validated by [ApiController] for DataAnnotations (Range etc.)
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            // Business rule: MinAge <= MaxAge if both provided
            if (req.MinAge.HasValue && req.MaxAge.HasValue && req.MinAge > req.MaxAge)
                return BadRequest("MinAge must be less than or equal to MaxAge");

            var result = await _service.SearchAsync(req);
            return Ok(new PagedResult<PatientResponseDto>
            {
                Items = result.Items.Select(ToDto).ToList(),
                Total = result.Total,
                Page = result.Page,
                PageSize = result.PageSize
            });
        }

        [HttpPost("create")]
        public async Task<ActionResult<PatientResponseDto>> Create([FromBody] PatientCreateDto dto)
        {
            // check modelstate first (DataAnnotations)
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            // Additional check: DOB not in future
            if (dto.DOB > DateOnly.FromDateTime(DateTime.UtcNow.Date))
                return BadRequest("DOB can't be in the future");

            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, ToDto(created));
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<PatientResponseDto>> Update(Guid id, [FromBody] PatientUpdateDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            if (dto.DOB > DateOnly.FromDateTime(DateTime.UtcNow.Date))
                return BadRequest("DOB can't be in the future");

            var updated = await _service.UpdateAsync(id, dto);
            return Ok(ToDto(updated));
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        private static PatientResponseDto ToDto(PatientManagement.Api.Models.Patient p)
            => new(
                p.Id,
                p.FirstName,
                p.LastName,
                p.DOB,
                p.Gender,
                p.City,
                p.Email,
                p.Phone,
                p.PatientConditions.Select(pc => new PatientResponseDto.ConditionItem(pc.ConditionId, pc.Condition.Name, pc.DiagnosedDate)).ToList()
            );
    }
}