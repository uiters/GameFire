using GameFire.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace GameFire.Enemy
{
    public enum TypeChiken
    {
        ChickenGreen = 1,
        ChickenRed = 2,
        ChickenParachuteGreen = 1,
        ChickenParachuteRed = 2,
        BossChickenRed = 3

    }
    public class Chicken : GameObject
    {
        #region Properties
        /// <summary>
        /// Loại chicken
        /// </summary>
        private TypeChiken type;
        /// <summary>
        /// Khung hình hiện tại
        /// </summary>
        private sbyte indexNow;
        /// <summary>
        /// Đang bị tấn công không?
        /// </summary>
        private bool isAttacked;

        /// <summary>
        /// Hình ảnh khi chết
        /// </summary>
        private Texture2D[] textureDies;
        /// <summary>
        /// Hình ảnh khi chết
        /// </summary>
        private Rectangle[] desRectDies;
        /// <summary>
        /// Hình ảnh của một khung hình
        /// </summary>
        private Rectangle sourceRectSkin;

        /// <summary>
        /// Âm thanh phát  khi chết
        /// </summary>
        private SoundEffect soundDying;
        /// <summary>
        /// Âm thanh phát ra khi đẻ trứng
        /// </summary>
        private SoundEffect soundLaying;
        /// <summary>
        /// Âm thanh phát ra khi bị đánh
        /// </summary>
        private SoundEffect soundHurt;

        public Point Location
        {
            get => _desRectSkin.Location;
            set => _desRectSkin.Location = value;
        }
        public bool IsAlive { get => _heart > 0; }
        public Vector2 Speed { get => _speed; set => _speed = value; }
        public object Tag { get; set; }
        public override Rectangle Bounds
        {
            get
            {
                Rectangle rectangle = new Rectangle(_desRectSkin.Location + new Point(5, 5), _desRectSkin.Size - new Point(10, 10));
                return rectangle;
            }
        }
        #endregion

        #region Constructor
        public Chicken(ContentManager content, Vector2 speed, Vector2 index, Rectangle location, TypeChiken type, float heart) : base(content, speed, index, location)
        {
            this.type = type;
            this._heart = heart;
            indexNow =(sbyte) _random.Next(0, 19);
            _totalTime = 0.0f;
            _minScores = (int)((int)type * _heart * 100) + 100;
            this.Load();
        }
        #endregion

        #region Load & unLoad
        /// <summary>
        /// Tải hình ảnh
        /// </summary>
        protected override void Load()
        {
            Texture2D textureDie;
            switch ((int)type)
            {
                case 1:
                    _skin = _content.Load<Texture2D>("Enemy/chickenGreen");
                    if (_desRectSkin.Size == Point.Zero)
                    {
                        _desRectSkin.Width = 47;
                        _desRectSkin.Height = _skin.Height;
                    }
                    sourceRectSkin = new Rectangle(47 * indexNow, 0, 47, _skin.Height);
                    break;
                case 2:
                    _skin = _content.Load<Texture2D>("Enemy/chickenRed");
                    if (_desRectSkin.Size == Point.Zero)
                    {
                        _desRectSkin.Width = 40;
                        _desRectSkin.Height = _skin.Height;
                    }
                    sourceRectSkin = new Rectangle(40 * indexNow, 0, 40, _skin.Height);
                    break;
                case 3:
                    _skin = _content.Load<Texture2D>("Enemy/bossRed");
                    if (_desRectSkin.Size == Point.Zero)
                    {
                        _desRectSkin.Width = 75;
                        _desRectSkin.Height = 68;
                    }
                    sourceRectSkin = new Rectangle(75 * indexNow, 0, 75, _skin.Height);
                    break;
                default:
                    break;
            }
            textureDie = _content.Load<Texture2D>("Enemy/dead");
            Rectangle desRectDie = new Rectangle(Point.Zero, new Point(32, 32));
            //animation die
            this.textureDies = new Texture2D[] { textureDie, textureDie, textureDie, textureDie, textureDie };
            this.desRectDies = new Rectangle[] { desRectDie, desRectDie, desRectDie, desRectDie, desRectDie };
            LoadSound();
        }

        /// <summary>
        /// Tải âm thành
        /// </summary>
        private void LoadSound()
        {
            switch (_random.Next(0, 5))
            {
                case 0:
                    soundDying = _content.Load<SoundEffect>("Music/chicken/Chicken_death1");
                    break;
                case 1:
                    soundDying = _content.Load<SoundEffect>("Music/chicken/Chicken_death2");
                    break;
                case 3:
                    soundDying = _content.Load<SoundEffect>("Music/chicken/Chicken_death3");
                    break;
                default:
                    soundDying = _content.Load<SoundEffect>("Music/chicken/Chicken_death4");
                    break;
            }
            switch (_random.Next(0, 4))
            {
                case 0:
                    soundHurt = _content.Load<SoundEffect>("Music/chicken/Chicken_hurt1");
                    break;
                case 1:
                    soundHurt = _content.Load<SoundEffect>("Music/chicken/Chicken_hurt2");
                    break;
                default:
                    soundHurt = _content.Load<SoundEffect>("Music/chicken/Chick_hurt1");
                    break;
            }
            soundLaying = _content.Load<SoundEffect>("Music/chicken/Chicken_lay");
        }

        /// <summary>
        /// Giải phóng tài nguyên
        /// </summary>
        protected override void Unload()
        {
            for (int i = 0; i < desRectDies.Length; i++)
            {
                textureDies[i] = null;
            }
            soundHurt = null;
            soundLaying = null;
            soundDying = null;
            base.Unload();
        }
        #endregion

        #region Method
        /// <summary>
        /// Cập nhật vị trí
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            if (!Visible)
                return;
            _timeLive += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_heart > 0)
            {
                AnimationFly(gameTime);
                if (isAttacked)
                {
                    _desRectSkin.Location = new Point(_desRectSkin.X, _desRectSkin.Y + 4);
                    isAttacked = false;
                }
            }
            else
            {
                for (int i = 0; i < desRectDies.Length; i++)
                {
                    if (desRectDies[i].Size.X <= 0 || desRectDies[i].Y <= 0)
                    {
                        Visible = false;
                    }
                    else
                    {
                        desRectDies[i] = new Rectangle(desRectDies[i].Location + new Point(2, 2), desRectDies[i].Size - new Point(4, 4));
                    }
                }

            }
        }
        /// <summary>
        /// Hàm vẽ lại
        /// </summary>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Color color)
        {
            if (!Visible)
            {
                return;
            }
            if (_heart > 0)
            {
                spriteBatch.Draw(_skin, _desRectSkin, sourceRectSkin, color);
            }
            else
            {
                for (int i = 0; i < textureDies.Length; i++)
                {
                    spriteBatch.Draw(textureDies[i], desRectDies[i], color);
                }
            }
        }
        #endregion

        #region Method Chicken
        /// <summary>
        /// Bị tấn công bằng đạn. Tiêu diệt cho điểm số.
        /// </summary>
        /// <param name="bullet"></param>
        /// <returns></returns>
        public int Attacked(Bullet bullet)
        {
            bullet.Visible = false;
            this._heart -= bullet.Damage;
            return GetScores();
        }

        /// <summary>
        /// Tấn công người chơi. Tiêu diệt cho điểm số
        /// </summary>
        public int Attacked(Ship ship)
        {
            this._heart -= 1.0f;
            return GetScores();
        }

        /// <summary>
        /// Nhận điểm khi tiêu diệt
        /// </summary>
        private int GetScores()
        {
            if (_heart <= 0.0f)
            {
                soundDying.Play(0.875f, 0, 0);
                for (int i = 0; i < desRectDies.Length; i++)
                {
                    Point randomLocation = new Point(_random.Next(-15, 15), _random.Next(-15, 15));
                    desRectDies[i].Location = _desRectSkin.Location + randomLocation;
                }
                return Scores;
            }
            else
            {
                soundHurt.Play(0.875f, 0, 0);
                isAttacked = true;
                _desRectSkin.Location = new Point(_desRectSkin.X, _desRectSkin.Y - 4);
                return 0;
            }
        }

        /// <summary>
        /// Hiệu ứng bay
        /// </summary>
        private void AnimationFly(GameTime gameTime)
        {
            _totalTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (_totalTime >= 70.0f)
            {
                indexNow = (++indexNow >= 19) ? (sbyte)0 : indexNow;
                sourceRectSkin.X = sourceRectSkin.Width * ((indexNow > 9) ? 19 - indexNow : indexNow);
                _totalTime = 0.0f;
            }
        }

        /// <summary>
        /// Đẻ trứng
        /// </summary>
        public Egg CreateEgg()
        {
            if (_random.Next(0, 750) == 100)
            {
                soundLaying.Play(0.65f, 0, 0);
                Point sizeEgg;
                int sizeX;
                if (type == TypeChiken.BossChickenRed)
                {
                    sizeEgg = new Point(25, 25);
                }
                else
                {
                    sizeX = 2 * _desRectSkin.Width / 5;
                    sizeEgg = new Point(sizeX, sizeX);
                }
                return new Egg(_content, new Vector2(0, _random.Next(2, 10)), _index, new Rectangle(_desRectSkin.Center + new Point(-sizeEgg.X / 2, 0), sizeEgg), this.type);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Tạo ra quà khi chết
        /// </summary>
        public Gift CreateGif()
        {

            if (_random.Next(0, 75) == 15)
                return new Gift(_content, new Vector2(0, _random.Next(2, 6)), _index, new Rectangle(Bounds.Location, Point.Zero));
            else
                return null;
        }
        #endregion

        #region destructor
        /// <summary>
        /// Huỷ bỏ thể hiện
        /// </summary>
        ~Chicken()
        {
            Unload();
        }
        #endregion
    }
}
