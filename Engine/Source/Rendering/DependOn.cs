namespace Engine.Rendering;

[AttributeUsage(AttributeTargets.Class)]
public class DependOn<T> : Attribute
{
	// TODO: Implement.
	// Consider if the data needs to be saved or if 'T' can be accessed directly from reflection without need for saving it as a field.
}
