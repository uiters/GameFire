using GameFire.bullet;
using GameFire.Enemy;
using GameFire.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace GameFire.MapPlay
{
    public class GamePlay
    {
        #region Properties
        public bool IsPlay { get; set; }
        public Rectangle Screen { get => screen; private set => screen = value; }
        //public List<Button> Buttons { set => buttons = value; }
        public bool Visible { get => visible; set => visible = value; }
        public bool IsEndGame { get => alreadySet; }

        private Ship ship;
        private List<Chicken> chickens;
        private ContentManager content;
        private List<Egg> eggs;
        private Scores scores;
        private Map1 map1;
        private Map2 map2;
        private Song song;
        private List<ChickenParachute> chickenParachutes;
        private List<Gift> gifts;
        private float timeEnd;
        private Rectangle rectEndGame;
        private Texture2D skinEndGame;
        private bool isWin;

        private SoundEffect soundLose;
        private SoundEffect soundWin;
        //private List<Button> buttons;
        private Vector2 index;
        private Rectangle screen;
        private bool isLeft;
        private bool isTop;
        private bool isShipDie;
        private float timeShake;
        private float totalTimeShake;
        private int currentMap;
        private bool alreadySet;
        private bool visible;
        #endregion

        #region Contructor
        public GamePlay(ContentManager content, Vector2 index, Rectangle screen)
        {
            this.Screen = screen;
            this.index = index;
            this.IsPlay = true;
            this.content = content;
            ship = new Ship(content, Vector2.Zero, index);
            chickens = new List<Chicken>();
            chickenParachutes = new List<ChickenParachute>();
            map1 = new Map1(content, index, chickens, IsPlay, screen);
            map2 = new Map2(content, index, chickenParachutes, IsPlay, screen);
            gifts = new List<Gift>();
            eggs = new List<Egg>();
            scores = new Scores(content);
            currentMap = 1;
            this.visible = true;
            this.isWin = false;
            this.timeEnd = 0;
            Load();
        }
        #endregion

        #region Load & unload
        private void Load()
        {
            song = content.Load<Song>("Music/backgroundMap");
            MediaPlayer.Volume = 0.5f;
            MediaPlayer.Play(song);
            soundLose = content.Load<SoundEffect>("Music/Gameover");
            soundWin = content.Load<SoundEffect>("Music/Gamewin");
        }
        private void Unload()
        {
            ship = null;
            chickens.Clear();
            eggs.Clear();
            chickenParachutes.Clear();
            MediaPlayer.Stop();
            song = null;
            scores = null;
            map1 = null;
            eggs = null;
            chickens = null;
            chickenParachutes = null;
            content = null;
            soundLose = null;
            soundWin = null;
            skinEndGame = null;
        }
        #endregion

        #region Method
        public void Update(GameTime gameTime)
        {
            if (Visible)
            {
                if (IsPlay)
                {
                    UpdateMap1(gameTime);
                    UpdateShip(gameTime);
                    UpdateChickens(gameTime);
                    UpdateEggs(gameTime);
                    UpdateScores(gameTime);
                    UpdateGives(gameTime);
                    if (currentMap == 2)
                    {
                        UpdateMap2(gameTime);
                        if (map2.IsClean  && chickens.Count < 1 && alreadySet == false)
                        {
                            isWin = true;
                            skinEndGame = content.Load<Texture2D>("background/win");
                            rectEndGame.X = (int)(25 * index.X);
                            rectEndGame.Y = (int)(15 * index.Y);
                            rectEndGame.Width = skinEndGame.Width;
                            rectEndGame.Height = skinEndGame.Height;
                            MediaPlayer.Stop();
                            soundWin.Play();
                            alreadySet = true;
                        }
                    }
                    UpdateChickenParachutes(gameTime);
                    if (map1.IsClean && currentMap < 2)
                        currentMap++;

                    if (isShipDie)
                    {
                        ScreenShake(gameTime);
                        if (ship.Heart <= 0 && alreadySet == false)
                        {
                            skinEndGame = content.Load<Texture2D>("background/lose");
                            rectEndGame.X = (int)(25 * index.X);
                            rectEndGame.Y = (int)(15 * index.Y);
                            rectEndGame.Width = skinEndGame.Width;
                            rectEndGame.Height = skinEndGame.Height;
                            MediaPlayer.Stop();
                            soundLose.Play();
                            alreadySet = true;
                        }
                    }
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (visible)
            {
                //DrawButtons(gameTime, spriteBatch);
                DrawScores(gameTime, spriteBatch);
                DrawChickens(gameTime, spriteBatch);
                DrawGives(gameTime, spriteBatch);
                DrawEggs(gameTime, spriteBatch);
                DrawChickenParachutes(gameTime, spriteBatch);
                DrawShip(gameTime, spriteBatch);
                if (ship.Heart <= 0 || isWin)
                    DrawLose(gameTime, spriteBatch);
            }
        }


        #endregion

        #region Check Collision
        private bool ShipCollision()
        {
            ShipCollisionGive();
            return ShipCollisionChicken() || ShipCollisionEgg() || ShipCollisionChickenParachute();
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
                    scores.ScoresPlay += chicken.Attacked(ship.Bullets[i]);
                    ship.Bullets.RemoveAt(i--);
                    if (chicken.IsAlive == false)
                    {
                        Gift gift = chicken.CrateGif();
                        if (gift != null)
                            gifts.Add(gift);
                    }
                    return true;
                }
            }
            return false;
        }
        private bool BulletCollision(ChickenParachute chickenParachute)
        {
            for (int i = 0; i < ship.Bullets.Count; i++)
            {
                if (ship.Bullets[i].Bounds.Intersects(chickenParachute.Bounds))
                {
                    int score = chickenParachute.Attacked(ship.Bullets[i]);
                    if (score > 0)
                    {
                        scores.ScoresPlay += score;
                        chickens.Add(chickenParachute.GetChicken());
                    }
                    ship.Bullets.RemoveAt(i--);
                    return true;
                }
            }
            return false;
        }

        private bool ShipCollisionChicken()
        {
            Rectangle rect1 = new Rectangle(ship.Bounds.Location + new Point(32, 5), new Point(13, 33));
            Rectangle rect2 = new Rectangle(ship.Bounds.Location + new Point(10, 37), new Point(62, 35));
            for (int i = 0; i < chickens.Count; i++)
            {
                if (!chickens[i].Visible) // chicken dead
                {
                    chickens.RemoveAt(i--);
                    continue;
                }
                else
                if (!ship.IsDeading && chickens[i].IsAlive && !ship.IsProtect) // collision with chicken
                {                  
                    if (rect1.Intersects(chickens[i].Bounds) || rect2.Intersects(chickens[i].Bounds))
                    {
                        scores.ScoresPlay += chickens[i].Attacked(ship);
                        return true;
                    }
                }
            }
            return false;
        }
        private bool ShipCollisionEgg()
        {
            Rectangle rect1 = new Rectangle(ship.Bounds.Location + new Point(32, 5), new Point(13, 33));
            Rectangle rect2 = new Rectangle(ship.Bounds.Location + new Point(10, 37), new Point(62, 35));
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
        private bool ShipCollisionChickenParachute()
        {
            Rectangle rect1 = new Rectangle(ship.Bounds.Location + new Point(32, 5), new Point(13, 33));
            Rectangle rect2 = new Rectangle(ship.Bounds.Location + new Point(10, 37), new Point(62, 35));
            for (int i = 0; i < chickenParachutes.Count; i++)
            {
                if (!chickenParachutes[i].Visible) // chicken dead
                {
                    chickenParachutes.RemoveAt(i--);
                    continue;
                }
                else if (!ship.IsDeading && chickenParachutes[i].Visible && !ship.IsProtect)
                {                   
                    if (rect1.Intersects(chickenParachutes[i].Bounds) || rect2.Intersects(chickenParachutes[i].Bounds))
                    {
                        scores.ScoresPlay += chickenParachutes[i].Attacked(ship);
                        return true;
                    }
                }
            }
            return false;
        }
        private void ShipCollisionGive()
        {
            for (int i = 0; i < gifts.Count; i++)
            {
                if (!gifts[i].Visible) // chicken dead
                {
                    gifts.RemoveAt(i--);
                    continue;
                }
                else if(!ship.IsDeading)
                {
                    Rectangle rect1 = new Rectangle(ship.Bounds.Location + new Point(32, 5), new Point(13, 33));
                    Rectangle rect2 = new Rectangle(ship.Bounds.Location + new Point(10, 37), new Point(62, 35));
                    if (rect1.Intersects(gifts[i].Bounds) || rect2.Intersects(gifts[i].Bounds))
                    {
                        gifts[i].Visible = false;
                        ship.Level += 1;
                        gifts[i].SoundPlay();
                        gifts.RemoveAt(i--);
                        
                        continue;
                    }
                }
            }
        }

        #endregion

        #region Ship
        private void UpdateShip(GameTime gameTime)
        {
            ship.Update(gameTime);
            if(ShipCollision() == true)
            {
                isShipDie = true;
                ship.Level /= 2;
                ship.Dead();
                ship.SoundDeadPlay();
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

        #region Chicken Parachute
        private void UpdateChickenParachutes(GameTime gameTime)
        {
            for (int i = 0; i < chickenParachutes.Count; i++)
            {
                if (!chickenParachutes[i].Visible) // chicken dead
                {
                    chickenParachutes.RemoveAt(i--);
                    continue;
                }
                chickenParachutes[i].Update(gameTime);
                if (chickenParachutes[i].Visible)
                    BulletCollision(chickenParachutes[i]);
            }
        }
        private void DrawChickenParachutes(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < chickenParachutes.Count; i++)
            {
                chickenParachutes[i].Draw(gameTime, spriteBatch, Color.White);
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

        #region Scores
        private void UpdateScores(GameTime gameTime)
        {
            scores.Update(gameTime);
        }
        private void DrawScores(GameTime gameTime, SpriteBatch spriteBatch)
        {
            scores.Draw(gameTime, spriteBatch);
        }
        #endregion

        #region Gives
        private void UpdateGives(GameTime gameTime)
        {
            for (int i = 0; i < gifts.Count; i++)
                gifts[i].Update(gameTime);
        }
        private void DrawGives(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < gifts.Count; i++)
                gifts[i].Draw(gameTime, spriteBatch, Color.White);
        }
        
        #endregion

        #region Map 1
        private void UpdateMap1(GameTime gameTime)
        {
            map1.Update(gameTime);
        }
        #endregion

        #region Map2
        private void UpdateMap2(GameTime gameTime)
        {
            map2.Update(gameTime);
        }
        #endregion

        #region Button
        //private void DrawButtons(GameTime gameTime, SpriteBatch spriteBatch)
        //{
        //    for (int i = 0; i < buttons.Count; i++)
        //    {
        //        buttons[i].Draw(spriteBatch, Color.White);
        //    }
        //}
        //private void UpdateButtons(GameTime gameTime)
        //{
        //    for (int i = 0; i < buttons.Count; i++)
        //    {
        //        buttons[i].isMouseHover();
        //    }
        //}
        #endregion

        #region Animation shake Screen when ship die
        private void ScreenShake(GameTime gameTime)
        {
            timeShake += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            totalTimeShake += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (totalTimeShake > 750)
            {
                isShipDie = false;
                totalTimeShake = 0;
                screen.X = 0;
                screen.Y = 0;
                return;
            }
            else
            if (timeShake > 20)
            {
                timeShake = 0;
                if (screen.Top > -15 && isTop == false)
                    screen.Y -= 4;
                else
                    isTop = true;
                if (screen.Bottom < 987 && isTop == true)
                    screen.Y += 4;
                else
                    isTop = false;

                if (screen.Left > -10 && isLeft == false)
                    screen.X -= 2;
                else
                    isLeft = true;

                if (screen.Right < 634 && isLeft == true)
                    screen.X += 2;
                else
                    isLeft = false;
            }
        }

        #endregion

        #region End Game
        private void DrawLose(GameTime gameTime, SpriteBatch spriteBatch)
        {
            timeEnd += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if(timeEnd < 7)
            {
                spriteBatch.Draw(skinEndGame, rectEndGame, Color.White);
            }
            else
            {
                timeEnd = 0;
                this.visible = false;
            }
        }
        #endregion

        #region Recycle
        public void Recycle()
        {
            this.IsPlay = true;
            ship.Bullets.Clear();
            chickens.Clear();
            chickenParachutes.Clear();
            
            gifts.Clear();
            eggs.Clear();
            scores.ScoresPlay = 0;
            ship.Heart = 3;
            map1 = null;
            map2 = null;
            map1 = new Map1(content, index, chickens, IsPlay, screen);
            map2 = new Map2(content, index, chickenParachutes, IsPlay, screen);
            currentMap = 1;
            this.visible = true;
            this.isWin = false;
            this.alreadySet = false;
            this.timeEnd = 0;
            ship.Level = 1;
            MediaPlayer.Play(song);
        }
        #endregion

        #region Destructor
        ~GamePlay()
        {
            Unload();
        }
        #endregion
    }
}
