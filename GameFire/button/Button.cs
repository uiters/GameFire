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

        protected int _index;
        protected bool _visible;
        protected bool _isMouseHover;
        protected bool _isMouseClick;

        public bool Visible { get => _visible; set => _visible = value; }
        public Rectangle DestRectangle { get => _destRectangle; set => _destRectangle = value; }
        public int Index { get => _index; }
        public bool IsMouseClick { get => _isMouseClick; private set => _isMouseClick = value; }

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
            _sourceRectangle = new Rectangle(skin.Width * index / 10, 0, skin.Width / 10, skin.Height);
            float zoomWidth = DestRectangle.Width * zoom;
            float zoomHeight = DestRectangle.Height * zoom;
            _destRectanleClick = new Rectangle(DestRectangle.X - (int)zoomWidth / 2, DestRectangle.Y - (int)zoomHeight / 2, (int)zoomWidth + DestRectangle.Width, (int)zoomHeight + DestRectangle.Height);
            Visible = visible;
            _index = index;
        }
        #endregion

        #region Destructor
        ~Button()
        {
            _skin = null;
        }
        #endregion

        #region Method
        public bool isMouseHover(MouseState mouseState)
        {
            if (Visible)
            {
                _isMouseHover = DestRectangle.Contains(mouseState.Position);
                IsMouseClick = (mouseState.LeftButton == ButtonState.Pressed) ? true : false;
                return _isMouseHover;
            }
            else
                return false;
        }
        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            if (!Visible)
                return;
            else
            {
                if (_isMouseHover && !IsMouseClick)
                {
                    spriteBatch.Draw(_skin, _destRectanleClick, _sourceRectangle, color);
                }
                else
                    spriteBatch.Draw(_skin, DestRectangle, _sourceRectangle, color);
            }
        }
        #endregion

    }
}
