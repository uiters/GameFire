using GameFire.bullet;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameFire.Enemy
{
    public enum TypeChiken
    {
        ChickenGreen = 1,
        ChickenRed = 2,

    }
    public class Chicken : GameObject
    {
        #region Properties
        private TypeChiken type;
        private float totalTime;
        private sbyte indexNow;
        private bool isAttacked;
        private int timeLive;
        private int minScores;

        private Texture2D[] textureDies;
        private Rectangle[] desRectDies;
        private Rectangle sourceRectSkin;

        public int Scores
        {
            get
            {
                int extraScores = (timeLive < 4.0f) ? _random.Next(minScores * 2 / 3, minScores) : _random.Next(minScores / (timeLive / 2), minScores);
                return extraScores + minScores;
            }
        }
        public Point Location
        {
            get => _desRectSkin.Location;
            set => _desRectSkin.Location = value;
        }
        public bool IsAlive { get => _heart > 0; }
        #endregion


        #region Constructor
        public Chicken(ContentManager content, Vector2 speed, Vector2 index, Rectangle location, TypeChiken type, float heart) : base(content, speed, index, location)
        {
            this.type = type;
            this._heart = heart;
            indexNow = 0;
            sourceRectSkin = new Rectangle(location.Width * indexNow, 0, location.Width, location.Height);
            totalTime = 0.0f;
            minScores = (int)((int)type * _heart * 100) + 100;
            this.Load();
        }
        public override Rectangle Bounds
        {
            get
            {
                Rectangle rectangle = new Rectangle(_desRectSkin.Location + new Point(5,5), _desRectSkin.Size - new Point(10,10));
                return rectangle;
            }
        }
        #endregion

        #region Load & unLoad
        protected override void Load()
        {
            switch ((int)type)
            {
                case 1:
                    _skin = _content.Load<Texture2D>("Enemy/chickenGreen");
                    break;
                default:
                    break;
            }
            Texture2D textureDie = _content.Load<Texture2D>("Enemy/dead");
            Rectangle desRectDie = new Rectangle(Point.Zero, new Point(32, 32));
            //animation die
            this.textureDies = new Texture2D[] { textureDie, textureDie, textureDie, textureDie, textureDie };
            this.desRectDies = new Rectangle[] { desRectDie, desRectDie, desRectDie, desRectDie, desRectDie };
        }

        protected override void Unload()
        {
            for (int i = 0; i < desRectDies.Length; i++)
            {
                textureDies[i] = null;
            }
            base.Unload();
        }
        #endregion

        #region Method
        public override void Update(GameTime gameTime)
        {
            if (!Visible)
                return;
            timeLive += gameTime.ElapsedGameTime.Seconds;
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
            this._heart -= bullet.Damage;
            if(_heart <= 0.0f)
            {
                for (int i = 0; i < desRectDies.Length; i++)
                {
                    Point randomLocation = new Point(_random.Next(-15, 15), _random.Next(-15, 15));
                    desRectDies[i].Location = _desRectSkin.Location + randomLocation; 
                }
                return Scores;
            }
            else
            {
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
                sourceRectSkin.X = _desRectSkin.Width * ((indexNow > 9) ? 19 - indexNow : indexNow);
                totalTime = 0.0f;
            }
        }

        public Egg CreateEgg()
        {
            if(_random.Next(0, 10000) == 200)
            {
                return new Egg(_content, new Vector2(0, 2), _index, new Rectangle(_desRectSkin.Center + new Point(-10 , 0), new Point(20,20)), this.type);
            }
            else
            {
                return null;
            }
        }
        #endregion

    }
}
