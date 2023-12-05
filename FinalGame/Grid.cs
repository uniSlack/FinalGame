using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalGame.Entities;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace FinalGame
{
    public class Grid
    {
        public Texture2D GridTexture;
        public Texture2D BlurTexture;

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color)
        {
            Vector2 tempPosition = position - new Vector2(BlurTexture.Height / 2, BlurTexture.Width / 2) ;
            spriteBatch.Draw(GridTexture, tempPosition,
                new Rectangle((int)position.X, (int)position.Y, BlurTexture.Height, BlurTexture.Width),
                color, 0,new Vector2(0), 1, SpriteEffects.None, 0);
            spriteBatch.Draw(BlurTexture, tempPosition, null, Color.White, 0,
                new Vector2(0), 1, SpriteEffects.None, 0);
        }
    }
}
