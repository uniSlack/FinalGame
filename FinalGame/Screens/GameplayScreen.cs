using FinalGame.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using static System.TimeZoneInfo;

namespace FinalGame.Screens
{
    public class GameplayScreen : GameScreen
    {
        private ContentManager Content;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Player player;
        private HealthBar healthBar;
        private List<Wall> walls = new List<Wall>();
        private List<Enemy> Enemies = new List<Enemy>();
        private Random r = new Random();
        private bool enimiesAlive = true;

        private float _pauseAlpha;
        private readonly InputAction _pauseAction;

        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            _pauseAction = new InputAction(
                new[] { Buttons.Start, Buttons.Back },
                new[] { Microsoft.Xna.Framework.Input.Keys.Back, Microsoft.Xna.Framework.Input.Keys.Escape }, true);
            
            player = new Player(new Vector2(100, 100));

            healthBar = new HealthBar();

            InitializeWalls();
            InitializeEnemies();
        }

        private void InitializeWalls()
        {
            walls.Add(new Wall(new Vector2(500, 200), false, 150));
            walls.Add(new Wall(new Vector2(400, 200), true, 150));
            walls.Add(new Wall(new Vector2(600, 300), false, 150));
        }

        private void InitializeEnemies()
        {
            Enemies.Add(new Enemy(new Vector2(500, 300), 0f, player));
        }

        public override void Activate()
        {
            if (Content == null)
                Content = new ContentManager(ScreenManager.Game.Services, "Content");

            //_gameFont = _content.Load<SpriteFont>("gamefont");

            _spriteBatch = new SpriteBatch(ScreenManager.GraphicsDevice);

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

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
        }

        public override void Deactivate()
        {
            base.Deactivate();
        }

        public override void Unload()
        {
            Content.Unload();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                _pauseAlpha = Math.Min(_pauseAlpha + 1f / 32, 1);
            else
                _pauseAlpha = Math.Max(_pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {
                if (player.Health <= 0)
                {
                    DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("Game Over! Good luck next time!", "You Died!", MessageBoxButtons.OK);//end game;
                    if (dialogResult == DialogResult.OK)
                    {
                        ScreenManager.Game.Exit();
                    }
                }

                if (!enimiesAlive)
                {
                    DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("Good Job! You defeated all the enemies!", "You Won!", MessageBoxButtons.OK);//end game;
                    if (dialogResult == DialogResult.OK)
                    {
                        ScreenManager.Game.Exit();
                    }
                }

                //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == Microsoft.Xna.Framework.Input.ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
                    //Exit();

                player.Update(gameTime, walls);

                enimiesAlive = false;
                foreach (Enemy e in Enemies)
                {
                    e.Update(gameTime, walls);
                    if (e.Alive) enimiesAlive = true;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {

            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 0, 0);

            
            var spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            player.Draw(gameTime, spriteBatch);

            foreach (Wall w in walls)
            {
                w.Draw(gameTime, spriteBatch);
            }

            foreach (Enemy e in Enemies)
            {
                e.Draw(gameTime, spriteBatch);
            }

            healthBar.Draw(spriteBatch, player.Health);

            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || _pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, _pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            // Look up inputs for the active player profile.
            int playerIndex = 0; //(int)ControllingPlayer.Value;

            var keyboardState = input.CurrentKeyboardStates[playerIndex];
            var gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected && input.GamePadWasConnected[playerIndex];

            
            
            player.HandleInput(gameTime, input, walls);
        }
    }
}
