using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;

namespace GameFire.MapPlay
{
    public class Map
    {
        #region Properties
        protected readonly Random _random = new Random();
        protected ContentManager _content;
        protected Vector2 _index;
        protected bool _isPlay;
        protected bool _isClean;
        protected Rectangle _screen;
        protected float _totalTime;

        public virtual bool IsClean { get; set; }
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
