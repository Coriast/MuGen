using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;

namespace MuGen.Source;
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
        for (int i = 0; i < filePaths.Length; i++)
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

    public static void Draw()
    {
        DrawBackground();

        DrawSpectrum();
    }

    public static void DrawSpectrum()
    {
        int freqDivisor = 102;

        if(selectedSong != null)
        {
            float[] spectrum = selectedSong.GetSpectrum();
            int specIndex = 0;
            int clampSize = (int)spectrum.Length / freqDivisor;

			float[] resizedSpectrum = new float[freqDivisor];
			for (int i = 0; i < resizedSpectrum.Length; i++)
            {
                float value = 0.0f;
                if(spectrum.Length - specIndex < clampSize)
                {
                    for (int rest = specIndex; rest < spectrum.Length; rest++)
                    {
                        value += spectrum[rest];
                        specIndex++;
                    }
                    value = value / clampSize;
                    resizedSpectrum[i] = value;
                } else
                {
                    for (int j = specIndex; j < specIndex + clampSize; j++)
                    {
                        value += spectrum[j];
                    }
                    specIndex += clampSize;
                    value = value / clampSize;
                    resizedSpectrum[i] = value;
                }
            }

			Rectangle position = new Rectangle(900, 150, 2, 1);
			for (int i = 0; i < freqDivisor; i++)
			{
				position.X += 3;
				position.Height = (int)(200 * (0.5/(-Math.Log(resizedSpectrum[i]))));
	
			    Globals.SpriteBatch.Draw(Globals.BasicTexture, position, Color.Black);
				Globals.SpriteBatch.Draw(Globals.BasicTexture, new Vector2(position.X, position.Y), position, Color.Black, ToRadians(180.0f), new Vector2(position.Width, 0.0f), new Vector2(1.0f, 1.0f), SpriteEffects.None, 0f);
			}
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
        foreach (SongNode song in Songs)
        {
            song.DrawText();
        }
        TextSpriteBatch.End();
    }

    public static void SetSound(SongNode song)
    {
        if (selectedSong != null)
        {
            if (selectedSong == song)
            {
                song.Stop();
                selectedSong = null;
            }
            else
            {
                selectedSong.Stop();
                song.Start();
                selectedSong = song;
            }
        }
        else
        {
            selectedSong = song;
            song.Start();
        }
    }

    public static float ToRadians(double degrees)
    {
        return (float)((Math.PI / (double)180.0f) * degrees);
	}
}
