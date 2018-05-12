using GameFire.bullet;
using GameFire.MapPlay;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace GameFire.Enemy
{
    public class ChickenParachute : GameObject
    {
        #region Properties
        private TypeChiken type;
        private float totalTime;
        private int minScores;
        private bool isAttacked;
        private Chicken chicken;
        private bool isAlive;

        private float rotation;
        private Vector2 origin;

        private float distance;


        private SoundEffect soundDying;
        private SoundEffect soundHurt;

        public Chicken Chicken { get => chicken; }
        #endregion

        #region Constructor
        public ChickenParachute(ContentManager content, Vector2 speed, Vector2 index, Rectangle location, TypeChiken type, float heart) 
            : base(content, speed, index, location)
        {
            this.distance = 0.01f;
            isAlive = true;
            this.rotation = _random.Next(-2, 2) / _random.Next(1 , 4);
            chicken = new Chicken(content, speed, index, new Rectangle(location.Location, new Point(47, 38)), TypeChiken.ChickenRed, 2);
            Load();
        }
        #endregion

        #region Load & unload
        protected override void Load()
        {
            _skin = _content.Load<Texture2D>("Enemy/chickenParachute");
            origin = _skin.Bounds.Center.ToVector2();
            LoadSound();
        }
        protected override void Unload()
        {
            chicken = null;
            soundDying = null;
            soundHurt = null;
            base.Unload();
        }
        protected void LoadSound()
        {
            switch (_random.Next(0, 5))
            {
                case 0:
                    soundDying = _content.Load<SoundEffect>("Music/chicken/Chicken_death1");
                    break;
                case 1:
                    soundDying = _content.Load<SoundEffect>("Music/chicken/Chicken_death2");
                    break;
                case 3:
                    soundDying = _content.Load<SoundEffect>("Music/chicken/Chicken_death3");
                    break;
                default:
                    soundDying = _content.Load<SoundEffect>("Music/chicken/Chicken_death4");
                    break;
            }
            switch (_random.Next(0, 4))
            {
                case 0:
                    soundHurt = _content.Load<SoundEffect>("Music/chicken/Chicken_hurt1");
                    break;
                case 1:
                    soundHurt = _content.Load<SoundEffect>("Music/chicken/Chicken_hurt2");
                    break;
                default:
                    soundHurt = _content.Load<SoundEffect>("Music/chicken/Chick_hurt1");
                    break;
            }
        }
        #endregion

        #region Method
        public override void Update(GameTime gameTime)
        {
            if(Visible)
            {
                if (isAlive)
                {
                    ChickenFall(gameTime);
                }
                else
                {
                    if (chicken.IsAlive)
                        chicken.Update(gameTime);
                    else
                        Visible = false;
                }
            }
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Color color)
        {
            spriteBatch.Draw(_skin, _desRectSkin, null, Color.White, rotation, origin, SpriteEffects.None, 1);
        }
        #endregion

        #region Private Method
        private void ChickenFall(GameTime gameTime)
        {
            totalTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (totalTime > 30f)
            {
                _desRectSkin.Y += (int)_speed.Y;
                distance = (rotation > 0.3f || rotation < -0.3f) ? -distance : distance;
                rotation += distance;
            }
        }
        #endregion
    }
}
