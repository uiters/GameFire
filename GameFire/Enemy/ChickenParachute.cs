using GameFire.bullet;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameFire.Enemy
{
    public class ChickenParachute : GameObject
    {
        #region Properties
        private TypeChiken type;
        private float totalTime;
        private float timeAttacked;

        private bool isAttack;

        private float rotation;
        private Vector2 origin;

        private float shake;


        private SoundEffect soundDying;
        private SoundEffect soundHurt;

        /// <summary>
        /// return bound of chicken
        /// </summary>
        public override Rectangle Bounds
        {
            get => new Rectangle(_desRectSkin.X - _desRectSkin.Width / 2, _desRectSkin.Y - _desRectSkin.Height / 2, _desRectSkin.Width, _desRectSkin.Height);
        }
        #endregion

        #region Constructor
        public ChickenParachute(ContentManager content, Vector2 speed, Vector2 index, Rectangle location, TypeChiken type, float heart)
            : base(content, speed, index, location)
        {
            this.shake = 0.0085f;
            this.rotation = _random.Next(-1, 1) / _random.Next(4, 8);
            this._minScores = (int)((int)type * _heart * 100) + 100;
            this._heart = heart;
            this.type = type;
            this.isAttack = false;
            this.Load();
        }
        #endregion

        #region Load & unload
        protected override void Load()
        {
            _skin = _content.Load<Texture2D>("Enemy/chickenParachute");
            origin = (_skin.Bounds.Center.ToVector2());
            LoadSound();
        }
        protected override void Unload()
        {
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
            if (Visible)
            {
                _timeLive += (float)gameTime.ElapsedGameTime.TotalSeconds;
                ChickenFall(gameTime);
                if (isAttack)
                    AnimationAttacked(gameTime);
            }
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Color color)
        {
            if (!Visible) return;
            else
                spriteBatch.Draw(_skin, _desRectSkin, null, Color.White, rotation, origin, SpriteEffects.None, 0);
        }
        public Chicken GetChicken()
        {          
            return new Chicken(_content, _index, _index, new Rectangle(Bounds.Location, Point.Zero), type , _random.Next(2, 5));
        }
        #endregion

        #region Private Method
        private void ChickenFall(GameTime gameTime)
        {
            totalTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (totalTime > 30f)
            {
                totalTime = 0;
               _desRectSkin.Y += (int)_speed.Y;
                shake = (rotation > 0.195f || rotation < -0.195f) ? -shake : shake;
                rotation += shake;
            }
            if (_desRectSkin.Y > 105 * _index.Y)
                Visible = false;
        }
        public int Attacked(Bullet bullet)
        {
            bullet.Visible = false;
            this._heart -= bullet.Damage;
            return GetScores(bullet);
        }
        public int Attacked(Ship ship)
        {
            this._heart -= 1.0f;
            return GetScores(ship);
        }
        private int GetScores(Bullet bullet)
        {
            if (_heart <= 0.0f)
            {
                soundDying.Play();
                return Scores;
            }
            else
            {
                soundHurt.Play();
                isAttack = true;
                return 0;
            }
        }
        private int GetScores(Ship ship)
        {
            if (_heart <= 0.0f)
            {
                soundDying.Play();
                return Scores;
            }
            else
            {
                soundHurt.Play();
                return 0;
            }
        }
        private void AnimationAttacked(GameTime gameTime)
        {
            timeAttacked += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if(timeAttacked > 30.0f)
            {
                _desRectSkin.Y -= 4;
                isAttack = false;
                timeAttacked = 0.0f;
            }
        }
        #endregion

        #region Destructor
        ~ChickenParachute()
        {
            Unload();
        }
        #endregion
    }
}
