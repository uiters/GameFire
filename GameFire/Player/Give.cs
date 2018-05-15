using GameFire.bullet;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace GameFire.Player
{
    public class Give :GameObject
    {
        #region Properties
        private sbyte indexNow;
        private Rectangle sourceRectSkin;
        private float totalTime;
        private SoundEffect soundLevelUp;
        #endregion

        #region Constructor
        /// <summary>
        /// location.size == point.zero auto size
        /// </summary>
        /// <param name="content"></param>
        /// <param name="speed"></param>
        /// <param name="index"></param>
        /// <param name="location"></param>
        public Give(ContentManager content, Vector2 speed, Vector2 index, Rectangle location ) : base(content, speed, index, location)
        {
            this._isMove = false;
            indexNow =(sbyte) _random.Next(0, 26);
            
            Load();
        }
        #endregion

        #region Load & unload
        protected override void Load()
        {
            _skin = _content.Load<Texture2D>("bullet/give");
            if (_desRectSkin.Size == Point.Zero)
            {
                _desRectSkin.Width = 44;
                _desRectSkin.Height = 37;
            }
            sourceRectSkin = _desRectSkin;
            sourceRectSkin.X = indexNow * 44;
            sourceRectSkin.Y = 0;
            soundLevelUp = _content.Load<SoundEffect>("Music/bullet/levelUp");
        }
        protected override void Unload()
        {
            soundLevelUp = null;
            base.Unload();
        }
        #endregion

        #region Method
        public override void Update(GameTime gameTime)
        {
            if(Visible)
            {
                AnimationRotation(gameTime);
                AnimationFall(gameTime);
            }
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Color color)
        {
            if(Visible)
            spriteBatch.Draw(_skin, _desRectSkin, sourceRectSkin ,color);
        }
        public void SoundPlay()
        {
            soundLevelUp.Play();
        }
        #endregion

        #region Method private
        private void AnimationFall(GameTime gameTime)
        {
            _timeLive += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if(_timeLive >35.0f)
            {
                if (_desRectSkin.Y < 105 * _index.Y)
                {
                    _desRectSkin.Y += (int)_speed.Y;
                    _timeLive = 0.0f;
                }
                else
                    _visible = false;           
            }
        }
        private void AnimationRotation(GameTime gameTime)
        {
            totalTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (totalTime >= 33.0f)
            {
                indexNow = (++indexNow >= 25) ? (sbyte)0 : indexNow;
                sourceRectSkin.X = 44 * indexNow;
                totalTime = 0.0f;
            }
        }
        
        #endregion
    }
}
