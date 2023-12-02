using FinalGame.Collisions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FinalGame.Entities
{
    public class Bullet
    {
        public Texture2D texture;
        public Vector2 Position { get; set; }
        public BoundingCircle Bounds;
        public Vector2 Velocity { get; set; }
        public bool Fired = false;
        public Player Player;

        int radius = 5;

        public void FireBullet(Vector2 position, Vector2 velocity, Player player)
        {
            Player = player;
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
                    //TODO break anim
                    Fired = false;
                }
            }

            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Bounds.Center = Position;

            if (Position.X < radius || Position.X > Constants.GAME_WIDTH - radius
                || Position.Y < radius || Position.Y > Constants.GAME_HEIGHT - radius)
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


            //Velocity /= 1.01f;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Bounds.Center, null, Color.White, 0,
                new Vector2(radius, radius), 1f, SpriteEffects.None, 0);
        }
    }
}
