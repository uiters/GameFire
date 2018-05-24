using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameFire.button
{
    public class Button
    {
        #region Properties
        protected Rectangle _destRectangle;
        protected Rectangle _destRectanleClick;
        protected Rectangle _sourceRectangle;
        protected Texture2D _skin;
        protected float _zoom;
        protected Color _color;
        protected int _index;
        protected bool _visible;
        protected bool _isMouseHover;
        protected bool _isMouseClick;
        protected bool _isChangeColor;
        private int color;
        private int increments = 5;

        public bool Visible { get => _visible; set => _visible = value; }
        public Rectangle DestRectangle
        {
            get => _destRectangle;
             set
            {
                _destRectangle = value;
                SourceRectangle = new Rectangle(_skin.Width * _index / 10, 0, _skin.Width / 10, _skin.Height);
                float zoomWidth = DestRectangle.Width * _zoom;
                float zoomHeight = DestRectangle.Height * _zoom;
                _destRectanleClick = new Rectangle(DestRectangle.X - (int)zoomWidth / 2, DestRectangle.Y - (int)zoomHeight / 2, (int)zoomWidth + DestRectangle.Width, (int)zoomHeight + DestRectangle.Height);
            }
        }
        public int Index { get => _index; }
        public bool IsMouseClick { get => _isMouseClick; private set => _isMouseClick = value; }
        public bool IsChangeColor { get => _isChangeColor; set => _isChangeColor = value; }
        public Rectangle SourceRectangle { get => _sourceRectangle; set => _sourceRectangle = value; }

        #endregion

        #region Constructor
        /// <summary>
        /// Button
        /// </summary>
        /// <param name="skin"></param> texture
        /// <param name="destRectangle"></param> about location, and position of one texture
        /// <param name="index"></param> current image in tuture
        /// <param name="visible"></param> show or not show
        /// <param name="zoom"></param> show %
        /// <param name="scale"></param> scale
        public Button(Texture2D skin, Rectangle destRectangle, int index = 0, bool visible = true, float zoom = 0.1f)
        {
            _skin = skin;
            DestRectangle = destRectangle;
            SourceRectangle = new Rectangle(skin.Width * index / 10, 0, skin.Width / 10, skin.Height);
            float zoomWidth = DestRectangle.Width * zoom;
            float zoomHeight = DestRectangle.Height * zoom;
            _destRectanleClick = new Rectangle(DestRectangle.X - (int)zoomWidth / 2, DestRectangle.Y - (int)zoomHeight / 2, (int)zoomWidth + DestRectangle.Width, (int)zoomHeight + DestRectangle.Height);
            Visible = visible;
            _index = index;
            _zoom = zoom;
            color = 0;
            _isChangeColor = true;
            _color = Color.White;
        }
        #endregion

        #region Destructor
        ~Button()
        {
            _skin = null;
        }
        #endregion

        #region Method
        public bool isMouseHover()
        {
            if (Visible)
            {
                if (_isChangeColor)
                {
                    if (color > 250)
                    {
                        increments = -5;
                    }
                    else if (color < 100)
                    {
                        increments = 5;
                    }
                    color += increments;
                    _color = new Color(color, color, color);
                }
                MouseState mouseState = Mouse.GetState();
                _isMouseHover = DestRectangle.Contains(mouseState.Position);
                IsMouseClick = (mouseState.LeftButton == ButtonState.Pressed) ? true : false;
                return _isMouseHover;
            }
            else
                return false;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible)
                return;
            else
            {
                if (_isMouseHover && !IsMouseClick)
                {
                    spriteBatch.Draw(_skin, _destRectanleClick, SourceRectangle, _color);
                }
                else
                    spriteBatch.Draw(_skin, DestRectangle, SourceRectangle, _color);
            }
        }
        #endregion

    }
}
