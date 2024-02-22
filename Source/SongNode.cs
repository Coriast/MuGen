using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;

namespace MuGen.Source;
public class SongNode
{
    public static Texture2D bgTexture { get; set; }
    public static SpriteFont font { get; set; }
    public Rectangle onScreen { get; set; }
    public string name { get; set; }
    public Audio audio { get; set; }
    private string filePath;

    private bool _selected = false;
    private bool _fileLoaded = false;
    private bool _mouseHover = false;
    private float _opacity = 0.7f;

    private MouseState _oldState;
    private Vector2 _position, _bufferPosition;

    public SongNode()
    {
        audio = new Audio();
    }

    public void Load(string filePath, int listPosition)
    {
        this.filePath = filePath;
        onScreen = new Rectangle(SongList.LeftPadding, SongList.TopPadding + SongList.Height * listPosition + 5 * listPosition, SongList.Width, SongList.Height);
        _position = new Vector2(onScreen.Left + SongList.LeftPadding, onScreen.Center.Y - font.MeasureString(name).Y / 2.0f);
        _bufferPosition = new Vector2(SongList.Width + font.MeasureString(name).X / 2.0f, _position.Y);
    }

    public void Update()
    {
        if (Mouse.GetState().LeftButton == ButtonState.Pressed && _oldState.LeftButton == ButtonState.Released)
        {
            if (checkMouseCollision(new Vector2(Mouse.GetState().X, Mouse.GetState().Y)))
            {
                if (!_fileLoaded)
                {
                    _fileLoaded = true;
                    audio.Load(filePath);
                }

                SongList.SetSound(this);
            }
        }
        _oldState = Mouse.GetState();

        if (checkMouseCollision(new Vector2(Mouse.GetState().X, Mouse.GetState().Y)))
        {
            _mouseHover = true;
            _opacity = 1.0f;

        }
        else
        {
            _mouseHover = false;
            _opacity = 0.7f;
        }


        if (font.MeasureString(name).X > SongList.Width - SongList.LeftPadding && (_mouseHover || _selected))
        {
            _position -= Vector2.UnitX;
            _bufferPosition -= Vector2.UnitX;
            if (_bufferPosition.X <= onScreen.Left + SongList.LeftPadding)
            {
                _position.X = _bufferPosition.X;
                _bufferPosition.X = SongList.Width - SongList.LeftPadding;
            }
        }
        else
        {
            _position.X = onScreen.Left + SongList.LeftPadding;
            _bufferPosition.X = SongList.Width + font.MeasureString(name).X / 2.0f;
        }
    }

    public void DrawBackground()
    {
        Globals.SpriteBatch.Draw(bgTexture, onScreen, _selected ? Color.Azure : Color.White * _opacity);
    }

    public void DrawText()
    {
        SongList.TextSpriteBatch.DrawString(font, name, _position, Color.Black);

        SongList.TextSpriteBatch.DrawString(font, name, _bufferPosition, Color.Black);
    }

    public bool checkMouseCollision(Vector2 point)
    {
        if (point.X >= onScreen.X && point.Y >= onScreen.Y &&
            point.X <= onScreen.X + onScreen.Width && point.Y <= onScreen.Y + onScreen.Height)
        {
            return true;
        }
        return false;
    }

    public void Start()
    {
        _selected = true;
        audio.Play();
    }

    public void Stop()
    {
        _selected = false;
        audio.Stop();
    }
}
