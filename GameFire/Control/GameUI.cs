using GameFire.button;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace GameFire
{
    public enum Buttons
    {
        Play = 0,
        Pause = 1,
        Back = 2,
        Help = 3,
        Again = 4,
        Setting = 5,
        MusicOn = 6,
        MusicOff= 7,
        Info = 8,
        Cancel = 9
    }
    internal class GameUI
    {
        #region Properties
        private bool isFullHD;
        private List<Button> buttons;
        private Texture2D skin;
        private Texture2D logo;
        private Rectangle desRectLogo;
        private Vector2 index;
        private float timeSleep = 0f;
        private bool visible;
        private SoundEffect soundClick;

        public bool Visible
        {
            get => visible;
            set => visible = value;
        }
        public List<Button> Buttons { get => buttons; private set => buttons = value; }
        #endregion

        #region Constructor
        public GameUI(ContentManager content, Vector2 index, bool isFullHD)
        {
            Buttons = new List<Button>();
            this.index = index;
            this.isFullHD = isFullHD;
            visible = true;
            Load(content);
            Initialize();
        }
        #endregion

        #region Load & unload
        private void Load(ContentManager content)
        {
            skin = content.Load<Texture2D>("button/4");
            logo = content.Load<Texture2D>("logo");
            soundClick = content.Load<SoundEffect>("Music/Quit");
        }
        private void UnLoad()
        {
            buttons.Clear();
            buttons = null;
            skin = null;
            logo = null;
            soundClick = null;
        }
        #endregion

        #region Method
        public void Draw(SpriteBatch spriteBatch)
        {
            if (Buttons != null && Visible == true)
            {
                foreach (var item in Buttons)
                    item.Draw(spriteBatch);
                spriteBatch.Draw(logo, desRectLogo, Color.White);
            }

        }
        public void Update(GameTime time)
        {
            if (Visible == true)
                for (int i = 0; i < buttons.Count; i++)
                {
                    if (buttons[i].Visible == true)
                    {
                        bool isMouserHover = buttons[i].isMouseHover();
                        if (isMouserHover == true && buttons[i].IsMouseClick == true)
                        {
                            timeSleep += (float)time.ElapsedGameTime.TotalMilliseconds;
                            if (timeSleep >= 65.0f)
                            {
                                soundClick.Play();
                                Mouse.SetPosition((int)index.X * 50, (int)index.Y * 80);
                                this.Visible = false;
                                break;
                            }
                        }
                    }
                }
        }
        private void Initialize()
        {
            float scale = 1;
            //=======================================
            //Button Play
            //=======================================
            #region Button play
            Rectangle playRectangle = new Rectangle();
            playRectangle.X = (int)(37.5 * index.X);
            playRectangle.Y = (int)(50 * index.Y);
            playRectangle.Width = (int)((skin.Width * scale) / 10 / 1.2);
            playRectangle.Height = (int)((skin.Height * scale) / 1.2);
            Button btnPlay = new Button(skin, playRectangle, (int)GameFire.Buttons.Play);
            #endregion
            //=======================================
            //Button Pause
            //=======================================
            #region Button Pause
            Rectangle pauseRectangle = new Rectangle();
            pauseRectangle.X = (int)(90 * index.X);
            pauseRectangle.Y = (int)(0 * index.Y);
            pauseRectangle.Width = (int)((skin.Width * scale) / 10)/ 3;
            pauseRectangle.Height = (int)((skin.Height * scale) / 3);
            Button btnPause = new Button(skin, pauseRectangle, (int)GameFire.Buttons.Pause, false);
            #endregion
            //=======================================
            //Button Back
            //=======================================
            #region Button Back
            //Rectangle backRectangle = new Rectangle();
            //backRectangle.X = (int)(index.X * 50);
            //backRectangle.Y = (int)(index.Y * 50);
            //backRectangle.Width = (int)(skin.Width * scale) / 10;
            //backRectangle.Height = (int)(skin.Height * scale);
            //Button btnBack = new Button(skin, backRectangle, (int)GameFire.Buttons.Back, false);
            #endregion
            //=======================================
            //Button Help
            //=======================================
            #region Button Help
            //Rectangle helpRectangle = new Rectangle();
            //helpRectangle.X = (int)(50 * index.X);
            //helpRectangle.Y = (int)(80 * index.Y);
            //helpRectangle.Width = (int)(skin.Width * scale) / 10;
            //helpRectangle.Height = (int)(skin.Height * scale);
            //Button btnHelp = new Button(skin, helpRectangle, (int)GameFire.Buttons.Help, false);
            #endregion
            //=======================================
            //Button Again
            //=======================================
            #region Button Again
            //Rectangle againRectangle = new Rectangle();
            //againRectangle.X = (int)(index.X);
            //againRectangle.Y = (int)(index.Y);
            //againRectangle.Width = (int)(skin.Width * scale) / 10;
            //againRectangle.Height = (int)(skin.Height * scale);
            //Button btnAgain = new Button(skin, againRectangle, (int)GameFire.Buttons.Again, false);
            #endregion
            //=======================================
            //Button Setting
            //=======================================
            #region Button Setting
            //Rectangle settingRectangle = new Rectangle();
            //settingRectangle.X = (int)(37.5 * index.X);
            //settingRectangle.Y = (int)(70 * index.Y);
            //settingRectangle.Width = (int)((skin.Width * scale) / 10 / 1.2);
            //settingRectangle.Height = (int)((skin.Height * scale) / 1.2);
            //Button btnSetting = new Button(skin, settingRectangle, (int)GameFire.Buttons.Setting, false);
            #endregion
            //=======================================
            //Button Music On
            //=======================================
            #region Button Music On
            //Rectangle musicOnRectangle = new Rectangle();
            //musicOnRectangle.X = (int)(index.X) / 2;
            //musicOnRectangle.Y = (int)(index.Y) / 2 - 200;
            //musicOnRectangle.Width = (int)(skin.Width * scale) / 10;
            //musicOnRectangle.Height = (int)(skin.Height * scale);
            //Button btnMusicOn = new Button(skin, musicOnRectangle, (int)GameFire.Buttons.MusicOn, false);
            #endregion
            //=======================================
            //Button Music Off
            //=======================================
            #region Button Music Off
            //Rectangle musicOffRectangle = new Rectangle();
            //musicOffRectangle.X = (int)(index.X) / 2;
            //musicOffRectangle.Y = (int)(index.Y) / 2 - 200;
            //musicOffRectangle.Width = (int)(skin.Width * scale) / 10;
            //musicOffRectangle.Height = (int)(skin.Height * scale);
            //Button btnMusicOff = new Button(skin, musicOffRectangle, (int)GameFire.Buttons.MusicOff, false);
            #endregion
            //=======================================
            //Button Info
            //=======================================
            #region Button Info
            //Rectangle infoRectangle = new Rectangle();
            //infoRectangle.X = (int)(50 * index.X) / 2;
            //infoRectangle.Y = (int)(90 * index.Y) / 2 - 200;
            //infoRectangle.Width = (int)(skin.Width * scale) / 10;
            //infoRectangle.Height = (int)(skin.Height * scale);
            //Button btnInfo = new Button(skin, infoRectangle, (int)GameFire.Buttons.Info, false);
            #endregion
            //=======================================
            //Button Cancel
            //=======================================
            #region Button Cancel
            //Rectangle cancelRectangle = new Rectangle();
            //cancelRectangle.X = (int)(index.X) / 2;
            //cancelRectangle.Y = (int)(index.Y) / 2;
            //cancelRectangle.Width = (int)(skin.Width * scale) / 10;
            //cancelRectangle.Height = (int)(skin.Height * scale);
            //Button btnCancel = new Button(skin, cancelRectangle, (int)GameFire.Buttons.Cancel, false);
            #endregion
            //=======================================
            //Add Buttons
            //=======================================
            #region Add Buttons
            Buttons.Add(btnPlay);
            Buttons.Add(btnPause);
            //Buttons.Add(btnBack);
            //Buttons.Add(btnHelp);
            //Buttons.Add(btnAgain);
            //Buttons.Add(btnSetting);
            //Buttons.Add(btnMusicOn);
            //Buttons.Add(btnMusicOff);
            //Buttons.Add(btnInfo);
            //Buttons.Add(btnCancel);
            #endregion
            //=======================================
            //Logo
            //=======================================
            if (isFullHD)
                desRectLogo = new Rectangle((int)index.X * 15, (int)index.Y * 15, 440, 240);
            else
            {
                desRectLogo = new Rectangle((int)index.X * 2, (int)index.Y * 15, 440, 240);            
            }
        }
        #endregion

        #region Destructor
        ~GameUI()
        {
            UnLoad();
        }
        #endregion
    }
}
