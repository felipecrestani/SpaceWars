using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceWars
{
    public class Meteor : IDisposable
    {
        public Texture2D Texture { get; set; }
        public Rectangle Person;
        public Direction Direction { get; set; }
        public int velocity = 5;
        public int angle;

        public Meteor(Texture2D texture)
        {
            Texture = texture;
            Random rand = new Random();
            var size = rand.Next(32, 100);
            this.Person = new Rectangle(rand.Next(0,960),0, size, size);
            angle = rand.Next(0, 3);
            velocity = rand.Next(1, 10);
        }

        public void Update()
        {
            Person.Y += velocity;
            Person.X += angle;
        }     

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Person, Color.White);
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Dispose()
        {
            Dispose();
        }
    }
}
