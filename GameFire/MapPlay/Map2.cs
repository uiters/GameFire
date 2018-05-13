using System;
using System.Collections.Generic;
using GameFire.Enemy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace GameFire.MapPlay
{
    public class Map2 : Map
    {
        #region Properties
        private readonly Random random = new Random();
        private int length;
        private Rectangle screen;
        List<ChickenParachute> chickens;
        private float totalTime;

        public int Count { get => chickens.Count; }

        #endregion

        #region Constructor
        public Map2(ContentManager content, Vector2 index, List<ChickenParachute> chickens, bool isPlay, Rectangle screen) : base(content, index, isPlay)
        {
            this.screen = screen;
            this.chickens = chickens;
        }
        #endregion

        #region Method

        public override void Update(GameTime gameTime)
        {
            if(_isPlay)
            {
                if(length < 50)
                {
                    ChickenParachute chicken = GetChickenParachute(gameTime);
                    if (chicken != null)
                    {
                        chickens.Add(chicken);
                        ++length;
                    }                    
                }
            }
        }
        #endregion

        #region private Method
        private ChickenParachute GetChickenParachute(GameTime gameTime)
        {
            totalTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if(totalTime >= 2500.0f)
            {
                totalTime = 0.0f;
                int randomX = random.Next(5, 99) * (int)_index.X;
                return new ChickenParachute(_content, new Vector2(2, random.Next(1, 5)), _index,
                    new Rectangle(new Point(randomX, -10), new Point(35, 48)), TypeChiken.ChickenParachuteRed, 1);// random.Next(1,10));
            }
            else
            {
                if (random.Next(1, 100) == 50)
                {
                    int randomX = random.Next(5, 99) * (int)_index.X;
                    return new ChickenParachute(_content, new Vector2(2, random.Next(1, 5)), _index,
                        new Rectangle(new Point(randomX, -10), new Point(35, 48)), TypeChiken.ChickenParachuteRed, 1);// random.Next(1, 10));
                }
                else
                    return null;
            }           
        }
        #endregion
    }
}
