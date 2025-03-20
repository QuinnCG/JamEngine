using Engine;

namespace Sandbox;

static class Game
{
	static void Main()
	{
		var res = Resource.Load<BinaryResource>("README.md");
		Console.WriteLine(System.Text.Encoding.Default.GetString(res.Data));

		Application.Run();
	}
}
