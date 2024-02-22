using Eto;
using Eto.Forms;
using FMOD;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using Keyboard = Microsoft.Xna.Framework.Input.Keyboard;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Mouse = Microsoft.Xna.Framework.Input.Mouse;


namespace MuGen.Source;
public class MuGen : Game
{
    private GraphicsDeviceManager _graphics;
    private MouseState _oldState;
    private Entity2D _folder;
    private FileExplorer _fileExplorer;
    private Application _app;

    public MuGen()
    {
        _graphics = new GraphicsDeviceManager(this);
        _folder = new Entity2D();
        Platform platform = Platform.Detect;
        _app = new Application(platform);
        _fileExplorer = new FileExplorer();

        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferHeight = 720;
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.ApplyChanges();

        Globals.GraphicsDevice = GraphicsDevice;
        Globals.SpriteBatch = new SpriteBatch(GraphicsDevice);

        SongList.Initialize();
        Audio.Initialize();
        base.Initialize();
    }

    protected override void LoadContent()
    {

        _folder.texture = Content.Load<Texture2D>("folder-icon");
        _folder.onScreen = new Rectangle(GraphicsDevice.Viewport.Width - 80, GraphicsDevice.Viewport.Height - 80, 70, 70);

        SongNode.bgTexture = Content.Load<Texture2D>("song-bg");
        SongNode.font = Content.Load<SpriteFont>("textFont");
    }

    protected override void Update(GameTime gameTime)
    {
        MouseState _mouseState = Mouse.GetState();
        KeyboardState _keyboardState = Keyboard.GetState();
        if (_keyboardState.IsKeyDown(Keys.Escape))
            Exit();

        if (_mouseState.LeftButton == ButtonState.Pressed && _oldState.LeftButton == ButtonState.Released)
        {
            if (_folder.checkPressed(new Vector2(_mouseState.X, _mouseState.Y)))
            {
                _fileExplorer.Open();
            }
        }
        _oldState = _mouseState;

        if (_fileExplorer.folderSelected)
        {
            SongList.Load(_fileExplorer.filePaths);
            _fileExplorer.folderSelected = false;
        }

        SongList.Update();
        Audio.Update();
        Globals.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);

        GraphicsDevice.Clear(Color.Silver);

        Globals.SpriteBatch.Begin();
        Globals.SpriteBatch.Draw(_folder.texture, _folder.onScreen, _folder.texture.Bounds, Color.White);
        SongList.DrawBackground();
        Globals.SpriteBatch.End();

        SongList.DrawText();
    }

}
