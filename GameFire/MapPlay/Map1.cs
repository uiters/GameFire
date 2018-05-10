using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFire.Enemy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace GameFire.MapPlay
{
    public class Map1 : Map
    {
        #region Properties
        private readonly int maxChiken = 10;
        #endregion

        #region Constructor
        public Map1(ContentManager content, Vector2 index, List<Chicken> chickens) : base(content, index)
        {
            _chickens = chickens;
            Initialize();
        }

        #endregion

        #region Update
        public override void Update(GameTime gameTime)
        {

        }
        #endregion



        #region Initialize
        internal override void Initialize()
        {
            Vector2 speed = Vector2.One;

            //--------------------------------
            // Chicken row 1
            //--------------------------------
            for (int i = 0; i < maxChiken; i++)
            {
                Rectangle location = new Rectangle(new Point(-10 * (int)_index.X, 10 * (int)_index.Y), new Point(47, 38));
                Chicken chicken = new Chicken(_content, speed, _index, location, TypeChiken.ChickenGreen, 1);
                _chickens.Add(chicken);
            }


            //--------------------------------
            // Chicken row 2
            //--------------------------------
            for (int i = 0; i < maxChiken; i++)
            {
                Rectangle location = new Rectangle(new Point(110 * (int)_index.X, 20 * (int)_index.Y), new Point(47, 38));
                Chicken chicken = new Chicken(_content, speed, _index, location, TypeChiken.ChickenGreen, 1);
                _chickens.Add(chicken);
            }


            //--------------------------------
            // Chicken row 3
            //--------------------------------
            for (int i = 0; i < maxChiken; i++)
            {
                Rectangle location = new Rectangle(new Point(-10 * (int)_index.X, 30 * (int)_index.Y), new Point(47, 38));
                Chicken chicken = new Chicken(_content, speed, _index, location, TypeChiken.ChickenGreen, 1);
                _chickens.Add(chicken);
            }


            //--------------------------------
            // Chicken row 4
            //--------------------------------
            for (int i = 0; i < maxChiken; i++)
            {
                Rectangle location = new Rectangle(new Point(110 * (int)_index.X, 40 * (int)_index.Y), new Point(47, 38));
                Chicken chicken = new Chicken(_content, speed, _index, location, TypeChiken.ChickenGreen, 1);
                _chickens.Add(chicken);
            }

            //--------------------------------
            // Chicken row 5
            //--------------------------------
            for (int i = 0; i < maxChiken; i++)
            {
                Rectangle location = new Rectangle(new Point(110 * (int)_index.X, 50 * (int)_index.Y), new Point(47, 38));
                Chicken chicken = new Chicken(_content, speed, _index, location, TypeChiken.ChickenGreen, 1);
                _chickens.Add(chicken);
            }
        }

        #endregion
    }
}
