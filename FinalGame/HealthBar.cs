using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalGame
{
    public class HealthBar
    {
        public Texture2D Texture;
        public Vector2 PositionDefault = new Vector2(10, 10);
        public Vector2 Position;
        public int boxWidth = 20;
        public int boxHeight = 20;
        public int spacing = 4;

        public void Draw(SpriteBatch spriteBatch, int health)
        {
            Position = PositionDefault;
            for(int i = 0; i < health; i++)
            {
                spriteBatch.Draw(Texture, new Rectangle((int)Position.X, (int)Position.Y, boxWidth, boxHeight), Color.Red);
                Position += new Vector2(boxWidth + spacing, 0);
            }
        }
    }
}
