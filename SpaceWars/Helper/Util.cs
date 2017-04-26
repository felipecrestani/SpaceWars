using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceWars
{
    public class Util
    {
        public static Vector2 CenterText(SpriteFont font, GraphicsDeviceManager graphics, string text)
        {
            var center = graphics.GraphicsDevice.Viewport.Bounds.Center.ToVector2();
            var fontMeasure = new Vector2(font.MeasureString(text).X / 2, 0);

            return center - fontMeasure;
        }
    }
}
