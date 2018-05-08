using Microsoft.Xna.Framework;
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

        public float Damage { get => damage; private set => damage = value; }
        #endregion

        #region Constructor
        public Bullet(ContentManager content, Vector2 speed, Vector2 index, Rectangle position, int level) : base(content, speed, index, position)
        {
            this.level = level;
            this.damage = 1.0f;
            Load();
        }
        #endregion

        #region Destructor
        ~Bullet()
        {

        }

        #endregion

        #region Load & unload
        protected override void Load()
        {
            switch (level)
            {
                case 1:
                    _skin = _content.Load<Texture2D>("bullet/a1");
                    break;
                case 2:
                    _skin = _content.Load<Texture2D>("bullet/a2");
                    break;
                case 3:
                    _skin = _content.Load<Texture2D>("bullet/a3");
                    break;
                case 10:
                    _skin = _content.Load<Texture2D>("bullet/b1");
                    break;
                case 11:
                    _skin = _content.Load<Texture2D>("bullet/b2");
                    break;
                case 12:
                    _skin = _content.Load<Texture2D>("bullet/b3");
                    break;
                default:
                    return;
            }
        }
        protected override void Unload()
        {
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
            if (_desRectSkin.Y >= -10)
                _desRectSkin.Location = (_desRectSkin.Location.ToVector2() - _speed).ToPoint();
            else
            {
                this.Visible = false;
            }
        }
        #endregion

    }
}
