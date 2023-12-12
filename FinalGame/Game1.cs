using FinalGame.Screens;
using FinalGame.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace FinalGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private readonly ScreenManager _screenManager;

        private Song backgroundMusic;

        //public float Scale = 1f;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            //_graphics.IsFullScreen = true;
            
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
            
            var screenFactory = new ScreenFactory();
            Services.AddService(typeof(IScreenFactory), screenFactory);

            

            _screenManager = new ScreenManager(this);
            Components.Add(_screenManager);

            backgroundMusic = Content.Load<Song>("Furious Freak");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = .5f;
            MediaPlayer.Play(backgroundMusic);

            AddInitialScreens();
        }

        private void AddInitialScreens()
        {
            _screenManager.AddScreen(new MainMenuScreen(), null);
        }

        protected override void Initialize()
        {
            if (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.AspectRatio == 5f / 3f ||
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.AspectRatio == 16f / 9f)
            {
                //_graphics.IsFullScreen = true;   
                _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                Constants.Scale = (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / (float)Constants.GAME_WIDTH;
                Constants.DISPLAY_HEIGHT = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                Constants.DISPLAY_WIDTH = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                _graphics.ApplyChanges();
            }
            base.Initialize();
        }

        protected override void LoadContent() { }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);    // The real drawing happens inside the ScreenManager component
        }
    }
}