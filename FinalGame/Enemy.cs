﻿using FinalGame.Collisions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System.ComponentModel.Design;

namespace FinalGame
{
    public class Enemy
    {
        public bool Alive;
        public Texture2D Texture;
        public Vector2 Position { get; set; }

        public BoundingCircle Bounds;

        public float Rotation;

        public int ShotTimer;
        public int Radius = 50;

        public Bullet Bullet;
        double BulletCooldownTimer;
        double BulletCooldownLength = 3;
        int BulletSpeed = 450;
        

        Player Player;

        public Enemy(Vector2 position, float rotation, Player player)
        {
            Player = player;
            Position = position;
            Rotation = rotation;
            Bounds = new BoundingCircle(Position, Radius);
            Bullet = new Bullet();
            BulletCooldownTimer = BulletCooldownLength;
            Alive = true;
        }

        public void Update(GameTime gameTime, List<Wall> walls)
        {
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
                spriteBatch.Draw(Texture, Bounds.Center, null, Color.Red, 0,
                new Vector2(Radius, Radius), 1, SpriteEffects.None, 0);
            }
            
            if (Bullet.Fired) Bullet.Draw(gameTime, spriteBatch);
        }

        public void Hit()
        {
            Alive = false;
        }
    }
}