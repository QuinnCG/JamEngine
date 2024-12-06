namespace Engine;

/// <summary>
/// Mark a <c>Component</c> class as dependant on another <c>Component</c>.
/// </summary>
/// <typeparam name="T">The <c>Component</c> <c>Type</c> this <c>Component</c> depends on to function.</typeparam>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class DependOn<T> : Attribute { }
