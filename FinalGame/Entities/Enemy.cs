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

        //public Bullet Bullet;
        public List<Bullet> Bullets = new List<Bullet>();
        public double BulletCooldownTimer;
        double BulletCooldownLength = 3;
        int BulletSpeed = 450;

        public bool burst = false;
        int bulletsFired = 0;
        public int burstNumber = 3;
        public double BurstShotSeperationLength = .1f;
        public double BurstShotSeperationTimer = 0;

        public bool Phasing;

        public Color color = Color.Red;

        public SoundEffect DeathSoundEffect;

        Player Player;

        public Enemy(Vector2 position, float rotation, Player player, List<Vector2> path)
        {
            Player = player;
            Position = position * Constants.Scale;
            Rotation = rotation;
            Bounds = new BoundingCircle(Position, Radius);
            for (int i = 0; i <= burstNumber; i++)
            {
                Bullets.Add(new Bullet());
            }
            
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
            if (Phasing) foreach (Bullet b in Bullets)
                {
                    b.Phasing = true;
                    b.speed = .6f;
                }
            if (Path != null)
            {
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

            if (burst)
            {
                if (BulletCooldownTimer <= 0 && Alive)
                {
                    if(bulletsFired < burstNumber)
                    {
                        if (BurstShotSeperationTimer <= 0)
                        {
                            Bullets[bulletsFired].FireBullet(Position, Vector2.Normalize(new Vector2(Player.Position.X - Position.X, Player.Position.Y - Position.Y)) * BulletSpeed, Player);
                            BurstShotSeperationTimer = BurstShotSeperationLength;
                            bulletsFired++;
                            if (bulletsFired >= burstNumber)
                            {
                                BulletCooldownTimer = BulletCooldownLength;
                                bulletsFired = 0;
                            }
                        }
                        else BurstShotSeperationTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                    }
                    else BulletCooldownTimer = BulletCooldownLength;
                }
                else BulletCooldownTimer -= gameTime.ElapsedGameTime.TotalSeconds;

                foreach (Bullet b in Bullets) if (b.Fired) b.Update(gameTime, walls);
            }
            else
            {
                if (BulletCooldownTimer <= 0 && Alive)
                {
                    Bullets[0].FireBullet(Position, Vector2.Normalize(new Vector2(Player.Position.X - Position.X, Player.Position.Y - Position.Y)) * BulletSpeed, Player);
                    BulletCooldownTimer = BulletCooldownLength;
                }
                else if (Bullets[0].Fired)
                {
                    Bullets[0].Update(gameTime, walls);
                }
                else
                {
                    BulletCooldownTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Phasing) color = Color.MediumPurple;
            if (Alive)
            {
                spriteBatch.Draw(Texture, Bounds.Center, null, color, Rotation,
                new Vector2(Radius, Radius), 1, SpriteEffects.None, 1);
            }

            foreach(Bullet b in Bullets) if (b.Fired) b.Draw(gameTime, spriteBatch);
        }

        public void Hit()
        {
            DeathSoundEffect.Play(.05f, 0, 0);
            Alive = false;
        }
    }
}
