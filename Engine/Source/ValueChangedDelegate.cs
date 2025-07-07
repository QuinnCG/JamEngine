namespace Engine;

/// <summary>
/// Reprents a callback for when a value is changed.
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
/// <param name="oldValue">The value before the change.</param>
/// <param name="newValue">The value after the change.</param>
public delegate void ValueChange<T>(T oldValue, T newValue);
