using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameFire.bullet
{
    public class Bullet
    {
        protected float speed;
        protected float damge;
        protected int x;
        protected int y;
        protected Texture2D textureBullet;
        protected SpriteBatch spriteBatch;
        public Bullet()
        {
            speed = 0;
            x = 0;
            y = 0;
        }
        public Bullet(float speed, float baseDamge)
        {
            this.speed = speed;
            this.damge = baseDamge;
        }
        virtual public void Fire(SpriteBatch spriteBatch, Point pointStart) { }
        virtual public void Update() { }
    }
}

namespace GameFire.bullet
{
    class GreenBullet : Bullet
    {
        public GreenBullet()
        {

        }
        public GreenBullet(float speed, float baseDamge) : base(speed, baseDamge)
        {
           
        }
        public int X { set => x = value; get => x; }
        public int Y { set => y = value; get => y; }

        public override void Fire(SpriteBatch spriteBatch, Point pointStart)
        {

            x = pointStart.X;
            y = pointStart.Y;
            Fire(spriteBatch, x, y);
        }
        public void Fire(SpriteBatch spriteBatch, int x, int y)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred);

            Rectangle rectangle1 = new Rectangle(x, y, (int)(textureBullet.Width / 2.5), (int)(textureBullet.Height));
            Rectangle rectangle2 = new Rectangle(0, 0, (int)(textureBullet.Width / 2.5), (int)(textureBullet.Height));
            spriteBatch.Draw(textureBullet, rectangle1, rectangle2, Color.White);
            spriteBatch.End();
        }
    }
}

