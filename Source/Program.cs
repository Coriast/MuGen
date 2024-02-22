using System;

static class Program
{

	[STAThread]
	static void Main(string[] args)
	{
		MuGen.Source.MuGen game = new MuGen.Source.MuGen();
		game.Run();
	}
}

