using GameFire.MapPlay;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace GameFire
{
    public class Game1 : Game
    {
        #region Properties
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Texture2D bacground;
        private Rectangle boxBackGround;
        private Vector2 index;
        private GameUI gameUI;
        private GamePlay gamePlay;
        private float timeColect;
        private bool isFullHD;
        #endregion

        #region Constructor
        public Game1()
        {
            LoadUI();
        }
        protected override void Initialize()
        {
            index.X = graphics.PreferredBackBufferWidth / 100.0f;
            index.Y = graphics.PreferredBackBufferHeight / 100.0f;
            base.Initialize();
        }
        #endregion

        #region Load & unload
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            bacground = this.Content.Load<Texture2D>("background/background");
            gameUI = new GameUI(Content, index, isFullHD);
            gamePlay = new GamePlay(Content, index, boxBackGround);
            //ship = new Ship(Content, Vector2.Zero, index);
        }

        protected override void UnloadContent()
        {
            graphics.Dispose();
            spriteBatch.Dispose();
            bacground.Dispose();
            gameUI = null;
            gamePlay = null;
            GC.Collect();
        }
        #endregion

        #region Method
        protected override void Update(GameTime gameTime)
        {
            timeColect += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timeColect >= 15000.0f)
            {
                timeColect = 0.0f;
                GC.Collect();
            }
            if (gameUI.Visible)
            {
                gameUI.Update(gameTime);
                this.IsMouseVisible = gameUI.Visible;
            }
            else
            {
                if (gamePlay.Visible)
                {
                    gamePlay.Update(gameTime);
                    boxBackGround = gamePlay.Screen;
                    if (gamePlay.IsEndGame && IsMouseVisible == false)
                    {
                        this.IsMouseVisible = true;
                    }
                }
                else
                {
                    gameUI.Visible = true;
                    gamePlay.Recycle();
                }
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
                gameUI.Draw(spriteBatch);
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
            if (width > 1366)
            {
                graphics.PreferredBackBufferWidth = (int)(width * 0.325);
                graphics.PreferredBackBufferHeight = (int)(height * 0.9);
                isFullHD = true;
            }
            else
            {
                isFullHD = false;
                graphics.PreferredBackBufferWidth = (int)(width * 0.325);
                graphics.PreferredBackBufferHeight = (int)(height * 0.9);
            }

            Window.Position = new Point((width / 2) - (graphics.PreferredBackBufferWidth / 2), (height / 2) - (graphics.PreferredBackBufferHeight / 2));
            boxBackGround = new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            this.TargetElapsedTime = new System.TimeSpan(0, 0, 0, 0, 33);
            this.IsMouseVisible = true;
        }
        #endregion
    }
}
