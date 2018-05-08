using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameFire.Control
{
    public class Scores
    {
        #region Properties
        private long scoresPlay;
        private int length;
        private List<sbyte> indexPre;
        private List<sbyte> indexNow;
        private List<Rectangle> desRectNumber;
        private List<Rectangle> sourceRectNumer;
        private ContentManager content;
        private Texture2D _skin;
        private float timeNow;
        public long ScoresPlay
        {
            get => scoresPlay;
            set
            {
                if (scoresPlay == value) return;
                scoresPlay = value;
                length = 0;
                long scores = value;
                while (scores != 0)
                {
                    if (length < indexNow.Count)
                    {
                        indexNow[length++] = (sbyte)(scores % 10);
                    }
                    else
                    {
                        indexNow.Add((sbyte)(scores % 10));
                        indexPre.Add(0);
                        Rectangle rectangle = new Rectangle(new Point(0, 0), _skin.Bounds.Size);
                        desRectNumber.Add(rectangle);
                        sourceRectNumer.Add(rectangle);
                    }
                    scores /= 10;
                }
                indexNow.Reverse();
            }
        }
        public bool Visiable { get; set; }

        #endregion

        #region Constructor
        public Scores(ContentManager content)
        {
            this.content = content;
            this.indexPre = new List<sbyte>();
            this.indexNow = new List<sbyte>();
            this.desRectNumber = new List<Rectangle>();
            this.sourceRectNumer = new List<Rectangle>();
        }

        #endregion

        #region Load & Unload
        private void Load()
        {
            _skin = content.Load<Texture2D>("scores1");
        }
        private void UnLoad()
        {

        }
        #endregion

        #region Method
        public void Update(GameTime gameTime)
        {
            if (!Visiable) return;
            timeNow += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timeNow >= 50.0f)
            {
                timeNow = 0.0f;
                for (int i = 0; i < length; i++)
                {
                    indexPre[i] = (indexPre[i] == indexNow[i]) ? indexPre[i] : (++indexPre[i] > 10) ? (sbyte)0 : indexPre[i];
                    sourceRectNumer[i] = new Rectangle(_skin.Width * indexPre[i], 0, _skin.Width, _skin.Height);
                }
            }
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!Visiable) return;
            for (int i = 0; i < length; i++)
            {
                spriteBatch.Draw(_skin, desRectNumber[i], sourceRectNumer[i], Color.White);
            }
        }
        private bool Equals(List<int> a, List<int> b)
        {
            if (a.Count != b.Count)
                return false;
            for (int i = 0; i < a.Count; i++)
            {
                if (a[i] != b[i]) return false;
            }
            return true;
        }
        #endregion


    }
}
