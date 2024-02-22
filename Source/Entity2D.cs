using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MuGen.Source;
public class Entity2D
{
    public Texture2D texture;
    public Rectangle onScreen;

    public bool checkPressed(Vector2 point)
    {
        if (point.X >= onScreen.X && point.Y >= onScreen.Y &&
            point.X <= onScreen.X + onScreen.Width && point.Y <= onScreen.Y + onScreen.Height)
        {
            return true;
        }
        return false;
    }
}
