using GameFire.bullet;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace GameFire
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D bacground;
        Rectangle boxBackGround;
        Vector2 _index;
        GameUI _gameUI;
        Ship _ship;

        public Game1()
        {
            LoadUI();
        }


        protected override void Initialize()
        {
            _index.X = graphics.PreferredBackBufferWidth / 100;
            _index.Y = graphics.PreferredBackBufferHeight / 100;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            bacground = this.Content.Load<Texture2D>("background/background");
            _gameUI = new GameUI(Content, _index);
            _ship = new Ship(Content, Vector2.Zero, _index);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (_gameUI.Visible == true)
            {
                _gameUI.Update(gameTime);
                this.IsMouseVisible = _gameUI.Visible;
            }
            else
            {
                _ship.Update(gameTime);
            }

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack);
            spriteBatch.Draw(bacground, boxBackGround, Color.White);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            if (_gameUI.Visible == true)
                _gameUI.Draw(spriteBatch, Color.White);
            else
                _ship.Draw(gameTime, spriteBatch);
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
            graphics.PreferredBackBufferWidth = (int)(width * 0.325);
            graphics.PreferredBackBufferHeight = (int)(height * 0.9);
            Window.Position = new Point((width / 2) - (graphics.PreferredBackBufferWidth / 2), (height / 2) - (graphics.PreferredBackBufferHeight / 2));
            boxBackGround = new Rectangle(0, 0, width, height);
            //boxBackGround = new Rectangle(0, 0, 800, 900);
            //graphics.IsFullScreen = true;
            this.TargetElapsedTime = new System.TimeSpan(0, 0, 0, 0, 33);
            this.IsMouseVisible = true;
        }
        
    }
}
