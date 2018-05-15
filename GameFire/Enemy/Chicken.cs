using GameFire.bullet;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using GameFire.Player;

namespace GameFire.Enemy
{
    public enum TypeChiken
    {
        ChickenGreen = 1,
        ChickenRed = 2,
        ChickenParachuteGreen = 1,
        ChickenParachuteRed = 2,
        BossChickenRed = 3

    }
    public class Chicken : GameObject
    {
        #region Properties
        private TypeChiken type;
        private float totalTime;
        private sbyte indexNow;
        private bool isAttacked;

        private Texture2D[] textureDies;
        private Rectangle[] desRectDies;
        private Rectangle sourceRectSkin;

        private SoundEffect soundDying;
        private SoundEffect soundLaying;
        private SoundEffect soundHurt;

        public Point Location
        {
            get => _desRectSkin.Location;
            set => _desRectSkin.Location = value;
        }
        public bool IsAlive { get => _heart > 0; }
        public Vector2 Speed { get => _speed; set => _speed = value; }
        public object Tag { get; set; }
        public override Rectangle Bounds
        {
            get
            {
                Rectangle rectangle = new Rectangle(_desRectSkin.Location + new Point(5, 5), _desRectSkin.Size - new Point(10, 10));
                return rectangle;
            }
        }
        #endregion

        #region Constructor
        public Chicken(ContentManager content, Vector2 speed, Vector2 index, Rectangle location, TypeChiken type, float heart) : base(content, speed, index, location)
        {
            this.type = type;
            this._heart = heart;
            indexNow =(sbyte) _random.Next(0, 19);
            totalTime = 0.0f;
            _minScores = (int)((int)type * _heart * 100) + 100;
            this.Load();
        }
        #endregion

        #region Load & unLoad
        protected override void Load()
        {
            Texture2D textureDie;
            switch ((int)type)
            {
                case 1:
                    _skin = _content.Load<Texture2D>("Enemy/chickenGreen");
                    if(_desRectSkin.Size == Point.Zero)
                    {
                        _desRectSkin.Width = 47;
                        _desRectSkin.Height = _skin.Height;
                    }
                    sourceRectSkin = new Rectangle(47 * indexNow, 0, 47, _skin.Height);
                    break;
                case 2:
                    _skin = _content.Load<Texture2D>("Enemy/chickenRed");
                    if(_desRectSkin.Size == Point.Zero)
                    {
                        _desRectSkin.Width = 40;
                        _desRectSkin.Height = _skin.Height;
                    }
                    sourceRectSkin = new Rectangle(40 * indexNow, 0, 40 , _skin.Height);
                    break;
                case 3:
                    _skin = _content.Load<Texture2D>("Enemy/bossRed");
                    if(_desRectSkin.Size == Point.Zero)
                    {
                        _desRectSkin.Width = 75;
                        _desRectSkin.Height = 68;
                    }
                    sourceRectSkin = new Rectangle(75 * indexNow, 0, 75, _skin.Height);
                    break;
                default:
                    break;
            }
            textureDie = _content.Load<Texture2D>("Enemy/dead");
            Rectangle desRectDie = new Rectangle(Point.Zero, new Point(32, 32));
            //animation die
            this.textureDies = new Texture2D[] { textureDie, textureDie, textureDie, textureDie, textureDie };
            this.desRectDies = new Rectangle[] { desRectDie, desRectDie, desRectDie, desRectDie, desRectDie };
            LoadSound();
        }

        private void LoadSound()
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
            switch (_random.Next(0,4))
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
            soundLaying = _content.Load<SoundEffect>("Music/chicken/Chicken_lay");
        }

        protected override void Unload()
        {
            for (int i = 0; i < desRectDies.Length; i++)
            {
                textureDies[i] = null;
            }
            soundHurt = null;
            soundLaying = null;
            soundDying = null;
            base.Unload();
        }
        #endregion

        #region Method
        public override void Update(GameTime gameTime)
        {
            if (!Visible)
                return;
            _timeLive +=(float) gameTime.ElapsedGameTime.TotalSeconds;
            if(_heart > 0)
            {
                AnimationFly(gameTime);
                if (isAttacked)
                {
                    _desRectSkin.Location = new Point(_desRectSkin.X, _desRectSkin.Y + 4);
                    isAttacked = false;
                }
            }
            else
            {
                for (int i = 0; i < desRectDies.Length; i++)
                {
                    if (desRectDies[i].Size.X <= 0 || desRectDies[i].Y <= 0)
                    {
                        Visible = false;
                    }
                    else
                    {
                        desRectDies[i] = new Rectangle(desRectDies[i].Location + new Point(2, 2), desRectDies[i].Size - new Point(4, 4));
                    }
                }

            }
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Color color)
        {
            if (!Visible)
            {
                return;
            }
            if(_heart > 0)
            {
                spriteBatch.Draw(_skin, _desRectSkin, sourceRectSkin, color);
            }
            else
            {
                for (int i = 0; i < textureDies.Length; i++)
                {
                    spriteBatch.Draw(textureDies[i], desRectDies[i], color);
                }
            }
        }
        #endregion

        #region Method Chicken
        /// <summary>
        /// Return scores if chicken dies .
        /// if chicken don't dies return 0
        /// </summary>
        /// <param name="bullet"></param>
        /// <returns></returns>
        public int Attacked(Bullet bullet)
        {
            bullet.Visible = false;
            this._heart -= bullet.Damage;
            return GetScores();
        }

        public int Attacked(Ship ship)
        {
            this._heart -= 1.0f;
            return GetScores();
        }

        private int GetScores()
        {
            if (_heart <= 0.0f)
            {
                soundDying.Play(0.875f, 0, 0);
                for (int i = 0; i < desRectDies.Length; i++)
                {
                    Point randomLocation = new Point(_random.Next(-15, 15), _random.Next(-15, 15));
                    desRectDies[i].Location = _desRectSkin.Location + randomLocation;
                }
                return Scores;
            }
            else
            {
                soundHurt.Play(0.875f, 0, 0);
                isAttacked = true;
                _desRectSkin.Location = new Point(_desRectSkin.X, _desRectSkin.Y - 4);
                return 0;
            }
        }

        private void AnimationFly(GameTime gameTime)
        {
            totalTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (totalTime >= 70.0f)
            {
                indexNow = (++indexNow >= 19) ? (sbyte)0 : indexNow;
                sourceRectSkin.X = sourceRectSkin.Width * ((indexNow > 9) ? 19 - indexNow : indexNow);
                totalTime = 0.0f;
            }
        }

        public Egg CreateEgg()
        {
            if(_random.Next(0, 750) == 100)
            {
                soundLaying.Play(0.65f, 0, 0);
                Point sizeEgg;
                int sizeX;
                if (type == TypeChiken.BossChickenRed)
                {
                    sizeEgg = new Point(25, 25);
                }
                else
                {
                    sizeX = 2 * _desRectSkin.Width / 5;
                    sizeEgg = new Point(sizeX, sizeX);
                }          
                return new Egg(_content, new Vector2(0, _random.Next(2, 10)), _index, new Rectangle(_desRectSkin.Center + new Point(-sizeEgg.X / 2, 0) , sizeEgg), this.type);
            }
            else
            {
                return null;
            }
        }

        public Give CrateGive()
        {

            if (_random.Next(1, 25) == 5)
                return new Give(_content, new Vector2(0, _random.Next(2, 6)), _index, new Rectangle(Bounds.Location, Point.Zero));
            else
                return null;
        }
        #endregion

        #region destructor
        ~Chicken()
        {
            Unload();
        }
        #endregion
    }
}
