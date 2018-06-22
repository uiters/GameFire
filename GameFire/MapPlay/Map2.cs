using System.Collections.Generic;
using GameFire.Enemy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace GameFire.MapPlay
{
    public class Map2 : Map
    {
        #region Properties
        private int countChickens;
        private List<ChickenParachute> chickens;

        public int Count { get => chickens.Count; }
        public override bool IsClean { get => !(countChickens < 100 || chickens.Count > 0); }
        #endregion

        #region Constructor
        public Map2(ContentManager content, Vector2 index, List<ChickenParachute> chickens, bool isPlay, Rectangle screen) : base(content, index, isPlay)
        {
            this._screen = screen;
            this.chickens = chickens;
        }
        #endregion

        #region Method

        public override void Update(GameTime gameTime)
        {
            if(_isPlay)
            {
                if(countChickens < 100)
                {
                    ChickenParachute chicken = GetChickenParachute(gameTime);
                    if (chicken != null)
                    {
                        chickens.Add(chicken);
                        ++countChickens;
                    }                    
                }
            }
        }
        #endregion

        #region private Method
        private ChickenParachute GetChickenParachute(GameTime gameTime)
        {
            _totalTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if(_totalTime >= 1500.0f)
            {
                _totalTime = 0.0f;
                int randomX = _random.Next(5, 99) * (int)_index.X;
                return new ChickenParachute(_content, new Vector2(2, _random.Next(1, 5)), _index,
                    new Rectangle(new Point(randomX, -10), new Point(35, 48)), TypeChiken.ChickenParachuteRed, _random.Next(1,7));
            }
            else
            {
                if (_random.Next(1, 100) == 50)
                {
                    int randomX = _random.Next(5, 99) * (int)_index.X;
                    return new ChickenParachute(_content, new Vector2(2, _random.Next(1, 5)), _index,
                        new Rectangle(new Point(randomX, -10), new Point(35, 48)), TypeChiken.ChickenParachuteRed, _random.Next(1, 5));
                }
                else
                    return null;
            }           
        }
        #endregion

        #region Destructor
        ~Map2()
        {
            chickens.Clear();
            chickens = null;
        }

        #endregion
    }
}
