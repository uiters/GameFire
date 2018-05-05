using GameFire.button;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
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
    public class GameUI
    {
        #region Properties
        protected List<Button> _buttons;
        protected Texture2D _skin;
        protected Texture2D _logo;
        protected Rectangle _desRectLogo;
        protected Vector2 _index;
        private float timeSleep = 0f;
        protected bool _visible;
        public bool Visible
        {
            get => _visible;
            set => _visible = value;
        }
        public List<Button> Buttons { get => _buttons; private set => _buttons = value; }
        #endregion

        #region Constructor
        public GameUI(ContentManager content, Vector2 index)
        {
            Buttons = new List<Button>();
            _index = index;
            _visible = true;
            Load(content);
            Initialize();
        }
        #endregion

        #region Destructor
        ~GameUI()
        {
            UnLoad();
        }
        #endregion

        #region Method
        private void Load(ContentManager content)
        {
            _skin = content.Load<Texture2D>("button/4");
            _logo = content.Load<Texture2D>("logo");
        }
        private void UnLoad()
        {
            this.Buttons.Clear();
            _skin = null;
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            if (Buttons != null && Visible == true)
            {
                foreach (var item in Buttons)
                    item.Draw(spriteBatch, color);
                spriteBatch.Draw(_logo, _desRectLogo, color);
            }

        }
        public void Update(GameTime time)
        {
            MouseState mouse = Mouse.GetState();
            if (Visible == true)
                foreach (var item in Buttons)
                {
                    bool isMouserHover = item.isMouseHover(mouse);
                    if (isMouserHover == true && item.IsMouseClick == true)
                    {
                        timeSleep += (float)time.ElapsedGameTime.TotalMilliseconds;
                        if (timeSleep >= 65.0f)
                        {
                            Mouse.SetPosition((int)_index.X * 50, (int)_index.Y * 80);
                            this.Visible = false;
                            break;
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
            playRectangle.X = (int)(35 * _index.X);
            playRectangle.Y = (int)(50 * _index.Y);
            playRectangle.Width = (int)(_skin.Width * scale) / 10;
            playRectangle.Height = (int)(_skin.Height * scale);
            Button btnPlay = new Button(_skin, playRectangle, (int)GameFire.Buttons.Play);
            #endregion
            //=======================================
            //Button Pause
            //=======================================
            #region Button Pause
            Rectangle pauseRectangle = new Rectangle();
            pauseRectangle.X = (int)(50 * _index.X);
            pauseRectangle.Y = (int)(60 * _index.Y);
            pauseRectangle.Width = (int)(_skin.Width * scale) / 10;
            pauseRectangle.Height = (int)(_skin.Height * scale);
            Button btnPause = new Button(_skin, pauseRectangle, (int)GameFire.Buttons.Pause, false);
            #endregion
            //=======================================
            //Button Back
            //=======================================
            #region Button Back
            Rectangle backRectangle = new Rectangle();
            backRectangle.X = (int)(_index.X * 50);
            backRectangle.Y = (int)(_index.Y * 50);
            backRectangle.Width = (int)(_skin.Width * scale) / 10;
            backRectangle.Height = (int)(_skin.Height * scale);
            Button btnBack = new Button(_skin, backRectangle, (int)GameFire.Buttons.Back, false);
            #endregion
            //=======================================
            //Button Help
            //=======================================
            #region Button Help
            Rectangle helpRectangle = new Rectangle();
            helpRectangle.X = (int)(50 * _index.X);
            helpRectangle.Y = (int)(80 * _index.Y);
            helpRectangle.Width = (int)(_skin.Width * scale) / 10;
            helpRectangle.Height = (int)(_skin.Height * scale);
            Button btnHelp = new Button(_skin, helpRectangle, (int)GameFire.Buttons.Help, false);
            #endregion
            //=======================================
            //Button Again
            //=======================================
            #region Button Again
            Rectangle againRectangle = new Rectangle();
            againRectangle.X = (int)(_index.X);
            againRectangle.Y = (int)(_index.Y);
            againRectangle.Width = (int)(_skin.Width * scale) / 10;
            againRectangle.Height = (int)(_skin.Height * scale);
            Button btnAgain = new Button(_skin, againRectangle, (int)GameFire.Buttons.Again, false);
            #endregion
            //=======================================
            //Button Setting
            //=======================================
            #region Button Setting
            Rectangle settingRectangle = new Rectangle();
            settingRectangle.X = (int)(50 * _index.X) / 2;
            settingRectangle.Y = (int)(70 * _index.Y) / 2 - 200;
            settingRectangle.Width = (int)(_skin.Width * scale) / 10;
            settingRectangle.Height = (int)(_skin.Height * scale);
            Button btnSetting = new Button(_skin, settingRectangle, (int)GameFire.Buttons.Setting, false);
            #endregion
            //=======================================
            //Button Music On
            //=======================================
            #region Button Music On
            Rectangle musicOnRectangle = new Rectangle();
            musicOnRectangle.X = (int)(_index.X) / 2;
            musicOnRectangle.Y = (int)(_index.Y) / 2 - 200;
            musicOnRectangle.Width = (int)(_skin.Width * scale) / 10;
            musicOnRectangle.Height = (int)(_skin.Height * scale);
            Button btnMusicOn = new Button(_skin, musicOnRectangle, (int)GameFire.Buttons.MusicOn, false);
            #endregion
            //=======================================
            //Button Music Off
            //=======================================
            #region Button Music Off
            Rectangle musicOffRectangle = new Rectangle();
            musicOffRectangle.X = (int)(_index.X) / 2;
            musicOffRectangle.Y = (int)(_index.Y) / 2 - 200;
            musicOffRectangle.Width = (int)(_skin.Width * scale) / 10;
            musicOffRectangle.Height = (int)(_skin.Height * scale);
            Button btnMusicOff = new Button(_skin, musicOffRectangle, (int)GameFire.Buttons.MusicOff, false);
            #endregion
            //=======================================
            //Button Info
            //=======================================
            #region Button Info
            Rectangle infoRectangle = new Rectangle();
            infoRectangle.X = (int)(50 * _index.X) / 2;
            infoRectangle.Y = (int)(90 * _index.Y) / 2 - 200;
            infoRectangle.Width = (int)(_skin.Width * scale) / 10;
            infoRectangle.Height = (int)(_skin.Height * scale);
            Button btnInfo = new Button(_skin, infoRectangle, (int)GameFire.Buttons.Info, false);
            #endregion
            //=======================================
            //Button Cancel
            //=======================================
            #region Button Cancel
            Rectangle cancelRectangle = new Rectangle();
            cancelRectangle.X = (int)(_index.X) / 2;
            cancelRectangle.Y = (int)(_index.Y) / 2;
            cancelRectangle.Width = (int)(_skin.Width * scale) / 10;
            cancelRectangle.Height = (int)(_skin.Height * scale);
            Button btnCancel = new Button(_skin, cancelRectangle, (int)GameFire.Buttons.Cancel, false);
            #endregion
            //=======================================
            //Add Buttons
            //=======================================
            #region Add Buttons
            Buttons.Add(btnPlay);
            Buttons.Add(btnPause);
            Buttons.Add(btnBack);
            Buttons.Add(btnHelp);
            Buttons.Add(btnAgain);
            Buttons.Add(btnSetting);
            Buttons.Add(btnMusicOn);
            Buttons.Add(btnMusicOff);
            Buttons.Add(btnInfo);
            Buttons.Add(btnCancel);
            #endregion
            //=======================================
            //Logo
            //=======================================
            _desRectLogo = new Rectangle((int)_index.X * 15, (int)_index.Y * 15, 440, 240);

        }
        #endregion
    }
}
