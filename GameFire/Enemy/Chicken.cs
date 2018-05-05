using GameFire.bullet;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFire.Enemy
{
    public class Chicken : GameObject
    {
        #region Properties
        private static readonly Random random = new Random();
        private int level;
        private float totalTime;
        private sbyte indexNow;
        private bool isAttacked;
        private int timeLive;
        private int minScores;

        private Texture2D[] textureDies;
        private Rectangle[] desRectDies;
        private Rectangle sourceRectSkin;

        public int scores
        {
            get
            {
                int extraScores = (timeLive < 6) ? minScores : random.Next(minScores / (timeLive / 3), minScores);
                return extraScores + minScores;
            }
        }
        public Point Position
        {
            get => _desRectSkin.Location;
            set => _desRectSkin.Location = value;
        }
        public bool IsAlive { get => _heart > 0; }
        #endregion


        #region Constructor
        public Chicken(ContentManager content, Vector2 speed, Vector2 index, Rectangle position, int level, float heart) : base(content, speed, index, position)
        {
            this.level = level;
            this._heart = heart;
            indexNow = 0;
            sourceRectSkin = new Rectangle(position.Width * indexNow, 0, position.Width, position.Height);
            totalTime = 0.0f;
            minScores = (int)(level * _heart * 100) + 100;
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
            switch (level)
            {
                case 1:
                    _skin = _content.Load<Texture2D>("Enemy/chicken");
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
            totalTime += (float) gameTime.ElapsedGameTime.TotalMilliseconds;
            if(_heart > 0)
            {
                if (isAttacked)
                {
                    _desRectSkin.Location = new Point(_desRectSkin.X, _desRectSkin.Y + 2);
                    isAttacked = false;
                }
                if (totalTime >= 70.0f)
                {
                    indexNow = (++indexNow >= 19) ? (sbyte)0 : indexNow;
                    sourceRectSkin.X = _desRectSkin.Width * ((indexNow > 9) ? 19 - indexNow : indexNow);
                    totalTime = 0.0f;
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
                    Point randomLocation = new Point(random.Next(-15, 15), random.Next(-15, 15));
                    desRectDies[i].Location = _desRectSkin.Location + randomLocation; 
                }
                return scores;
            }
            else
            {
                isAttacked = true;
                _desRectSkin.Location = new Point(_desRectSkin.X, _desRectSkin.Y - 2);
                return 0;
            }
        }
        #endregion

    }
}
