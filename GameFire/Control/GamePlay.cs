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
        public bool IsPlay { get; set; }
        private Ship ship;
        private List<Chicken> chickens;
        private ContentManager content;
        private List<Egg> eggs;
        private long scores;
        #endregion

        #region Contructor
        public GamePlay(ContentManager content, Vector2 index)
        {
            this.IsPlay = true;
            this.content = content;
            ship = new Ship(content, Vector2.Zero, index);
            chickens = new List<Chicken>();
            Chicken chicken = new Chicken(content, Vector2.Zero, index, new Rectangle(100, 100, 47, 38), TypeChiken.ChickenGreen, 10);
            chickens.Add(chicken);
            eggs = new List<Egg>();
        }
        #endregion

        #region Method
        public void Update(GameTime gameTime)
        {
            if(IsPlay)
            {
                UpdateShip(gameTime);
                UpdateChickens(gameTime);
                UpdateEggs(gameTime);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {  
            DrawShip(gameTime, spriteBatch);
            DrawChickens(gameTime, spriteBatch);
            DrawEggs(gameTime, spriteBatch);
        }


        #endregion

        #region Check Collision
        private bool ShipCollision()
        {
            return ShipCollisionChicken() || ShipCollisionEgg();
        }

        /// <summary>
        /// Kill Chiken & add scores
        /// </summary>
        /// <param name="chicken"></param>
        /// <returns></returns>
        private bool BulletCollision(Chicken chicken)
        {
            for (int i = 0; i < ship.Bullets.Count; i++)
            {
                if (ship.Bullets[i].Bounds.Intersects(chicken.Bounds))
                {
                    scores += chicken.Attacked(ship.Bullets[i]);
                    ship.Bullets[i].Visible = false;
                    ship.Bullets.RemoveAt(i--);
                    return true;
                }
            }
            return false;
        }


        private bool ShipCollisionChicken()
        {
            for (int i = 0; i < chickens.Count; i++)
            {
                if (!chickens[i].Visible) // chicken dead
                {
                    chickens.RemoveAt(i);
                    continue;
                }
                else
                if (chickens[i].IsAlive && !ship.IsProtect) // collision with chicken
                {
                    Rectangle rect1 = new Rectangle(ship.Bounds.Location + new Point(32, 5), new Point(13, 33));
                    Rectangle rect2 = new Rectangle(ship.Bounds.Location + new Point(10, 37), new Point(62, 35));

                    if (rect1.Intersects(chickens[i].Bounds) || rect2.Intersects(chickens[i].Bounds))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private bool ShipCollisionEgg()
        {
            for (int i = 0; i < eggs.Count; i++)
            {
                if (!eggs[i].Visible)
                {
                    eggs.RemoveAt(i--);
                    continue;
                }
                else
                {
                    if (!eggs[i].IsBreak && !ship.IsProtect)
                    {
                        Rectangle rect1 = new Rectangle(ship.Bounds.Location + new Point(32, 5), new Point(13, 33));
                        Rectangle rect2 = new Rectangle(ship.Bounds.Location + new Point(10, 37), new Point(62, 35));
                        if (rect1.Intersects(eggs[i].Bounds) || rect2.Intersects(eggs[i].Bounds))
                        {
                            eggs.RemoveAt(i);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        #endregion

        #region Ship
        private void UpdateShip(GameTime gameTime)
        {
            ship.Update(gameTime);
            if(ShipCollision() == true)
            {
                ship.Dead();
            }
        }
        private void DrawShip(GameTime gameTime, SpriteBatch spriteBatch)
        {
            ship.Draw(gameTime, spriteBatch, Color.White);
        }
        #endregion

        #region Chicken
        private void UpdateChickens(GameTime gameTime)
        {
            for (int i = 0; i < chickens.Count; i++)
            {
                if (!chickens[i].Visible) // chicken dead
                {
                    chickens.RemoveAt(i--);
                    continue;
                }
                chickens[i].Update(gameTime);

                Egg egg = chickens[i].CreateEgg();
                if (egg != null)
                    eggs.Add(egg);

                if (chickens[i].IsAlive)
                    BulletCollision(chickens[i]);
            }
        }
        private void DrawChickens(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < chickens.Count; i++)
            {
                chickens[i].Draw(gameTime, spriteBatch, Color.White);
            }
        }
        #endregion

        #region Eggs
        private void UpdateEggs(GameTime gameTime)
        {
            for (int i = 0; i < eggs.Count; i++)
            {
                if (!eggs[i].Visible) // chicken dead
                {
                    eggs.RemoveAt(i--);
                    continue;
                }
                else
                {
                    eggs[i].Update(gameTime);
                }
            }
        }
        private void DrawEggs(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < eggs.Count; i++)
            {
                eggs[i].Draw(gameTime, spriteBatch, Color.White);
            }
        }
        #endregion
    }
}
