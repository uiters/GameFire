using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFire.bullet
{
    public class Ship : GameObject
    {

        #region Properties
        private float timeApear = 0f;
        private bool isProtect = true;
        private Texture2D textureProtect;
        private float rotationProtect = 0.0f;
        private float scale = 0.0f;
        private bool isMaxBig = false;
        private Vector2 origin;
        #endregion

        #region Constructor
        public Ship(ContentManager content, Vector2 speed, Vector2 index) : base(content, speed, index, Rectangle.Empty)
        {
            _heart = 3;
            this._visible = false;
        }
        #endregion


        #region Method
        protected override void Load()
        {
            _skin = _content.Load<Texture2D>("ship");
            textureProtect = _content.Load<Texture2D>("procted");
            MouseState mouse = Mouse.GetState();
            _desRectSkin = new Rectangle(mouse.X, mouse.Y, 72, 71);
            origin = new Vector2(textureProtect.Width / 2, textureProtect.Height / 2);
        }
        public override void Update(GameTime gameTime)
        {
            if(_visible == true)
            {
                if(_isMove == false)
                {
                    this.Apperend(gameTime);
                    UpdateScale();                  
                }
                else
                {
                    if (isProtect == true)
                        Protect(gameTime);
                    MouseState mouse = Mouse.GetState();
                    if (_desRectSkin.X != mouse.X || _desRectSkin.Y != mouse.Y)
                        _desRectSkin = new Rectangle(mouse.X - 31, mouse.Y - 31, 72, 71);
                }
            }
            else
            {
                if(_heart > 0)
                {
                    _desRectSkin = new Rectangle((int)_index.X * 45, (int)_index.Y * 110, 72, 71);
                    _visible = true;
                    _isMove = false;
                    --_heart;
                }
            }

        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            
            if (isProtect == true)
            {
                rotationProtect += 0.15f;

                Rectangle rectangle = new Rectangle();               
                rectangle.X = _desRectSkin.X + 35;
                rectangle.Y = _desRectSkin.Y + 40;
                rectangle.Width = (int)(112 + 112 * scale);
                rectangle.Height = (int)(112 + 112 * scale);

                spriteBatch.Draw(textureProtect, rectangle, null, Color.White, rotationProtect, origin, SpriteEffects.None, 1);
                
            }
            base.Draw(gameTime, spriteBatch);
        }

        private void Apperend(GameTime gameTime)
        {
            timeApear += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if(timeApear >= 1000.0f)
            {
                _desRectSkin = new Rectangle(_desRectSkin.X, _desRectSkin.Y - 5, 72, 71);
            }
            if(timeApear >= 2500.0f)
            {
                Mouse.SetPosition(_desRectSkin.X, _desRectSkin.Y);
                _isMove = true;
                timeApear = 0.0f;
            }
        }
        
        private void Protect(GameTime gameTime)
        {
            timeApear += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if(timeApear >= 3000.0f)
            {
                isProtect = false;
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

    }
}
