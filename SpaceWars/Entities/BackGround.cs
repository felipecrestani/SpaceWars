using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceWars.Interfaces;

namespace SpaceWars.Entities
{
    public class BackGround : IGameEntity
    {
        private Texture2D Texture { get; set; }
        private GraphicsDeviceManager Graphics { get; set; }
        private float backgroundAngle = 0.1f;
        public BackGround(Texture2D texture, GraphicsDeviceManager graphics)
        {
            this.Texture = texture;
            this.Graphics = graphics;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, new Rectangle(500, 400, 2500, 1600), new Rectangle(0, 0, Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight), Color.White, backgroundAngle, new Vector2(500, 400), SpriteEffects.None, 1);
        }

        public void Update(GameTime gameTime)
        {
            backgroundAngle += 0.01f;
        }
    }
}
