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
        private int length;
        private Rectangle screen;
        public int Count { get => _chickens.Count; }
        #endregion

        #region Constructor
        public Map2(ContentManager content, Vector2 index, List<Chicken> chickens, bool isPlay, Rectangle screen) : base(content, index, isPlay)
        {
            this.screen = screen;
            _chickens = chickens;
            Initialize();
        }
        #endregion


        #region Initialize
        internal override void Initialize()
        {
            for (int i = 0; i < 10; i++)
            {

            }
        }
        #endregion
    }
}
