using Patterns_Example.Behavioral_Design_Patterns.ChainOfResponsibility.Models;
using Patterns_Example.Behavioral_Design_Patterns.ChainOfResponsibility.Models.Enums;

namespace Patterns_Example.Behavioral_Design_Patterns.ChainOfResponsibility;

/// <summary>
/// Builder class for creating support requests with fluent interface.
/// </summary>
public class SupportRequestBuilder
{
    private readonly SupportRequest _request = new();

    public static SupportRequestBuilder Create() => new();

    public SupportRequestBuilder ForCustomer(string name, string email, CustomerTier tier)
    {
        _request.CustomerName = name;
        _request.CustomerEmail = email;
        _request.CustomerTier = tier;
        return this;
    }

    public SupportRequestBuilder WithCategory(SupportCategory category)
    {
        _request.Category = category;
        return this;
    }

    public SupportRequestBuilder WithPriority(Priority priority)
    {
        _request.Priority = priority;
        return this;
    }

    public SupportRequestBuilder WithSubject(string subject)
    {
        _request.Subject = subject;
        return this;
    }

    public SupportRequestBuilder WithDescription(string description)
    {
        _request.Description = description;
        return this;
    }

    public SupportRequestBuilder WithTags(params string[] tags)
    {
        _request.Tags.AddRange(tags);
        return this;
    }

    public SupportRequestBuilder WithMetadata(string key, object value)
    {
        _request.Metadata[key] = value;
        return this;
    }

    public SupportRequest Build()
    {
        // Auto-generate tags if none provided
        if (!_request.Tags.Any())
        {
            _request.Tags = new List<string> 
            { 
                _request.Category.ToString().ToLower(), 
                _request.Priority.ToString().ToLower() 
            };
        }

        return _request;
    }
}