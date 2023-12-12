using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FinalGame.Collisions;
using Microsoft.Xna.Framework.Input;
using System.Security.Policy;

namespace FinalGame.Entities
{
    public class TeleportGrenade
    {
        public Texture2D texture;
        public Vector2 Position { get; set; }
        public BoundingCircle Bounds;
        public Vector2 Velocity { get; set; }
        public bool Fired = false;
        public Player Player;

        int radius = 5;

        public void FireGrenade(Vector2 position, Vector2 velocity)
        {
            Bounds = new BoundingCircle(position, radius);
            Position = position;
            Velocity = velocity;
            Fired = true;
        }

        public void Update(GameTime gameTime, List<Wall> walls)
        {
            foreach (Wall w in walls)
            {
                if (Bounds.CollidesWith(w.Bounds))
                {
                    float nearestX = Math.Abs(Position.X - MathHelper.Clamp(Position.X, w.Bounds.Left, w.Bounds.Right));
                    float nearestY = Math.Abs(Position.Y - MathHelper.Clamp(Position.Y, w.Bounds.Top, w.Bounds.Bottom));
                    if (nearestX > nearestY)
                    {
                        Position -= Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        Velocity *= new Vector2(-1, 1);
                    }
                    else
                    {
                        Position -= Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        Velocity *= new Vector2(1, -1);
                    }
                }
            }

            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Bounds.Center = Position;

            if (Position.X < radius || Position.X > Constants.DISPLAY_WIDTH - radius)
            {
                Position -= Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                Velocity *= new Vector2(-1, 1);

            }

            if (Position.Y < radius || Position.Y > Constants.DISPLAY_HEIGHT - radius)
            {
                Position -= Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                Velocity *= new Vector2(1, -1);
            }

            Velocity /= 1.01f;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //TODO change color based on if you can tele?
            spriteBatch.Draw(texture, Bounds.Center, null, Color.White, 0,
                new Vector2(radius, radius), .10f, SpriteEffects.None, 1);
        }

        public Vector2 teleport()
        {
            Fired = false;
            return Position;
        }
    }
}
