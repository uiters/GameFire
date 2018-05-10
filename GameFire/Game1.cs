using GameFire.bullet;
using GameFire.MapPlay;
using GameFire.Enemy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace GameFire
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Texture2D bacground;
        private Rectangle boxBackGround;
        private Vector2 index;
        private GameUI gameUI;
        private GamePlay gamePlay;
        private float timeColect;
        public Game1()
        {
            LoadUI();
        }


        protected override void Initialize()
        {
            index.X = graphics.PreferredBackBufferWidth / 100;
            index.Y = graphics.PreferredBackBufferHeight / 100;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            bacground = this.Content.Load<Texture2D>("background/background");
            gameUI = new GameUI(Content, index);
            gamePlay = new GamePlay(Content, index, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));
            //ship = new Ship(Content, Vector2.Zero, index);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            timeColect += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if(timeColect >= 30000.0f)
            {
                timeColect = 0.0f;
                GC.Collect();
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (gameUI.Visible == true)
            {
                gameUI.Update(gameTime);
                this.IsMouseVisible = gameUI.Visible;
            }
            else
            {
                gamePlay.Update(gameTime);
            }

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack);
            spriteBatch.Draw(bacground, boxBackGround, Color.White);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            if (gameUI.Visible == true)
                gameUI.Draw(spriteBatch, Color.White);
            else
            {
                gamePlay.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
        void LoadUI()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.Title = "GameFire";
            IsFixedTimeStep = true;
            graphics.SynchronizeWithVerticalRetrace = false;
            int width = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            int height = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            //graphics.PreferredBackBufferWidth = 1920;
            //graphics.PreferredBackBufferHeight = 1080;
            if(width > 1024)
            {
                graphics.PreferredBackBufferWidth = (int)(width * 0.325);
                graphics.PreferredBackBufferHeight = (int)(height * 0.9);
            }
            else
            {
                graphics.PreferredBackBufferWidth = (int)(width * 0.5);
                graphics.PreferredBackBufferHeight = (int)(height * 0.875);
            }

            Window.Position = new Point((width / 2) - (graphics.PreferredBackBufferWidth / 2), (height / 2) - (graphics.PreferredBackBufferHeight / 2));
            //boxBackGround = new Rectangle(0, 0, 800, 900);
            //graphics.IsFullScreen = true;
            boxBackGround = new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            this.TargetElapsedTime = new System.TimeSpan(0, 0, 0, 0, 33);
            this.IsMouseVisible = true;
        }
        
    }
}
