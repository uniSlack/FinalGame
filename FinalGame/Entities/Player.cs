using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FinalGame.Collisions;
using Microsoft.Xna.Framework.Input;
using System.Xml.Linq;
using System.Security.Policy;
using FinalGame.StateManagement;
using System.Transactions;
using Microsoft.Xna.Framework.Audio;

namespace FinalGame.Entities
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

        public int Radius = 20;
        public int Health = 3;

        public float colorBlinkTime = 10f;
        public float colorBlinkTimer = 10f;

        public SoundEffect AttackSound;
        public SoundEffect TeleportFailSound;
        public SoundEffect TeleportSuccessSound;
        public SoundEffect HurtSound;


        public Player(Vector2 position)
        {
            Position = position;
            Bounds = new BoundingCircle(position, Radius);

            teleportGrenade = new TeleportGrenade();
            attack = new Attack(this);
        }


        public void HandleInput(GameTime gameTime, InputState input, List<Wall> walls)
        {

            keyboardState = Keyboard.GetState();
            priorMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            int speed = 3;

            Vector2 postitionChange = new Vector2(0, 0);

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



            Vector2 mouseDirection = new Vector2(currentMouseState.X - Position.X, currentMouseState.Y - Position.Y);
            Rotation = (float)Math.Atan2(mouseDirection.Y, mouseDirection.X);

            if (currentMouseState.LeftButton == ButtonState.Pressed && priorMouseState.LeftButton == ButtonState.Released
                && !attack.Active)
            {
                attack.StartAttack(mouseDirection);
                AttackSound.Play(.1f, 0, 0);
            }

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
                        )
                    {
                        TeleportFailSound.Play(.4f, 0, 0);
                        color = Color.Blue;
                        return;
                    }
                    foreach (Wall w in walls)
                    {
                        if (potentialBounds.CollidesWith(w.Bounds))
                        {
                            TeleportFailSound.Play(.4f, 0, 0);
                            color = Color.Blue;
                            return;
                        }
                    }
                    TeleportSuccessSound.Play(.4f, 0, 0);
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
        }

        public void Update(GameTime gameTime, List<Wall> walls)
        {
            if (attack.Active)
            {
                attack.Update(gameTime, null);
            }
            if (teleportGrenade.Fired)
            {
                teleportGrenade.Update(gameTime, walls);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (color != Color.White)
            {
                spriteBatch.Draw(Texture, Bounds.Center, null, Color.Lerp(Color.White, color, colorBlinkTimer/colorBlinkTime), Rotation,
                new Vector2(Radius, Radius), 1, SpriteEffects.None, 1);
                colorBlinkTimer -= .5f;
                if (colorBlinkTimer <= 0 || color == Color.White) 
                {
                    colorBlinkTimer = colorBlinkTime;
                    color= Color.White;
                }
            }
            else
            {
                spriteBatch.Draw(Texture, Bounds.Center, null, color, Rotation,
                new Vector2(Radius, Radius), 1, SpriteEffects.None, 1);
            }
            

            if (teleportGrenade.Fired) teleportGrenade.Draw(gameTime, spriteBatch);
            if (attack.Active) attack.Draw(gameTime, spriteBatch);


        }

        public void Hit()
        {
            HurtSound.Play(.1f, 0, 0);
            color = Color.Red;
            Health--;
        }
    }
}
