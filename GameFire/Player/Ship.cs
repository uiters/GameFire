using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace GameFire.bullet
{
    public class Ship : GameObject
    {

        #region Properties

        #region Protect Ship
        private float timeApear = 0f;
        private bool isProtect = true;
        private Texture2D textureProtect;
        private float rotationProtect = 0.0f;
        private float scale = 0.0f;
        private bool isMaxBig = false;
        private Vector2 origin;

        private Texture2D textureDie;
        private Rectangle desRectDie;
        private bool isDeading;

        public bool IsProtect { get => isProtect; set => isProtect = value; }
        public int Heart { get => (int)_heart; set => _heart = value; }
        #endregion

        #region Bullet
        private List<Bullet> bullets;
        private float timeDelay = 0.0f;
        private int level;
        public List<Bullet> Bullets { get => bullets; private set => bullets = value; }
        #endregion

        #endregion

        #region Constructor
        public Ship(ContentManager content, Vector2 speed, Vector2 index) : base(content, speed, index, Rectangle.Empty)
        {
            _heart = 30;
            level = 1;
            this.Visible = false;
            desRectDie = new Rectangle();
            Load();
        }
        #endregion

        #region Load & Unload
        protected override void Load()
        {
            _skin = _content.Load<Texture2D>("ship");
            textureProtect = _content.Load<Texture2D>("procted");
            MouseState mouse = Mouse.GetState();
            _desRectSkin = new Rectangle(mouse.X, mouse.Y, 72, 71);
            origin = new Vector2(textureProtect.Width / 2, textureProtect.Height / 2);
            Bullets = new List<Bullet>();
            textureDie = _content.Load<Texture2D>("shipDie");
            desRectDie.Size = new Point(30, 30);
        }
        #endregion

        #region Method

        public override void Update(GameTime gameTime)
        {
            if(Visible == true)
            {
                if (isDeading == true)
                {
                    if(desRectDie.Width < 50 && desRectDie.Height < 50)
                    {
                        desRectDie.Size += new Point(5, 5);
                    }
                    else
                    {
                        this.isDeading = false;
                        this.Visible = false;
                        this.IsProtect = true;
                    }
                }
                else
                if(_isMove == false)
                {
                    this.Apperend(gameTime);
                    UpdateScale();                  
                }
                else
                {
                    if (IsProtect == true)
                        Protect(gameTime);
                    MouseState mouse = Mouse.GetState();
                    KeyboardState keyboard = Keyboard.GetState();
                    if (_desRectSkin.X != mouse.X || _desRectSkin.Y != mouse.Y)
                        _desRectSkin = new Rectangle(mouse.X - 31, mouse.Y - 31, 72, 71);
                    timeDelay += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    this.BulletClick(mouse, keyboard);
                    this.UpdateBullet(gameTime);
                }
            }
            else
            {
                if(_heart > 0)
                {
                    _desRectSkin = new Rectangle((int)_index.X * 45, (int)_index.Y * 110, 72, 71);
                    Visible = true;
                    _isMove = false;
                    --_heart;
                }
            }

        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Color color)
        {
            if (!Visible)
                return;
            if(isDeading == true)
            {
                spriteBatch.Draw(textureDie, desRectDie, color);
            }
            else
            {
                if (IsProtect == true)
                {
                    rotationProtect += 0.15f;
                    Rectangle rectangle = new Rectangle();
                    rectangle.X = _desRectSkin.X + 35;
                    rectangle.Y = _desRectSkin.Y + 40;
                    rectangle.Width = (int)(112 + 112 * scale);
                    rectangle.Height = (int)(112 + 112 * scale);
                    spriteBatch.Draw(textureProtect, rectangle, null, Color.White, rotationProtect, origin, SpriteEffects.None, 1);

                }
                this.DrawBullet(gameTime, spriteBatch);
                base.Draw(gameTime, spriteBatch, Color.White);
            }          
        }
        public void Dead()
        {
            this.isDeading = true;
            desRectDie.Location = this._desRectSkin.Location + new Point(31, 31);
            //desRectDie.Size = new Point(5, 5);
        }

        #region Game Start
        private void Apperend(GameTime gameTime)
        {
            timeApear += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timeApear >= 1000.0f)
            {
                _desRectSkin = new Rectangle(_desRectSkin.X, _desRectSkin.Y - 5, 72, 71);
            }
            if (timeApear >= 2500.0f)
            {
                Mouse.SetPosition(_desRectSkin.X, _desRectSkin.Y);
                _isMove = true;
                timeApear = 0.0f;
            }
        }

        private void Protect(GameTime gameTime)
        {
            timeApear += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timeApear >= 3000.0f)
            {
                IsProtect = false;
                timeApear = 0.0f;
            }
            else
            {
                UpdateScale();
            }
        }

        private void UpdateScale()
        {
            if (isMaxBig == false)
            {
                scale += 0.005f;
                if (scale >= 0.15f)
                    isMaxBig = true;
            }
            else
            {
                scale -= 0.01f;
                if (scale <= -0.1f)
                    isMaxBig = false;
            }
        }
        #endregion


        #region Bullet
        private void BulletClick(MouseState mouse, KeyboardState keyboard)
        {
            if(timeDelay >= 150.0f)
            {
                if (mouse.LeftButton == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Space))
                {
                    Bullet bullet = new Bullet(_content, new Vector2(0, 10.0f), _index, new Rectangle(mouse.Position - new Point(0, 31), new Point(10,25)), level);
                    _desRectSkin = new Rectangle(_desRectSkin.X, _desRectSkin.Y + 5, _desRectSkin.Width, _desRectSkin.Height);
                    Bullets.Add(bullet);
                    timeDelay = 0.0f;
                }
            }            
        }
        private void UpdateBullet(GameTime gameTime)
        {
            if (Bullets.Count > 0)
                for (int i = 0; i < Bullets.Count; i++)
                {
                    Bullets[i].Update(gameTime);
                    if (!Bullets[i].Visible)
                        Bullets.RemoveAt(i--);
                }
        }
        private void DrawBullet(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Bullets.Count > 0)
                foreach (var bullet in Bullets)
                {
                    bullet.Draw(gameTime, spriteBatch, Color.White);
                }
        }
        #endregion

        #endregion

        #region Destructor
        ~Ship()
        {
            textureProtect = null;
        }
        #endregion

    }
}
