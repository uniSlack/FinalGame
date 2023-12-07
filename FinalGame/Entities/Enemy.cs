using FinalGame.Collisions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System.ComponentModel.Design;
using Microsoft.Xna.Framework.Audio;
using System.Security.Policy;
using System.IO;

namespace FinalGame.Entities
{
    public class Enemy
    {
        public bool Alive;
        public Texture2D Texture;
        public Vector2 Position { get; set; }
        public List<Vector2> Path;
        int currentStep = 0;
        int nextStep = 1;

        public BoundingCircle Bounds;

        public float Rotation;

        public int Radius = 20;
        public int Speed = 1;

        public Bullet Bullet;
        public double BulletCooldownTimer;
        double BulletCooldownLength = 3;
        int BulletSpeed = 450;

        public SoundEffect DeathSoundEffect;

        Player Player;

        public Enemy(Vector2 position, float rotation, Player player, List<Vector2> path)
        {
            Player = player;
            Position = position;
            Rotation = rotation;
            Bounds = new BoundingCircle(Position, Radius);
            Bullet = new Bullet();
            BulletCooldownTimer = BulletCooldownLength;
            Alive = true;
            if (path != null) Path = path;
        }

        private void IncrementStep()
        {
            currentStep = nextStep;
            if (nextStep + 1 == Path.Count) nextStep = 0;
            else nextStep++;
        }

        public void Update(GameTime gameTime, List<Wall> walls)
        {
            if (Path != null)
            {
                //if (
                //    (Position.X <= Path[nextStep].X + 5 &&
                //    Position.X >= Path[nextStep].X - 5 ) &&
                //    (Position.Y <= Path[nextStep].X + 5 &&
                //    Position.Y >= Path[nextStep].X - 5)
                //    )
                if (Position == Path[nextStep])
                {
                    IncrementStep();
                }
                Vector2 potentialPosition = new Vector2(Path[nextStep].X - Path[currentStep].X, Path[nextStep].Y - Path[currentStep].Y);
                potentialPosition.Normalize();
                potentialPosition *= Speed;
                Position += potentialPosition;
                Bounds.Center = Position;


            }

                // end movement
                Vector2 playerToEnemy = new Vector2(Position.X - Player.Position.X, Position.Y - Player.Position.Y);
            Rotation = (float)Math.Atan2(playerToEnemy.Y, playerToEnemy.X) + (float)Math.PI;

            // check attack collision
            if (Player.attack.Active)
            {
                if (Player.attack.Bounds.CollidesWith(Bounds))
                {

                    if (Rotation - Math.PI >= Player.attack.Rotation - Math.PI / 32 && Rotation - Math.PI <= Player.attack.Rotation + Math.PI / 32)
                    {
                        Hit();
                    }
                }
            }

            if (BulletCooldownTimer <= 0 && Alive)
            {
                Bullet.FireBullet(Position, Vector2.Normalize(new Vector2(Player.Position.X - Position.X, Player.Position.Y - Position.Y)) * BulletSpeed, Player);
                BulletCooldownTimer = BulletCooldownLength;
            }
            else if (Bullet.Fired)
            {
                Bullet.Update(gameTime, walls);
            }
            else
            {
                BulletCooldownTimer -= gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Alive)
            {
                spriteBatch.Draw(Texture, Bounds.Center, null, Color.Red, Rotation,
                new Vector2(Radius, Radius), 1, SpriteEffects.None, 1);
            }

            if (Bullet.Fired) Bullet.Draw(gameTime, spriteBatch);
        }

        public void Hit()
        {
            DeathSoundEffect.Play(.05f, 0, 0);
            Alive = false;
        }
    }
}
