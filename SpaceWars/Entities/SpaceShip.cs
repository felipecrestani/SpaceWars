using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceWars
{
    public class SpaceShip
    {
        public Texture2D Texture { get; set; }
        public Rectangle Person;
        public const int velocity = 6;

        public SpaceShip(Texture2D texture)
        {
            this.Texture = texture;
            this.Person = new Rectangle(300, 400, 100, 100);
        }

        public void Update(Direction direction)
        {           

            if(direction == Direction.Up)
            {
                Person.Y -= velocity;
                direction = Direction.Up;
            }

            if (direction == Direction.Down)
            {
                Person.Y += velocity;
                direction = Direction.Down;
            }

            if (direction == Direction.Left)
            {
                Person.X -= velocity;
                direction = Direction.Left;
            }

            if (direction == Direction.Right)
            {
                Person.X += velocity;
                direction = Direction.Right;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Person, Color.White);
        }

    }
}
