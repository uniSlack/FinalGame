﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FinalGame.Collisions;
using Microsoft.Xna.Framework.Input;
using System.Xml.Linq;

namespace FinalGame
{
    public class Player
    {
        public Texture2D Texture;
        public Vector2 Position { get; set; } // center of circle

        public BoundingCircle Bounds;

        public TeleportGrenade teleportGrenade;
        public Attack attack;

        KeyboardState keyboardState;
        MouseState currentMouseState;
        MouseState priorMouseState;

        public Color color = Color.White;
        public float Rotation = 0;

        public int Radius;
        

        public Player(Vector2 position, int r)
        {
            Position = position;
            Radius = r;
            Bounds = new BoundingCircle(position, Radius);

            teleportGrenade = new TeleportGrenade();
            attack = new Attack(this);
        }

        public void Update(GameTime gameTime, List<Wall> walls)
        {
            keyboardState = Keyboard.GetState();
            priorMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            int speed = 3;

            Vector2 postitionChange = new Vector2(0,0);

            if ((keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
                && Position.Y > Radius) postitionChange += new Vector2(0, -speed);
            if ((keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
                && Position.Y < Constants.GAME_HEIGHT - Radius) postitionChange += new Vector2(0, speed);
            if ((keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
                && Position.X > Radius) postitionChange += new Vector2(-speed, 0);
            if ((keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
                && Position.X < Constants.GAME_WIDTH - Radius) postitionChange += new Vector2(speed, 0);

            Position += postitionChange;

            Bounds.Center = Position;

            foreach (Wall w in walls)
            {
                if (Bounds.CollidesWith(w.Bounds))
                {
                    Position -= postitionChange;
                    Bounds.Center = Position;
                }
            }

            // Post movement
            Vector2 mouseDirection = new Vector2(currentMouseState.X - Position.X, currentMouseState.Y - Position.Y);
            Rotation = (float)Math.Atan2(mouseDirection.Y, mouseDirection.X);

            if (teleportGrenade.Fired)
            {
                teleportGrenade.Update(gameTime, walls);
                if (currentMouseState.RightButton == ButtonState.Pressed && priorMouseState.RightButton == ButtonState.Released)
                {
                    Vector2 potentialPosition = teleportGrenade.teleport();
                    BoundingCircle potentialBounds = new BoundingCircle(potentialPosition, Radius);
                    if (
                        !(potentialPosition.Y > Radius &&
                        potentialPosition.Y < Constants.GAME_HEIGHT - Radius &&
                        potentialPosition.X > Radius &&
                        potentialPosition.X < Constants.GAME_WIDTH - Radius)
                        ) return;
                    foreach (Wall w in walls)
                    {
                        if (potentialBounds.CollidesWith(w.Bounds))
                        {
                            return;
                        }
                    }
                    Bounds = potentialBounds;
                    Position = potentialPosition;
                }
            }
            else if (currentMouseState.RightButton == ButtonState.Pressed && priorMouseState.RightButton == ButtonState.Released)
            {
                teleportGrenade.FireGrenade(
                    Position + Vector2.Normalize(mouseDirection) * 20,
                    mouseDirection
                    );
            }

            if (attack.Active)
            {
                attack.Update(gameTime, mouseDirection);
            }
            else if(currentMouseState.LeftButton == ButtonState.Pressed && priorMouseState.LeftButton == ButtonState.Released)
            {
                attack.StartAttack(mouseDirection);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Bounds.Center, null, color, 0, 
                new Vector2(Radius,Radius), 1, SpriteEffects.None, 0);
            color = Color.White;

            if (teleportGrenade.Fired) teleportGrenade.Draw(gameTime, spriteBatch);
            if (attack.Active) attack.Draw(gameTime, spriteBatch);
        }

        public void Hit()
        {
            color = Color.Red;
        }

        //private void UpdateMovement()
        //{

        //}

        //private void UpdateTeleport()
        //{

        //}

        //private void UpdateAttack()
        //{

        //}
    }
}