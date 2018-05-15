using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GameFire.bullet
{
    public class GameObject
    {
        #region Properties
        protected static readonly Random _random = new Random();
        protected ContentManager _content;
        protected Texture2D _skin;
        protected Rectangle _desRectSkin;
        protected Vector2 _speed;
        protected Vector2 _index;
        protected bool _visible;
        protected bool _isMove;
        protected float _heart;
        protected float _timeLive;
        protected int _minScores;

        virtual public int Scores
        {
            get
            {
                _visible = false;
                int extraScores = (_timeLive < 4) ? _random.Next(_minScores * 2 / 3, _minScores * 2) : _random.Next(_minScores / (int)(_timeLive / 2), _minScores);
                return extraScores + _minScores;
            }
        }

        virtual public Rectangle Bounds
        {
            get => _desRectSkin;
        }

        virtual public bool Visible { get => _visible; set => _visible = value; }
        #endregion

        #region Constructor
        public GameObject(ContentManager content, Vector2 speed, Vector2 index, Rectangle location)
        {
            this.Visible = true;
            this._content = content;
            this._speed = speed;
            _desRectSkin = location;
            _index = index;
        }
        #endregion

        #region Destructor
        ~GameObject()
        {

        }
        #endregion

        #region Method
        virtual protected void Load()
        {

        }
        virtual protected void Unload()
        {
            _skin = null;
            _content = null;
        }
        virtual public void Update(GameTime gameTime)
        {

        }
        virtual public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Color color)
        {
            if (Visible == true)
                spriteBatch.Draw(_skin, _desRectSkin, color);
        }
        
        #endregion
    }
}
