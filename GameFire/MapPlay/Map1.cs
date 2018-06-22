using System.Collections.Generic;
using GameFire.Enemy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace GameFire.MapPlay
{
    public class Map1 : Map
    {
        #region Properties
        protected List<Chicken> chickens;
        private byte mode = 0;
        private float timeNow;
        private bool isLeft;

        public int Count { get => chickens.Count; }
        public override bool IsClean
        {
            get
            {
                if ((_isClean == false && chickens.Count == 0) || _isClean)
                {
                    _isClean = true;
                    return _isClean;
                }
                else return false;
            }
        }
        #endregion

        #region Constructor
        public Map1(ContentManager content, Vector2 index, List<Chicken> chickens, bool isPlay, Rectangle screen) : base(content, index, isPlay)
        {
            this._screen = screen;
            this.chickens = chickens;
            Initialize();
        }

        #endregion

        #region Update
        public override void Update(GameTime gameTime)
        {
            if (!_isPlay) return;
            _totalTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            timeNow += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            switch (mode)
            {
                case 0:
                    Starting();
                    break;
                case 1:
                    Playing();
                    break;
                case 2:
                    Ending();
                    break;
                default:
                    break;
            }
        }
        public void AddChicken(Chicken chicken)
        {
            chickens.Add(chicken);
        }
        #endregion

        #region Private Method Map 1

        /// <summary>
        /// Starting when start map
        /// </summary>
        private void Starting()
        {
            if (timeNow >= 2210.0f)
            {
                timeNow = 0.0f;
                mode = 1;
                return;
            }
            else
            if (_totalTime >= 30.0f)
            {
                _totalTime = 0.0f;
                for (int i = 0; i < Count; i++)
                {
                    if (chickens[i].Visible)
                    {
                        int row = (int)chickens[i].Tag;
                        if (row % 2 == 1)
                        {
                            row = (row == 1) ? i + 1 : row * 10 - i;
                            chickens[i].Location += new Point(row, 0);
                        }
                        else
                        {
                            row = row * 10 - i;
                            chickens[i].Location -= new Point(row, 0);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Playing when chicken is greater 20
        /// </summary>
        private void Playing()
        {
            if (Count >= 20 && timeNow < 15000f)
            {
                if(_totalTime >= 30.0f)
                {
                    if (_totalTime / 2000 > 1)
                    {
                        isLeft = !isLeft;
                        _totalTime = 0;
                    }
                    for (int i = 0; i < Count; i++)
                    {
                        if (isLeft)
                            chickens[i].Location += new Point(2, 0);
                        else
                            chickens[i].Location -= new Point(2, 0);
                    }
                }
            }
            else
            {
                mode = 2;
                timeNow = 0.0f;
            }
        }

        /// <summary>
        /// ending when chickeng is less then 20
        /// </summary>
        private void Ending()
        {
            if (_totalTime >= 30.0f)
            {
                _totalTime = 0.0f;
                for (int i = 0; i < Count; i++)
                {
                    if (_screen.Contains(chickens[i].Bounds) == false)
                        chickens[i].Speed = CalculatePoint(chickens[i].Bounds);
                    else
                    {
                        if (timeNow > 1500.0f && _random.Next(0, 5) == 2)
                        {
                            chickens[i].Speed = new Vector2(_random.Next(-7, 7), _random.Next(-10, 10));
                        }
                    }
                    chickens[i].Location += chickens[i].Speed.ToPoint();
                }
                if(timeNow > 1500.0f)
                    timeNow = 0.0f;
            }

        }

        #region CalculatePoint
        private Vector2 CalculatePoint(Rectangle location)
        {
            Vector2 speed = new Vector2(CalculateX(location.Location), CalculateY(location.Location));
            if (speed.X == -100 && speed.Y != -100) // x don't touch && y touch
            {
                speed.X = _random.Next(-5, 5);
            }
            else
            if (speed.Y == -100 && speed.X != -100) // y don't touch && x touch
            {
                speed.Y = _random.Next(-10, 10);
            }
            return speed;
        }
        private float CalculateY(Point location)
        {
            float y = 0;
            if (location.Y < 15 * _index.Y) // touch the upper corner
            {
                y = _random.Next(2, 10); // chicken move down
            }
            else if (location.Y > 85 * _index.Y) // touch the bottom corner
            {
                y = _random.Next(-10, -5); // chicken move up
            }
            else
            {
                y = -100; // don't touch
            }
            return y;
        }
        private float CalculateX(Point location)
        {
            float x = 0;
            if (location.X < 10 * _index.X) // touch the left corner
            {
                x = _random.Next(5, 9);
            }
            else if (location.X > 90 * _index.X) // touch the right corner
            {
                x = _random.Next(-10, -5);
            }
            else
            {
                x = -100; // don't touch
            }
            return x;

        }
        #endregion

        #endregion

        #region Initialize
        internal override void Initialize()
        {

            //--------------------------------
            // Chicken row 1
            //--------------------------------
            for (int i = 0; i < 10; i++)
            {
                Vector2 speed = new Vector2(_random.Next(1, 5), _random.Next(1, 10));
                Rectangle location = new Rectangle(new Point(-10 * (int)_index.X, 10 * (int)_index.Y), new Point(47, 38));
                Chicken chicken = new Chicken(_content, speed, _index, location, TypeChiken.ChickenGreen, _random.Next(1, 5)) { Tag = 1 };
                chickens.Add(chicken);
            }


            //--------------------------------
            // Chicken row 2
            //--------------------------------
            for (int i = 0; i < 10; i++)
            {
                Vector2 speed = new Vector2(_random.Next(1, 5), _random.Next(1, 10));
                Rectangle location = new Rectangle(new Point(110 * (int)_index.X, 20 * (int)_index.Y), new Point(47, 38));
                Chicken chicken = new Chicken(_content, speed, _index, location, TypeChiken.ChickenGreen, _random.Next(1, 5)) { Tag = 2 };
                chickens.Add(chicken);
            }


            //--------------------------------
            // Chicken row 3
            //--------------------------------
            for (int i = 0; i < 10; i++)
            {
                Vector2 speed = new Vector2(_random.Next(1, 5), _random.Next(1, 10));
                Rectangle location = new Rectangle(new Point(-10 * (int)_index.X, 30 * (int)_index.Y), new Point(47, 38));
                Chicken chicken = new Chicken(_content, speed, _index, location, TypeChiken.ChickenGreen, _random.Next(1, 5)) { Tag = 3 };
                chickens.Add(chicken);
            }


            //--------------------------------
            // Chicken row 4
            //--------------------------------
            for (int i = 0; i < 10; i++)
            {
                Vector2 speed = new Vector2(_random.Next(1, 5), _random.Next(1, 10));
                Rectangle location = new Rectangle(new Point(110 * (int)_index.X, 40 * (int)_index.Y), new Point(47, 38));
                Chicken chicken = new Chicken(_content, speed, _index, location, TypeChiken.ChickenGreen, _random.Next(1, 5)) { Tag = 4 };
                chickens.Add(chicken);
            }

            //--------------------------------
            // Chicken row 5
            //--------------------------------
            for (int i = 0; i < 10; i++)
            {
                Vector2 speed = new Vector2(_random.Next(1, 5), _random.Next(1, 10));
                Rectangle location = new Rectangle(new Point(-10 * (int)_index.X, 50 * (int)_index.Y), new Point(47, 38));
                Chicken chicken = new Chicken(_content, speed, _index, location, TypeChiken.ChickenGreen, _random.Next(1, 5)) { Tag = 5 };
                chickens.Add(chicken);
            }

        }
        #endregion

        #region Destructor
        ~Map1()
        {
            chickens = null;
        }
        #endregion
    }
}
