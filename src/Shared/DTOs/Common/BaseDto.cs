namespace DanceChoreographyManager.Shared.DTOs.Common;

/// <summary>
/// Base data transfer object
/// </summary>
public abstract class BaseDto
{
    /// <summary>
    /// Unique identifier
    /// </summary>
    public Guid Id { get; set; }
}

/// <summary>
/// Base DTO for API responses
/// </summary>
/// <typeparam name="T">Type of response data</typeparam>
public class ApiResponse<T>
{
    /// <summary>
    /// Whether the operation was successful
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Message about the operation
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Response data
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Create a successful response
    /// </summary>
    /// <param name="data">Response data</param>
    /// <param name="message">Success message</param>
    /// <returns>Api response</returns>
    public static ApiResponse<T> CreateSuccess(T data, string? message = null)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data
        };
    }

    /// <summary>
    /// Create a failure response
    /// </summary>
    /// <param name="message">Error message</param>
    /// <returns>Api response</returns>
    public static ApiResponse<T> CreateFailure(string message)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Data = default
        };
    }
}

/// <summary>
/// Pagination response
/// </summary>
/// <typeparam name="T">Type of response items</typeparam>
public class PagedResponse<T>
{
    /// <summary>
    /// Current page number
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Page size
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Total items count
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Total pages
    /// </summary>
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    /// <summary>
    /// Whether there's a next page
    /// </summary>
    public bool HasNextPage => Page < TotalPages;

    /// <summary>
    /// Whether there's a previous page
    /// </summary>
    public bool HasPreviousPage => Page > 1;

    /// <summary>
    /// Items in the page
    /// </summary>
    public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
}