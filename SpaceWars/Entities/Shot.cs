using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceWars
{
    public class Shot :IDisposable
    {
        public Texture2D Texture { get; set; }
        public Rectangle Person;
        public Direction Direction { get; set; }
        public const int velocity = 15;

        public Shot(Texture2D texture,int x, int y)
        {
            this.Texture = texture;
            this.Person = new Rectangle(x, y, 6, 12);
        }

        public void Update()
        {
            Person.Y -= velocity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Person, Color.White);
        }

        public void Dispose()
        {
            Dispose();
        }
    }
}
