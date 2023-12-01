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
        private HealthBar healthBar;
        private List<Wall> walls = new List<Wall>();
        private List<Enemy> Enemies = new List<Enemy>();
        private Random r = new Random();
        private bool enimiesAlive = true;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        private void InitializeWalls()
        {
            walls.Add(new Wall(new Vector2(500, 200), false, 150));
            walls.Add(new Wall(new Vector2(400, 200), true, 150));
            walls.Add(new Wall(new Vector2(600, 300), false, 150));
        }

        private void InitializeEnemies()
        {
            Enemies.Add(new Enemy(new Vector2(500,300), 0f, player));
        }

        protected override void Initialize()
        {
            player = new Player(new Vector2(100, 100));

            healthBar = new HealthBar();

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
                e.Texture = Content.Load<Texture2D>("Circle4");
                e.Bullet.texture = Content.Load<Texture2D>("TeleportGrenade");
            }

            player.Texture = Content.Load<Texture2D>("Circle4");
            player.teleportGrenade.texture = Content.Load<Texture2D>("TeleportGrenade");
            player.attack.Texture = Content.Load<Texture2D>("WhiteTexture");

            healthBar.Texture = Content.Load<Texture2D>("WhiteTexture");
        }

        protected override void Update(GameTime gameTime)
        {
            if (player.Health <= 0)
            {
                DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("Game Over! Good luck next time!", "You Died!", MessageBoxButtons.OK);//end game;
                if (dialogResult == DialogResult.OK)
                {
                    Exit();
                }
            }

            if (!enimiesAlive)
            {
                DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("Good Job! You defeated all the enimies!", "You Won!", MessageBoxButtons.OK);//end game;
                if (dialogResult == DialogResult.OK)
                {
                    Exit();
                }
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == Microsoft.Xna.Framework.Input.ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
                Exit();

            player.Update(gameTime, walls);

            enimiesAlive = false;
            foreach (Enemy e in Enemies)
            {
                e.Update(gameTime, walls);
                if (e.Alive) enimiesAlive = true;
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

            healthBar.Draw(_spriteBatch, player.Health);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}