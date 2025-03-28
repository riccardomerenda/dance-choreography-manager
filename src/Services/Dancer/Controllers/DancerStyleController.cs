using DanceChoreographyManager.Services.Dancer.Data;
using DanceChoreographyManager.Services.Dancer.DTOs;
using DanceChoreographyManager.Services.Dancer.Models;
using DanceChoreographyManager.Services.Dancer.Repositories;
using DanceChoreographyManager.Shared.DTOs.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DanceChoreographyManager.Services.Dancer.Controllers;

[ApiController]
[Route("api/dancer/{dancerId}/styles")]
[Authorize]
public class DancerStyleController : ControllerBase
{
    private readonly IDancerRepository _dancerRepository;
    private readonly DancerDbContext _context;
    private readonly ILogger<DancerStyleController> _logger;

    public DancerStyleController(
        IDancerRepository dancerRepository,
        DancerDbContext context,
        ILogger<DancerStyleController> logger)
    {
        _dancerRepository = dancerRepository;
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get all dance styles for a dancer
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<DancerStyleDto>>>> GetDancerStyles(Guid dancerId)
    {
        var dancer = await _dancerRepository.GetByIdWithDetailsAsync(dancerId);
        if (dancer == null)
        {
            return NotFound(ApiResponse<List<DancerStyleDto>>.CreateFailure($"Dancer with ID {dancerId} not found"));
        }

        var styles = dancer.DanceStyles.Select(ds => new DancerStyleDto
        {
            Style = ds.Style,
            ProficiencyLevel = ds.ProficiencyLevel,
            YearsOfExperience = ds.YearsOfExperience,
            Notes = ds.Notes
        }).ToList();

        return Ok(ApiResponse<List<DancerStyleDto>>.CreateSuccess(styles));
    }

    /// <summary>
    /// Add a dance style to a dancer
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<DancerStyleDto>>> AddDancerStyle(Guid dancerId, DancerStyleDto styleDto)
    {
        var dancer = await _dancerRepository.GetByIdWithDetailsAsync(dancerId);
        if (dancer == null)
        {
            return NotFound(ApiResponse<DancerStyleDto>.CreateFailure($"Dancer with ID {dancerId} not found"));
        }

        // Check if the style already exists for this dancer
        if (dancer.DanceStyles.Any(ds => ds.Style == styleDto.Style))
        {
            return BadRequest(ApiResponse<DancerStyleDto>.CreateFailure($"Dance style {styleDto.Style} already exists for this dancer"));
        }

        var dancerStyle = new DancerStyle
        {
            Id = Guid.NewGuid(),
            DancerId = dancerId,
            Style = styleDto.Style,
            ProficiencyLevel = styleDto.ProficiencyLevel,
            YearsOfExperience = styleDto.YearsOfExperience,
            Notes = styleDto.Notes,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = User.Identity?.Name ?? "system"
        };

        _context.DancerStyles.Add(dancerStyle);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Dance style {Style} added to dancer {DancerId}", styleDto.Style, dancerId);

        return CreatedAtAction(nameof(GetDancerStyles), new { dancerId = dancerId }, 
            ApiResponse<DancerStyleDto>.CreateSuccess(styleDto, "Dance style added successfully"));
    }

    /// <summary>
    /// Update a dance style for a dancer
    /// </summary>
    [HttpPut("{style}")]
    public async Task<ActionResult<ApiResponse<DancerStyleDto>>> UpdateDancerStyle(
        Guid dancerId, DanceStyle style, DancerStyleDto styleDto)
    {
        var dancer = await _dancerRepository.GetByIdWithDetailsAsync(dancerId);
        if (dancer == null)
        {
            return NotFound(ApiResponse<DancerStyleDto>.CreateFailure($"Dancer with ID {dancerId} not found"));
        }

        var dancerStyle = dancer.DanceStyles.FirstOrDefault(ds => ds.Style == style);
        if (dancerStyle == null)
        {
            return NotFound(ApiResponse<DancerStyleDto>.CreateFailure($"Dance style {style} not found for this dancer"));
        }

        // Update the style
        dancerStyle.ProficiencyLevel = styleDto.ProficiencyLevel;
        dancerStyle.YearsOfExperience = styleDto.YearsOfExperience;
        dancerStyle.Notes = styleDto.Notes;
        dancerStyle.LastModifiedAt = DateTime.UtcNow;
        dancerStyle.LastModifiedBy = User.Identity?.Name ?? "system";

        await _context.SaveChangesAsync();

        _logger.LogInformation("Dance style {Style} updated for dancer {DancerId}", style, dancerId);

        return Ok(ApiResponse<DancerStyleDto>.CreateSuccess(styleDto, "Dance style updated successfully"));
    }

    /// <summary>
    /// Remove a dance style from a dancer
    /// </summary>
    [HttpDelete("{style}")]
    public async Task<ActionResult<ApiResponse<bool>>> RemoveDancerStyle(Guid dancerId, DanceStyle style)
    {
        var dancer = await _dancerRepository.GetByIdWithDetailsAsync(dancerId);
        if (dancer == null)
        {
            return NotFound(ApiResponse<bool>.CreateFailure($"Dancer with ID {dancerId} not found"));
        }

        var dancerStyle = dancer.DanceStyles.FirstOrDefault(ds => ds.Style == style);
        if (dancerStyle == null)
        {
            return NotFound(ApiResponse<bool>.CreateFailure($"Dance style {style} not found for this dancer"));
        }

        _context.DancerStyles.Remove(dancerStyle);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Dance style {Style} removed from dancer {DancerId}", style, dancerId);

        return Ok(ApiResponse<bool>.CreateSuccess(true, "Dance style removed successfully"));
    }
}