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
        public Texture2D Texture;

        public void Draw(SpriteBatch spriteBatch, Player player)
        {
            Vector2 tempPosition = player.Position - new Vector2(Texture.Height/2,Texture.Width/2);
            spriteBatch.Draw(Texture, tempPosition, null, Color.Blue, 0,
                new Vector2(0), 1, SpriteEffects.None, 0);
        }
    }
}
