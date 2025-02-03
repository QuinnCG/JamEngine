namespace Engine;

/// <summary>
/// An <see cref="Entity"/> that can be registered with <see cref="Application.RegisterGlobal(GlobalEntity)"/> to have an <see cref="Entity"/> whose lifespan is in sync with the <see cref="Application"/>'s.
/// </summary>
public abstract class GlobalEntity : Entity { }
