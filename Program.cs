using System;

static class Program
{

	[STAThread]
	static void Main(string[] args)
	{
		MuGen.MuGen game = new MuGen.MuGen();
		game.Run();
	}
}

