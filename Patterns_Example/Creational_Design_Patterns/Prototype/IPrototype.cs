namespace Patterns_Example.Creational_Design_Patterns.Prototype;

/// <summary>
/// The Prototype interface declares the cloning methods. In most cases,
/// it's a single "clone" method.
/// </summary>
/// <typeparam name="T">The type of object to clone</typeparam>
public interface IPrototype<T>
{
    /// <summary>
    /// Creates a shallow copy of the current object
    /// </summary>
    /// <returns>A shallow copy of the object</returns>
    T Clone();
    
    /// <summary>
    /// Creates a deep copy of the current object
    /// </summary>
    /// <returns>A deep copy of the object</returns>
    T DeepClone();
}
