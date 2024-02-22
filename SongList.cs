using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MuGen;
public static class SongList
{
	public static List<SongNode> Songs = new List<SongNode>();
	public static SpriteBatch TextSpriteBatch = new SpriteBatch(Globals.GraphicsDevice);
	public static RasterizerState TextRasterizer = new RasterizerState() { ScissorTestEnable = true };
	public static int LeftPadding = 10;
	public static int TopPadding = 25;
	public static int Width = 450;
	public static int Height = 35;
	public static SongNode selectedSong = null;

	public static void Initialize()
	{
		TextSpriteBatch.GraphicsDevice.ScissorRectangle = new Rectangle(LeftPadding, TopPadding, Width - LeftPadding, Globals.GraphicsDevice.Viewport.Height);
	}

	public static void Load(string[] filePaths)
	{
		for (int i  = 0; i <  filePaths.Length; i++) 
		{
			SongNode songNode = new SongNode();
			int cutStr = filePaths[i].LastIndexOf('\\') + 1;
			int length = filePaths[i].LastIndexOf(".mp3");
			songNode.name = filePaths[i].Substring(cutStr, length - cutStr);
			songNode.Load(filePaths[i], i);
			Songs.Add(songNode);
		}
	}

	public static void Update()
	{
        foreach (SongNode song in Songs)
        {
			song.Update();
        }
    }

	public static void DrawBackground()
	{
		foreach (SongNode song in Songs)
		{
			song.DrawBackground();
		}
	}

	public static void DrawText()
	{
		TextSpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, TextRasterizer);
		foreach(SongNode song in Songs)
		{
			song.DrawText();
		}
		TextSpriteBatch.End();
	}

	public static void SetSound(SongNode song)
	{
        if (selectedSong != null)
        {
            if(selectedSong == song)
			{
				song.Stop();
				selectedSong = null;
			} else
			{
				selectedSong.Stop();
				song.Start();
				selectedSong = song;
			}
        } else
		{
			selectedSong = song;
			song.Start();
		}
    }
}
