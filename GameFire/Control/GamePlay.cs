using GameFire.bullet;
using GameFire.Enemy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFire.Control
{
    public class GamePlay
    {
        #region Properties
        private Ship ship;
        private List<Chicken> chickens;
        private ContentManager content;
        #endregion

        #region Contructor
        public GamePlay(ContentManager content, Vector2 index)
        {
            this.content = content;
            ship = new Ship(content, Vector2.Zero, index);
            chickens = new List<Chicken>();
            Chicken chicken = new Chicken(content, Vector2.Zero, index, new Rectangle(100, 100, 47, 38), 1, 10);
            chickens.Add(chicken);
        }
        #endregion

        #region Method
        public void Update(GameTime gameTime)
        {
            ship.Update(gameTime);
            for (int i = 0; i < chickens.Count; i++)
            {
                if(!chickens[i].Visible) // chicken dead
                {
                    chickens.RemoveAt(i);
                    continue;
                }

                chickens[i].Update(gameTime);

                if(chickens[i].IsAlive)
                    BulletCollision(chickens[i]);
            }
            ShipCollision();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            ship.Draw(gameTime, spriteBatch, Color.White);
            for (int i = 0; i < chickens.Count; i++)
            {
                chickens[i].Draw(gameTime, spriteBatch, Color.White);
            }
        }

        #region Check Collision
        private bool ShipCollision()
        {
            for (int i = 0; i < chickens.Count; i++)
            {
                if (!chickens[i].Visible) // chicken dead
                {
                    chickens.RemoveAt(i);
                    continue;
                }
                else
                if (chickens[i].IsAlive && !ship.IsProtect)
                {
                    Rectangle rect1 = new Rectangle(ship.Bounds.Location + new Point(32, 5), new Point(13, 33));
                    Rectangle rect2 = new Rectangle(ship.Bounds.Location + new Point(10, 37), new Point(62, 35));
                    
                    if(rect1.Intersects(chickens[i].Bounds) || rect2.Intersects(chickens[i].Bounds))
                    {
                        ship.Dead();
                        return true;
                    }
                }
            }
            return false;
        }

        private bool BulletCollision(Chicken chicken)
        {
            for (int i = 0; i < ship.Bullets.Count; i++)
            {
                if (ship.Bullets[i].Bounds.Intersects(chicken.Bounds))
                {
                    chicken.Attacked(ship.Bullets[i]);
                    ship.Bullets[i].Visible = false;
                    ship.Bullets.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }
        #endregion


        #region Map1
        
        #endregion
        #endregion


    }
}
