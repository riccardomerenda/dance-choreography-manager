using DanceChoreographyManager.Services.Dancer.DTOs;
using DanceChoreographyManager.Services.Dancer.Models;
using DanceChoreographyManager.Services.Dancer.Repositories;
using DanceChoreographyManager.Shared.DTOs.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DanceChoreographyManager.Services.Dancer.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DancerController : ControllerBase
{
    private readonly IDancerRepository _dancerRepository;
    private readonly ILogger<DancerController> _logger;

    public DancerController(
        IDancerRepository dancerRepository,
        ILogger<DancerController> logger)
    {
        _dancerRepository = dancerRepository;
        _logger = logger;
    }

    /// <summary>
    /// Get all dancers with filtering and pagination
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<PagedResponse<DancerDto>>> GetDancers([FromQuery] DancerFilterParams filterParams)
    {
        var (dancers, totalCount) = await _dancerRepository.GetFilteredAsync(filterParams);

        var dancerDtos = dancers.Select(dancer => MapToDancerDto(dancer)).ToList();

        var response = new PagedResponse<DancerDto>
        {
            Page = filterParams.Page,
            PageSize = filterParams.PageSize,
            TotalCount = totalCount,
            Items = dancerDtos
        };

        return Ok(response);
    }

    /// <summary>
    /// Get a dancer by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<DancerDto>>> GetDancer(Guid id)
    {
        var dancer = await _dancerRepository.GetByIdWithDetailsAsync(id);
        if (dancer == null)
        {
            return NotFound(ApiResponse<DancerDto>.CreateFailure($"Dancer with ID {id} not found"));
        }

        var dancerDto = MapToDancerDto(dancer);
        return Ok(ApiResponse<DancerDto>.CreateSuccess(dancerDto));
    }

    /// <summary>
    /// Create a new dancer
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<DancerDto>>> CreateDancer(CreateDancerDto createDancerDto)
    {
        // Check if email already exists
        if (!string.IsNullOrWhiteSpace(createDancerDto.Email) &&
            await _dancerRepository.EmailExistsAsync(createDancerDto.Email))
        {
            ModelState.AddModelError("Email", "A dancer with this email already exists");
            return BadRequest(ApiResponse<DancerDto>.CreateFailure("A dancer with this email already exists"));
        }

        var dancer = new Models.Dancer
        {
            Id = Guid.NewGuid(),
            FirstName = createDancerDto.FirstName,
            LastName = createDancerDto.LastName,
            Email = createDancerDto.Email,
            PhoneNumber = createDancerDto.PhoneNumber,
            DateOfBirth = createDancerDto.DateOfBirth,
            Gender = createDancerDto.Gender,
            HeightCm = createDancerDto.HeightCm,
            WeightKg = createDancerDto.WeightKg,
            ExperienceLevel = createDancerDto.ExperienceLevel,
            EmergencyContactName = createDancerDto.EmergencyContactName,
            EmergencyContactPhone = createDancerDto.EmergencyContactPhone,
            MedicalNotes = createDancerDto.MedicalNotes,
            Notes = createDancerDto.Notes,
            JoinedDate = DateTime.UtcNow,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = User.Identity?.Name ?? "system"
        };

        // Add dance styles if provided
        if (createDancerDto.DanceStyles != null && createDancerDto.DanceStyles.Any())
        {
            dancer.DanceStyles = createDancerDto.DanceStyles.Select(ds => new DancerStyle
            {
                Id = Guid.NewGuid(),
                DancerId = dancer.Id,
                Style = ds.Style,
                ProficiencyLevel = ds.ProficiencyLevel,
                YearsOfExperience = ds.YearsOfExperience,
                Notes = ds.Notes,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = User.Identity?.Name ?? "system"
            }).ToList();
        }

        await _dancerRepository.AddAsync(dancer);

        _logger.LogInformation("Dancer created: {DancerId} - {FullName}", dancer.Id, dancer.FullName);

        var dancerDto = MapToDancerDto(dancer);
        return CreatedAtAction(nameof(GetDancer), new { id = dancer.Id }, 
            ApiResponse<DancerDto>.CreateSuccess(dancerDto, "Dancer created successfully"));
    }

    /// <summary>
    /// Update an existing dancer
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<DancerDto>>> UpdateDancer(Guid id, UpdateDancerDto updateDancerDto)
    {
        var dancer = await _dancerRepository.GetByIdWithDetailsAsync(id);
        if (dancer == null)
        {
            return NotFound(ApiResponse<DancerDto>.CreateFailure($"Dancer with ID {id} not found"));
        }

        // Check if email already exists
        if (!string.IsNullOrWhiteSpace(updateDancerDto.Email) && 
            updateDancerDto.Email != dancer.Email &&
            await _dancerRepository.EmailExistsAsync(updateDancerDto.Email, id))
        {
            return BadRequest(ApiResponse<DancerDto>.CreateFailure("A dancer with this email already exists"));
        }

        // Update properties if provided
        if (updateDancerDto.FirstName != null)
            dancer.FirstName = updateDancerDto.FirstName;
        
        if (updateDancerDto.LastName != null)
            dancer.LastName = updateDancerDto.LastName;
        
        if (updateDancerDto.Email != null)
            dancer.Email = updateDancerDto.Email;
        
        if (updateDancerDto.PhoneNumber != null)
            dancer.PhoneNumber = updateDancerDto.PhoneNumber;
        
        if (updateDancerDto.DateOfBirth.HasValue)
            dancer.DateOfBirth = updateDancerDto.DateOfBirth;
        
        if (updateDancerDto.Gender.HasValue)
            dancer.Gender = updateDancerDto.Gender.Value;
        
        if (updateDancerDto.HeightCm.HasValue)
            dancer.HeightCm = updateDancerDto.HeightCm;
        
        if (updateDancerDto.WeightKg.HasValue)
            dancer.WeightKg = updateDancerDto.WeightKg;
        
        if (updateDancerDto.ExperienceLevel.HasValue)
            dancer.ExperienceLevel = updateDancerDto.ExperienceLevel.Value;
        
        if (updateDancerDto.EmergencyContactName != null)
            dancer.EmergencyContactName = updateDancerDto.EmergencyContactName;
        
        if (updateDancerDto.EmergencyContactPhone != null)
            dancer.EmergencyContactPhone = updateDancerDto.EmergencyContactPhone;
        
        if (updateDancerDto.MedicalNotes != null)
            dancer.MedicalNotes = updateDancerDto.MedicalNotes;
        
        if (updateDancerDto.Notes != null)
            dancer.Notes = updateDancerDto.Notes;
        
        if (updateDancerDto.IsActive.HasValue)
            dancer.IsActive = updateDancerDto.IsActive.Value;

        dancer.LastModifiedAt = DateTime.UtcNow;
        dancer.LastModifiedBy = User.Identity?.Name ?? "system";

        await _dancerRepository.UpdateAsync(dancer);

        _logger.LogInformation("Dancer updated: {DancerId} - {FullName}", dancer.Id, dancer.FullName);

        var dancerDto = MapToDancerDto(dancer);
        return Ok(ApiResponse<DancerDto>.CreateSuccess(dancerDto, "Dancer updated successfully"));
    }

    /// <summary>
    /// Delete a dancer
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteDancer(Guid id)
    {
        var dancer = await _dancerRepository.GetByIdAsync(id);
        if (dancer == null)
        {
            return NotFound(ApiResponse<bool>.CreateFailure($"Dancer with ID {id} not found"));
        }

        await _dancerRepository.DeleteAsync(dancer);

        _logger.LogInformation("Dancer deleted: {DancerId} - {FullName}", dancer.Id, dancer.FullName);

        return Ok(ApiResponse<bool>.CreateSuccess(true, "Dancer deleted successfully"));
    }

    /// <summary>
    /// Map a Dancer entity to a DancerDto
    /// </summary>
    private static DancerDto MapToDancerDto(Models.Dancer dancer)
    {
        return new DancerDto
        {
            Id = dancer.Id,
            FirstName = dancer.FirstName,
            LastName = dancer.LastName,
            FullName = dancer.FullName,
            Email = dancer.Email,
            PhoneNumber = dancer.PhoneNumber,
            DateOfBirth = dancer.DateOfBirth,
            Age = dancer.Age,
            Gender = dancer.Gender,
            HeightCm = dancer.HeightCm,
            WeightKg = dancer.WeightKg,
            ExperienceLevel = dancer.ExperienceLevel,
            EmergencyContactName = dancer.EmergencyContactName,
            EmergencyContactPhone = dancer.EmergencyContactPhone,
            JoinedDate = dancer.JoinedDate,
            IsActive = dancer.IsActive,
            CreatedAt = dancer.CreatedAt,
            LastModifiedAt = dancer.LastModifiedAt,
            DanceStyles = dancer.DanceStyles?.Select(ds => new DancerStyleDto
            {
                Style = ds.Style,
                ProficiencyLevel = ds.ProficiencyLevel,
                YearsOfExperience = ds.YearsOfExperience,
                Notes = ds.Notes
            }).ToList() ?? new List<DancerStyleDto>()
        };
    }
}