using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalGame.Entities;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FinalGame.StateManagement;
using System.Windows.Forms;

namespace FinalGame
{
    public class Grid
    {
        public Texture2D GridTexture;
        public Texture2D BlurTexture;
        public Texture2D TempTexture;
        //Color[] blur;
        //Color[] grid;


        public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color)
        {
            Vector2 tempPosition = position - new Vector2(BlurTexture.Height / 2, BlurTexture.Width / 2) ;
            spriteBatch.Draw(GridTexture, tempPosition,
                new Rectangle((int)position.X, (int)position.Y, BlurTexture.Height, BlurTexture.Width),
                color, 0,new Vector2(0), 1, SpriteEffects.None, 0);
            spriteBatch.Draw(BlurTexture, tempPosition, null, Color.White, 0,
                new Vector2(0), 1, SpriteEffects.None, 0);
        }

        public void Draw2(SpriteBatch spriteBatch, Vector2 position, Color color, Texture2D blankTexture)
        {
            position /= Constants.Scale;
            int w = blankTexture.Width;
            int h = blankTexture.Height;
            Vector2 tempPosition = position - new Vector2(w / 2, h / 2);
            Color[] blur = new Color[w * h];
            BlurTexture.GetData<Color>(0, 0, new Rectangle(0, 0, w, h), blur, 0, blur.Count());
            Color[] grid = new Color[w *h];
            
            GridTexture.GetData<Color>(0, 0, new Rectangle((int)position.X, (int)position.Y, w, h), grid, 0, grid.Count());
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    grid[(i * 200) + j].A = (byte)(255 - blur[(i * 200) + j].A);
                }
            }

            blankTexture.SetData<Color>(grid);
            spriteBatch.Draw(blankTexture, position * Constants.Scale, null, color, 0,
                new Vector2(w / 2, h / 2), 1, SpriteEffects.None, 0);
        }
    }
}
