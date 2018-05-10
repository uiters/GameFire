using GameFire.Enemy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace GameFire.MapPlay
{
    public class Map
    {
        #region Properties
        protected List<Chicken> _chickens;
        protected ContentManager _content;
        protected Vector2 _index;
        protected bool _isPlay;
        #endregion

        #region Constructor
        public Map(ContentManager content, Vector2 index, bool isPlay)
        {
            _content = content;
            _index = index;
            _isPlay = isPlay;
        }

        virtual internal void Initialize()
        {

        }
        #endregion

        #region Method
        virtual public void Update(GameTime gameTime)
        {

        }

        #endregion
    }
}
