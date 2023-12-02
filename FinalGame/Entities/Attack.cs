using FinalGame.Collisions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using System.Security.Policy;

namespace FinalGame.Entities
{
    public class Attack
    {

        public Texture2D Texture;
        public Vector2 Position { get; set; }

        public bool Active = false;
        public float ArcLength = (float)Math.PI / 2;
        public double Duration = .3;
        public double Countdown = 0;
        public BoundingCircle Bounds;
        public float Rotation = 0;
        public int BeamWidth = 5;
        public int BeamLength = 50;
        public float beamDistatnceFromPlayerScalar = 2.1f;

        Player player;
        Vector2 mouseDirection;

        public Attack(Player p)
        {
            player = p;
        }

        public void StartAttack(Vector2 mouse)
        {
            Bounds = new BoundingCircle(player.Position, player.Radius + BeamLength);
            Active = true;
            Countdown = Duration;
            SetRotationAndPosition();
        }

        public void Update(GameTime gameTime, Vector2? mouse)
        {
            if (mouse != null) mouseDirection = (Vector2)mouse;
            SetRotationAndPosition();
            Countdown -= gameTime.ElapsedGameTime.TotalSeconds;
            if (Countdown <= 0)
            {
                Active = false;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                Texture,
                new Rectangle((int)Position.X, (int)Position.Y, BeamWidth, BeamLength),
                null,
                Color.White,
                Rotation - (float)Math.PI / 2,
                new Vector2(0, -(player.Radius * beamDistatnceFromPlayerScalar)),
                SpriteEffects.None,
                0
                );
        }

        private void SetRotationAndPosition()
        {
            Rotation = player.Rotation + (float)(ArcLength / 2 - ArcLength * (Countdown / Duration));
            Position = player.Position;
        }
    }
}
