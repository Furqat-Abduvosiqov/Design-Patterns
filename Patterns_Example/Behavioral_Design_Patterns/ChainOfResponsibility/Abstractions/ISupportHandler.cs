using Patterns_Example.Behavioral_Design_Patterns.ChainOfResponsibility.Models;

namespace Patterns_Example.Behavioral_Design_Patterns.ChainOfResponsibility.Abstractions;

/// <summary>
/// Handler interface that defines the contract for handling support requests.
/// This is the base interface for all handlers in the chain of responsibility.
/// </summary>
public interface ISupportHandler
{
    /// <summary>
    /// Sets the next handler in the chain.
    /// </summary>
    /// <param name="nextHandler">The next handler to set</param>
    /// <returns>The next handler for method chaining</returns>
    ISupportHandler SetNext(ISupportHandler nextHandler);
    
    /// <summary>
    /// Handles the support request or passes it to the next handler.
    /// </summary>
    /// <param name="request">The support request to handle</param>
    /// <returns>The result of handling the request</returns>
    SupportResponse Handle(SupportRequest request);
    
    /// <summary>
    /// Gets information about this handler.
    /// </summary>
    HandlerInfo GetHandlerInfo();
}
