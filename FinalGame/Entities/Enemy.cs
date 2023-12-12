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
        public Texture2D Texture2;
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
            Radius = (int)((float)Radius * Constants.Scale);
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
            if (path != null)
            {
                List<Vector2> temp = new List<Vector2>();
                foreach (Vector2 v  in path) temp.Add( v * Constants.Scale);
                Path = temp;
            }
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
                if (Position.X <= Path[nextStep].X + 10 && Position.X >= Path[nextStep].X - 10 &&
                    Position.Y <= Path[nextStep].Y + 10 && Position.Y >= Path[nextStep].Y - 10)
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
            if (Phasing)
            {
                if (burst) color = Color.Lerp(Color.MediumPurple, Color.White, .1f);
                else color = Color.MediumPurple;
            }

            if (Alive)
            {
                spriteBatch.Draw(Texture, Bounds.Center, null, color, Rotation,
                new Vector2(Radius / Constants.Scale, Radius / Constants.Scale), Constants.Scale, SpriteEffects.None, .9f);
                if (burst)
                {
                    spriteBatch.Draw(Texture2, Bounds.Center, null, color, Rotation,
                        new Vector2(Texture2.Height / 2), Constants.Scale * .22f, SpriteEffects.None, 1f);
                }
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
