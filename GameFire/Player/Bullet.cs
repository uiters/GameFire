using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameFire.bullet
{
    public class Bullet : GameObject
    {
        #region Properties
        private float damage;
        private int level;
        private readonly float maxSpeed = 100.0f;
        private SoundEffect songFire;
        //private List<Texture2D> 
        public float Damage { get => damage; private set => damage = value; }
        #endregion

        #region Constructor

        /// <summary>
        /// position  is Rectangle of Ship
        /// </summary>
        /// <param name="content"></param>
        /// <param name="speed"></param>
        /// <param name="index"></param>
        /// <param name="position"> is Rectangle Ship</param>
        /// <param name="level"></param>
        public Bullet(ContentManager content, Vector2 speed, Vector2 index, Rectangle position, int level) : base(content, speed, index, position)
        {
            this.level = level;     
            this.damage = level;
            Load();
        }
        #endregion

        #region Load & unload
        protected override void Load()
        {
            switch (level)
            {
                case 1:
                    _skin = _content.Load<Texture2D>("bullet/a1");
                    InitializeLevel3();
                    break;
                case 2:
                    _skin = _content.Load<Texture2D>("bullet/a2");
                    InitializeLevel3();
                    break;
                case 3:
                    _skin = _content.Load<Texture2D>("bullet/a3");
                    InitializeLevel3();
                    break;
                //case 4:

                //case 10:
                //    _skin = _content.Load<Texture2D>("bullet/b1");
                //    break;
                //case 11:
                //    _skin = _content.Load<Texture2D>("bullet/b2");
                //    break;
                //case 12:
                //    _skin = _content.Load<Texture2D>("bullet/b3");
                //    break;
                default:
                    _skin = _content.Load<Texture2D>("bullet/a3");
                    InitializeLevel3();
                    break;

            }
            songFire = _content.Load<SoundEffect>("Music/bullet/a");
            songFire.Play(0.35f, 0, 0);
        }
        protected override void Unload()
        {
            songFire = null;
            base.Unload();
        }
        #endregion

        #region Method
        public override void Update(GameTime gameTime)
        {
            if (!Visible)
                return;
            AnimationFly(gameTime);
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Color color)
        {
            base.Draw(gameTime, spriteBatch, color);
        }
        private void AnimationFly(GameTime gameTime)
        {
            if (_speed.Y <= maxSpeed)
                _speed.Y += _speed.Y * 0.05f;
            if (_desRectSkin.Y > -75)
            {
                _desRectSkin.Y -= (int)_speed.Y;
            }
            else
            {
                this.Visible = false;
            }
        }
        #endregion

        #region InitializeLevel
        /// <summary>
        /// Initialize bullet level < four
        /// </summary>
        private void InitializeLevel3()
        {
            _desRectSkin.X = _desRectSkin.Center.X;
            _desRectSkin.Width = _skin.Width * 2;
            _desRectSkin.Height = (int)(_skin.Height * 2.5);
            _desRectSkin.X -= _desRectSkin.Width / 2;
        }
        #endregion

        #region Destructor
        ~Bullet()
        {
            Unload();
        }

        #endregion
    }
}
