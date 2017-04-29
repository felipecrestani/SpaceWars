using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using SpaceWars.Entities;
using SpaceWars.Entities.Controller;
using SpaceWars.Helper;
using System.Linq;

namespace SpaceWars
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpaceShip spaceShip;
        BackGround backGround;
        ShotController shotController;
        List<Meteor> Meteors;
        double timer = 5, lastShot;
        SpriteFont font, smallFont;
        int score;
        SoundEffect Fire_Sound, Theme_Song, meteor_Explosion;
        SoundEffectInstance soundEffectInstance;
        GameState gameState;
        bool isFiring;
        Texture2D backgroundTexture, meteorTexture, spaceshipTexture, shotTexture, meteorExplosionTexture, menuTexture;
        List<AnimatedSprite> MeteorsExplostionList;
        KeyboardState lastKey;
        GamePadState lastButton;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.ToggleFullScreen();
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            shotController = new ShotController();
            Meteors = new List<Meteor>();
            MeteorsExplostionList = new List<AnimatedSprite>();
            gameState = GameState.Menu;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            menuTexture = Content.Load<Texture2D>("menu");

            meteorTexture = Content.Load<Texture2D>("meteor");
            meteorExplosionTexture = Content.Load<Texture2D>("explosion");
            meteor_Explosion = Content.Load<SoundEffect>("meteorexplosion");

            spaceshipTexture = Content.Load<Texture2D>("spaceship");
            spaceShip = new SpaceShip(spaceshipTexture);

            shotTexture = Content.Load<Texture2D>("shot");
            
            font = Content.Load<SpriteFont>("font");
            smallFont = Content.Load<SpriteFont>("SmallFont");
            Fire_Sound = Content.Load<SoundEffect>("laser");
            Theme_Song = Content.Load<SoundEffect>("Tool - Schism");

            backgroundTexture = Content.Load<Texture2D>("background");
            backGround = new BackGround(backgroundTexture,graphics);

            soundEffectInstance = Theme_Song.CreateInstance();
            soundEffectInstance.IsLooped = true;
            soundEffectInstance.Play();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            GamePad(gameTime);
            Keyboard(gameTime);
            backGround.Update(gameTime);

            if (gameState == GameState.Playing || gameState == GameState.GameOver)
            {
                foreach (var shot in shotController.ShotsList)
                {
                    shot.Update();

                    if (shot.Person.Y < 0)
                        shotController.Remove(shot);

                    foreach (var meteor in Meteors)
                    {
                        if (meteor.Person.Intersects(shot.Person))
                        {
                            score++;
                            MeteorsExplostionList.Add(new AnimatedSprite(meteorExplosionTexture, 5, 5, meteor.Location()));
                            meteor.Remove();
                            shotController.Remove(shot);
                            meteor_Explosion.Play();
                        }
                    }
                }

                shotController.DisposeShots();

                var elapsed = gameTime.ElapsedGameTime.TotalSeconds;
                timer -= elapsed;

                if (timer < 0)
                {
                    Meteor meteor = new Meteor(meteorTexture);
                    Meteors.Add(meteor);
                    timer = 1;
                }

                foreach (var meteor in Meteors)
                {
                    meteor.Update();

                    if (meteor.Person.Intersects(spaceShip.Person))
                    {
                        gameState = GameState.GameOver;
                    }
                }

                foreach (var item in MeteorsExplostionList)
                {
                    item.Update();

                    if (item.currentFrame == item.totalFrames - 1)
                        item.isActive = false;
                }

                MeteorsExplostionList.RemoveAll(x => x.isActive == false);

            }
            base.Update(gameTime);
        }

        private void GamePad(GameTime gameTime)
        {
            GamePadCapabilities capabilities = Microsoft.Xna.Framework.Input.GamePad.GetCapabilities(PlayerIndex.One);

            GamePadState state = Microsoft.Xna.Framework.Input.GamePad.GetState(PlayerIndex.One);

            if (capabilities.IsConnected)
            {
                if (capabilities.HasLeftXThumbStick)
                {
                    if (state.ThumbSticks.Left.X < -0.5f)
                    {
                        spaceShip.Update(Direction.Left);
                    }
                    if (state.ThumbSticks.Left.X > 0.5f)
                    {
                        spaceShip.Update(Direction.Right);
                    }
                    if (state.ThumbSticks.Left.Y > 0.5f)
                    {
                        spaceShip.Update(Direction.Up);
                    }
                    if (state.ThumbSticks.Left.Y < -0.5f)
                    {
                        spaceShip.Update(Direction.Down);
                    }

                    if (!isFiring)
                    {
                        if (state.IsButtonDown(Buttons.A))
                        {
                            if (gameTime.ElapsedGameTime.TotalSeconds >= lastShot)
                            {
                                Shot shot = new Shot(shotTexture,spaceShip.Person.Center.X - 2, spaceShip.Person.Y);
                                shotController.Add(shot);
                                Fire_Sound.Play();
                                lastShot = gameTime.ElapsedGameTime.TotalSeconds;
                            }
                        }
                    }

                    if (state.IsButtonUp(Buttons.A))
                    {
                        isFiring = false;
                    }

                    if (state.IsButtonDown(Buttons.Start) && lastButton.IsButtonUp(Buttons.Start))
                    {
                        if (gameState == GameState.GameOver)
                        {
                            Reset();
                        }
                        else
                        {
                            gameState = GameState.Playing;
                        }                        
                    }

                    lastButton = Microsoft.Xna.Framework.Input.GamePad.GetState(PlayerIndex.One);
                }
            }
        }

        private void Keyboard(GameTime gameTime)
        {
            if (Microsoft.Xna.Framework.Input.Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                spaceShip.Update(Direction.Up);
            }

            if (Microsoft.Xna.Framework.Input.Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                spaceShip.Update(Direction.Down);
            }

            if (Microsoft.Xna.Framework.Input.Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                spaceShip.Update(Direction.Left);
            }

            if (Microsoft.Xna.Framework.Input.Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                spaceShip.Update(Direction.Right);
            }

            if (!isFiring)
            {
                if (Microsoft.Xna.Framework.Input.Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    if (gameTime.ElapsedGameTime.Seconds >= lastShot)
                    {
                        Shot shot = new Shot(shotTexture, spaceShip.Person.Center.X - 2, spaceShip.Person.Y);
                        shotController.Add(shot);
                        Fire_Sound.Play();
                        lastShot = gameTime.ElapsedGameTime.Seconds;
                        isFiring = true;
                    }
                }
            }

            if (Microsoft.Xna.Framework.Input.Keyboard.GetState().IsKeyUp(Keys.Space))
            {
                isFiring = false;
            }


            if (Microsoft.Xna.Framework.Input.Keyboard.GetState().IsKeyDown(Keys.Enter)
                && lastKey.IsKeyUp(Keys.Enter))
            {
                if (gameState == GameState.GameOver)
                {                   
                    Reset();
                }
                else
                {
                    gameState = GameState.Playing;
                }
            }

            lastKey = Microsoft.Xna.Framework.Input.Keyboard.GetState();
        }

        private void Reset()
        {
            gameState = GameState.Menu;
            score = 0;
            Meteors.Clear();
            shotController.ShotsList.Clear();
            spaceShip = new SpaceShip(spaceshipTexture);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();            

            if(gameState == GameState.Menu)
            {                
                spriteBatch.Draw(menuTexture, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                spaceShip.Draw(spriteBatch);
            }

            if (gameState == GameState.Playing)
            {
                backGround.Draw(spriteBatch);

                spaceShip.Draw(spriteBatch);

                foreach (var shot in shotController.ShotsList)
                {
                    shot.Draw(spriteBatch);
                }

                foreach (var meteor in Meteors)
                {
                    meteor.Draw(spriteBatch);
                }

                foreach (var item in MeteorsExplostionList)
                {
                    item.Draw(spriteBatch);
                }

                spriteBatch.DrawString(font, "Time: " + gameTime.TotalGameTime.ToString("h'h 'm'm 's's'") + " Score: " + score.ToString(), Vector2.Zero, Color.White);
                //spriteBatch.DrawString(font, "X:" + spaceShip.Person.X + " Y:" + spaceShip.Person.Y, new Vector2(0, 800), Color.White);
            }

            if (gameState == GameState.GameOver)
            {
                spriteBatch.DrawString(font, "Game Over!", Util.CenterText(font,graphics,"Game Over"), Color.White);
                spriteBatch.DrawString(font, "Score: " + score, Util.CenterText(font, graphics, "Score: " + score) - new Vector2(0,100), Color.White);
            }

            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
