using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FinalGame.Collisions;
using Microsoft.Xna.Framework.Input;

namespace FinalGame.Entities
{
    public class Wall
    {
        public Texture2D Texture;
        public Vector2 Position { get; set; }
        public bool Rotation { get; set; }

        public BoundingRectangle Bounds;

        public Wall(Vector2 position, bool rotation, int l)
        {
            Position = position;
            Rotation = rotation;
            if (rotation) Bounds = new BoundingRectangle(position.X, Position.Y, 30, l);
            else Bounds = new BoundingRectangle(position.X, Position.Y, l, 30);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, new Rectangle((int)Bounds.X, (int)Bounds.Y, (int)Bounds.Width, (int)Bounds.Height),
            null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
        }
    }
}
