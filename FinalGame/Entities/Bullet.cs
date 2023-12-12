using FinalGame.Collisions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Drawing.Drawing2D;

namespace FinalGame.Entities
{
    public class Bullet
    {
        public Texture2D texture;
        public Vector2 Position { get; set; }
        public BoundingCircle Bounds;
        public Vector2 Velocity { get; set; }
        public bool Fired = false;
        public float speed = 1;
        public float baseSpeed = 1;
        public Player Player;

        public Color color = Color.Red;
        public bool Phasing;

        int radius = 5;

        public void FireBullet(Vector2 position, Vector2 velocity, Player player)
        {
            Player = player;
            Bounds = new BoundingCircle(position, radius);
            Position = position;
            Velocity = velocity;
            speed = baseSpeed * Constants.Scale;
            Fired = true;
        }

        public void Update(GameTime gameTime, List<Wall> walls)
        {
            if (!Phasing)
            {
                foreach (Wall w in walls)
                {
                    if (Bounds.CollidesWith(w.Bounds))
                    {
                        //TODO break anim
                        Fired = false;
                    }
                }
            }
            

            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds * speed;
            Bounds.Center = Position;

            if (Position.X < radius || Position.X > Constants.DISPLAY_WIDTH - radius
                || Position.Y < radius || Position.Y > Constants.DISPLAY_HEIGHT - radius)
            {
                //TODO break anim
                Fired = false;
            }

            if (Bounds.CollidesWith(Player.Bounds))
            {
                Player.Hit();
                Fired = false;
                Bounds = new BoundingCircle();
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Phasing) color = Color.MediumPurple;
            spriteBatch.Draw(texture, Bounds.Center, null, color, 0,
                new Vector2(radius, radius), 1f, SpriteEffects.None, 1);
        }
    }
}
