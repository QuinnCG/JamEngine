using Engine;

namespace Sandbox;

static class Sandbox
{
	static void Main()
	{
		var res = Resource.Load<BinaryResource>("README.md");
		Console.WriteLine(System.Text.Encoding.Default.GetString(res.Data));

		Application.Run();
	}
}
