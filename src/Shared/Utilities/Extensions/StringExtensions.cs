using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace DanceChoreographyManager.Shared.Utilities.Extensions;

/// <summary>
/// Extensions for string operations
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Converts a string to a URL-friendly slug
    /// </summary>
    /// <param name="text">Text to convert</param>
    /// <returns>URL-friendly slug</returns>
    public static string ToSlug(this string text)
    {
        if (string.IsNullOrEmpty(text))
            return string.Empty;

        // Remove diacritics (accents)
        var normalizedString = text.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

        foreach (var c in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        // Convert to lowercase and replace spaces with hyphens
        var slug = stringBuilder.ToString().Normalize(NormalizationForm.FormC)
            .ToLowerInvariant()
            .Replace(" ", "-");

        // Remove special characters
        slug = Regex.Replace(slug, @"[^a-z0-9\s-]", string.Empty);
        
        // Remove consecutive hyphens
        slug = Regex.Replace(slug, @"-+", "-");
        
        // Trim hyphens from start and end
        slug = slug.Trim('-');

        return slug;
    }

    /// <summary>
    /// Truncates a string to a maximum length and adds an ellipsis if truncated
    /// </summary>
    /// <param name="value">String to truncate</param>
    /// <param name="maxLength">Maximum length</param>
    /// <param name="addEllipsis">Whether to add an ellipsis</param>
    /// <returns>Truncated string</returns>
    public static string Truncate(this string value, int maxLength, bool addEllipsis = true)
    {
        if (string.IsNullOrEmpty(value)) 
            return string.Empty;
        
        if (value.Length <= maxLength) 
            return value;
        
        var truncatedValue = value[..maxLength].TrimEnd();
        
        if (addEllipsis)
            truncatedValue += "...";
        
        return truncatedValue;
    }
    
    /// <summary>
    /// Checks if a string is a valid email address
    /// </summary>
    /// <param name="email">Email string to validate</param>
    /// <returns>True if valid, false otherwise</returns>
    public static bool IsValidEmail(this string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            // Use a simple regex for basic validation
            var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return regex.IsMatch(email);
        }
        catch
        {
            return false;
        }
    }
}