using DanceChoreographyManager.Services.Course.DTOs;
using DanceChoreographyManager.Services.Course.Models;
using DanceChoreographyManager.Services.Course.Repositories;
using DanceChoreographyManager.Shared.DTOs.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DanceChoreographyManager.Services.Course.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CourseController : ControllerBase
{
    private readonly ICourseRepository _courseRepository;
    private readonly ILogger<CourseController> _logger;

    public CourseController(
        ICourseRepository courseRepository,
        ILogger<CourseController> logger)
    {
        _courseRepository = courseRepository;
        _logger = logger;
    }

    /// <summary>
    /// Get all courses with filtering and pagination
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<PagedResponse<CourseDto>>> GetCourses([FromQuery] CourseFilterParams filterParams)
    {
        var (courses, totalCount) = await _courseRepository.GetFilteredAsync(filterParams);

        var courseDtos = courses.Select(course => MapToCourseDto(course)).ToList();

        var response = new PagedResponse<CourseDto>
        {
            Page = filterParams.Page,
            PageSize = filterParams.PageSize,
            TotalCount = totalCount,
            Items = courseDtos
        };

        return Ok(response);
    }

    /// <summary>
    /// Get a course by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<CourseDto>>> GetCourse(Guid id)
    {
        var course = await _courseRepository.GetByIdWithDetailsAsync(id);
        if (course == null)
        {
            return NotFound(ApiResponse<CourseDto>.CreateFailure($"Course with ID {id} not found"));
        }

        var courseDto = MapToCourseDto(course);
        return Ok(ApiResponse<CourseDto>.CreateSuccess(courseDto));
    }

    /// <summary>
    /// Create a new course
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<CourseDto>>> CreateCourse(CreateCourseDto createCourseDto)
    {
        // Check if name already exists
        if (await _courseRepository.NameExistsAsync(createCourseDto.Name))
        {
            ModelState.AddModelError("Name", "A course with this name already exists");
            return BadRequest(ApiResponse<CourseDto>.CreateFailure("A course with this name already exists"));
        }

        // Validate start and end dates
        if (createCourseDto.EndDate <= createCourseDto.StartDate)
        {
            ModelState.AddModelError("EndDate", "End date must be after start date");
            return BadRequest(ApiResponse<CourseDto>.CreateFailure("End date must be after start date"));
        }

        var course = new Models.Course
        {
            Id = Guid.NewGuid(),
            Name = createCourseDto.Name,
            Description = createCourseDto.Description,
            DanceStyle = createCourseDto.DanceStyle,
            DifficultyLevel = createCourseDto.DifficultyLevel,
            StartDate = createCourseDto.StartDate,
            EndDate = createCourseDto.EndDate,
            DurationMinutes = createCourseDto.DurationMinutes,
            Capacity = createCourseDto.Capacity,
            EnrollmentCount = 0,
            Location = createCourseDto.Location,
            InstructorId = createCourseDto.InstructorId,
            InstructorName = createCourseDto.InstructorName,
            IsActive = true,
            Price = createCourseDto.Price,
            Currency = createCourseDto.Currency ?? "USD",
            CreatedAt = DateTime.UtcNow,
            CreatedBy = User.Identity?.Name ?? "system"
        };

        // Add sessions if provided
        if (createCourseDto.Sessions != null && createCourseDto.Sessions.Any())
        {
            course.Sessions = createCourseDto.Sessions.Select(s => new CourseSession
            {
                Id = Guid.NewGuid(),
                CourseId = course.Id,
                StartDateTime = s.StartDateTime,
                EndDateTime = s.EndDateTime,
                Location = s.Location ?? course.Location,
                Notes = s.Notes,
                IsCanceled = false,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = User.Identity?.Name ?? "system"
            }).ToList();
        }

        await _courseRepository.AddAsync(course);

        _logger.LogInformation("Course created: {CourseId} - {Name}", course.Id, course.Name);

        var courseDto = MapToCourseDto(course);
        return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, 
            ApiResponse<CourseDto>.CreateSuccess(courseDto, "Course created successfully"));
    }

    /// <summary>
    /// Update an existing course
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<CourseDto>>> UpdateCourse(Guid id, UpdateCourseDto updateCourseDto)
    {
        var course = await _courseRepository.GetByIdWithDetailsAsync(id);
        if (course == null)
        {
            return NotFound(ApiResponse<CourseDto>.CreateFailure($"Course with ID {id} not found"));
        }

        // Check if name already exists
        if (updateCourseDto.Name != null && updateCourseDto.Name != course.Name && 
            await _courseRepository.NameExistsAsync(updateCourseDto.Name, id))
        {
            return BadRequest(ApiResponse<CourseDto>.CreateFailure("A course with this name already exists"));
        }

        // Validate start and end dates if both are provided
        if (updateCourseDto.StartDate.HasValue && updateCourseDto.EndDate.HasValue && 
            updateCourseDto.EndDate.Value <= updateCourseDto.StartDate.Value)
        {
            return BadRequest(ApiResponse<CourseDto>.CreateFailure("End date must be after start date"));
        }

        // Validate start date if only that is provided
        if (updateCourseDto.StartDate.HasValue && !updateCourseDto.EndDate.HasValue && 
            updateCourseDto.StartDate.Value >= course.EndDate)
        {
            return BadRequest(ApiResponse<CourseDto>.CreateFailure("Start date must be before end date"));
        }

        // Validate end date if only that is provided
        if (!updateCourseDto.StartDate.HasValue && updateCourseDto.EndDate.HasValue && 
            updateCourseDto.EndDate.Value <= course.StartDate)
        {
            return BadRequest(ApiResponse<CourseDto>.CreateFailure("End date must be after start date"));
        }

        // Update properties if provided
        if (updateCourseDto.Name != null)
            course.Name = updateCourseDto.Name;
        
        if (updateCourseDto.Description != null)
            course.Description = updateCourseDto.Description;
        
        if (updateCourseDto.DanceStyle.HasValue)
            course.DanceStyle = updateCourseDto.DanceStyle.Value;
        
        if (updateCourseDto.DifficultyLevel.HasValue)
            course.DifficultyLevel = updateCourseDto.DifficultyLevel.Value;
        
        if (updateCourseDto.StartDate.HasValue)
            course.StartDate = updateCourseDto.StartDate.Value;
        
        if (updateCourseDto.EndDate.HasValue)
            course.EndDate = updateCourseDto.EndDate.Value;
        
        if (updateCourseDto.DurationMinutes.HasValue)
            course.DurationMinutes = updateCourseDto.DurationMinutes.Value;
        
        if (updateCourseDto.Capacity.HasValue)
            course.Capacity = updateCourseDto.Capacity.Value;
        
        if (updateCourseDto.Location != null)
            course.Location = updateCourseDto.Location;
        
        if (updateCourseDto.InstructorId.HasValue)
            course.InstructorId = updateCourseDto.InstructorId;
        
        if (updateCourseDto.InstructorName != null)
            course.InstructorName = updateCourseDto.InstructorName;
        
        if (updateCourseDto.IsActive.HasValue)
            course.IsActive = updateCourseDto.IsActive.Value;
        
        if (updateCourseDto.Price.HasValue)
            course.Price = updateCourseDto.Price.Value;
        
        if (updateCourseDto.Currency != null)
            course.Currency = updateCourseDto.Currency;

        course.LastModifiedAt = DateTime.UtcNow;
        course.LastModifiedBy = User.Identity?.Name ?? "system";

        await _courseRepository.UpdateAsync(course);

        _logger.LogInformation("Course updated: {CourseId} - {Name}", course.Id, course.Name);

        var courseDto = MapToCourseDto(course);
        return Ok(ApiResponse<CourseDto>.CreateSuccess(courseDto, "Course updated successfully"));
    }

    /// <summary>
    /// Delete a course
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteCourse(Guid id)
    {
        var course = await _courseRepository.GetByIdAsync(id);
        if (course == null)
        {
            return NotFound(ApiResponse<bool>.CreateFailure($"Course with ID {id} not found"));
        }

        await _courseRepository.DeleteAsync(course);

        _logger.LogInformation("Course deleted: {CourseId} - {Name}", course.Id, course.Name);

        return Ok(ApiResponse<bool>.CreateSuccess(true, "Course deleted successfully"));
    }

    /// <summary>
    /// Map a Course entity to a CourseDto
    /// </summary>
    private static CourseDto MapToCourseDto(Models.Course course)
    {
        return new CourseDto
        {
            Id = course.Id,
            Name = course.Name,
            Description = course.Description,
            DanceStyle = course.DanceStyle,
            DifficultyLevel = course.DifficultyLevel,
            StartDate = course.StartDate,
            EndDate = course.EndDate,
            DurationMinutes = course.DurationMinutes,
            Capacity = course.Capacity,
            EnrollmentCount = course.EnrollmentCount,
            Location = course.Location,
            InstructorId = course.InstructorId,
            InstructorName = course.InstructorName,
            IsActive = course.IsActive,
            Price = course.Price,
            Currency = course.Currency,
            IsFull = course.IsFull,
            HasStarted = course.HasStarted,
            HasEnded = course.HasEnded,
            CreatedAt = course.CreatedAt,
            LastModifiedAt = course.LastModifiedAt,
            Sessions = course.Sessions?.Select(s => new SessionDto
            {
                Id = s.Id,
                CourseId = s.CourseId,
                StartDateTime = s.StartDateTime,
                EndDateTime = s.EndDateTime,
                Location = s.Location,
                Notes = s.Notes,
                IsCanceled = s.IsCanceled,
                CancellationReason = s.CancellationReason,
                DurationMinutes = s.DurationMinutes,
                HasStarted = s.HasStarted,
                HasEnded = s.HasEnded
            }).ToList() ?? new List<SessionDto>()
        };
    }
}