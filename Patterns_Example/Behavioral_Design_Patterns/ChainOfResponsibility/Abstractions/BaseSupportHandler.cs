using Patterns_Example.Behavioral_Design_Patterns.ChainOfResponsibility.Models;
using Patterns_Example.Behavioral_Design_Patterns.ChainOfResponsibility.Models.Enums;

namespace Patterns_Example.Behavioral_Design_Patterns.ChainOfResponsibility.Abstractions;

/// <summary>
/// Base abstract handler that implements the chain mechanism.
/// </summary>
public abstract class BaseSupportHandler : ISupportHandler
{
    private ISupportHandler? _nextHandler;
    protected readonly HandlerInfo _handlerInfo;

    protected BaseSupportHandler(HandlerInfo handlerInfo)
    {
        _handlerInfo = handlerInfo;
    }

    public virtual ISupportHandler SetNext(ISupportHandler nextHandler)
    {
        _nextHandler = nextHandler;
        return nextHandler;
    }

    public virtual SupportResponse Handle(SupportRequest request)
    {
        var startTime = DateTime.Now;
        
        // Add event for handler processing
        request.AddEvent("HANDLER_PROCESSING", $"Processing by {_handlerInfo.Name}", _handlerInfo.Name);
        
        // Check if this handler can process the request
        if (CanHandle(request))
        {
            var response = ProcessRequest(request);
            response.ProcessingTime = DateTime.Now - startTime;
            response.HandlerName = _handlerInfo.Name;
            
            if (response.IsHandled)
            {
                request.AddEvent("HANDLED", $"Successfully handled by {_handlerInfo.Name}", _handlerInfo.Name);
                return response;
            }
        }
        
        // Add event for passing to next handler
        request.AddEvent("PASSED_TO_NEXT", $"Passed from {_handlerInfo.Name} to next handler", _handlerInfo.Name);
        
        // Pass to next handler if available
        if (_nextHandler != null)
        {
            return _nextHandler.Handle(request);
        }
        
        // No handler could process the request
        request.AddEvent("UNHANDLED", "No handler in chain could process this request", "CHAIN_END");
        return new SupportResponse
        {
            IsHandled = false,
            HandlerName = "UNHANDLED",
            Message = "No appropriate handler found in the chain",
            ProcessingTime = DateTime.Now - startTime
        };
    }

    public HandlerInfo GetHandlerInfo()
    {
        return _handlerInfo;
    }

    /// <summary>
    /// Determines if this handler can process the given request.
    /// </summary>
    protected abstract bool CanHandle(SupportRequest request);
    
    /// <summary>
    /// Processes the request and returns a response.
    /// </summary>
    protected abstract SupportResponse ProcessRequest(SupportRequest request);
    
    /// <summary>
    /// Helper method to check if handler supports the request category.
    /// </summary>
    protected bool SupportsCategory(SupportCategory category)
    {
        return _handlerInfo.HandledCategories.Contains(category);
    }
    
    /// <summary>
    /// Helper method to check if handler supports the request priority.
    /// </summary>
    protected bool SupportsPriority(Priority priority)
    {
        return _handlerInfo.HandledPriorities.Contains(priority);
    }
    
    /// <summary>
    /// Helper method to check if handler supports the customer tier.
    /// </summary>
    protected bool SupportsTier(CustomerTier tier)
    {
        return _handlerInfo.HandledTiers.Contains(tier);
    }
}