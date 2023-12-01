using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FinalGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Player player;
        private List<Wall> walls = new List<Wall>();
        private List<Enemy> Enemies = new List<Enemy>();
        private Random r = new Random();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        private void InitializeWalls()
        {
            walls.Add(new Wall(new Vector2(200, 200), false, 150));
            walls.Add(new Wall(new Vector2(100, 200), true, 150));
            walls.Add(new Wall(new Vector2(300, 300), false, 150));
        }

        private void InitializeEnemies()
        {
            Enemies.Add(new Enemy(new Vector2(600,300), 0f, player));
        }

        protected override void Initialize()
        {
            player = new Player(new Vector2(100, 100), 50);

            InitializeWalls();
            InitializeEnemies();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            foreach (Wall w in walls)
            {
                w.Texture = Content.Load<Texture2D>("WhiteTexture");
            }

            foreach (Enemy e in Enemies)
            {
                e.Texture = Content.Load<Texture2D>("Circle3");
                e.Bullet.texture = Content.Load<Texture2D>("TeleportGrenade");
            }

            player.Texture = Content.Load<Texture2D>("Circle3");
            player.teleportGrenade.texture = Content.Load<Texture2D>("TeleportGrenade");
            player.attack.Texture = Content.Load<Texture2D>("WhiteTexture");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == Microsoft.Xna.Framework.Input.ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
                Exit();

            player.Update(gameTime, walls);

            foreach (Enemy e in Enemies)
            {
                e.Update(gameTime, walls);
                // check attack collision
                if (player.attack.Active)
                {
                    if (player.attack.Bounds.CollidesWith(e.Bounds))
                    {
                        Vector2 playerToEnemy = new Vector2(player.Position.X - e.Position.X, player.Position.Y - e.Position.Y);
                        float P2EAngle = (float)Math.Atan2(playerToEnemy.Y, playerToEnemy.X);
                        if (P2EAngle >= player.attack.Rotation - Math.PI/16 || P2EAngle <= player.attack.Rotation + Math.PI / 16)
                        {
                            e.Hit();
                        }
                    }
               }
            }

            if (player.Health <= 2)
            {
                DialogResult dialogResult = System.Windows.Forms.MessageBox.Show( "Game Over! Good luck next time!", "You Died!", MessageBoxButtons.OK);//end game;
                if (dialogResult == DialogResult.OK)
                {
                    Exit();
                }
            }
                

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            player.Draw(gameTime, _spriteBatch);

            foreach(Wall w in walls)
            {
                w.Draw(gameTime, _spriteBatch);
            }

            foreach(Enemy e in Enemies)
            {
                e.Draw(gameTime, _spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}