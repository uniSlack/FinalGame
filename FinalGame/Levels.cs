using FinalGame.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace FinalGame
{
    public class Levels
    {
        public int TotalLevels = 2;

        public List<List<Wall>> WallsPerLevel = new List<List<Wall>>()
        {
            new List<Wall>()
            {
                new Wall(new Vector2(600, 0), true, 220, 30),
                new Wall(new Vector2(600, 260), true, 220, 30)
            },
            new List<Wall>()
            {
                new Wall(new Vector2(500, 200), false, 150, 30),
                new Wall(new Vector2(400, 200), true, 150, 30),
                new Wall(new Vector2(600, 300), false, 150, 30)
            },
            new List<Wall>()
            {
                new Wall(new Vector2(320, 210), true, 100, 30),
                new Wall(new Vector2(450, 210), true, 100, 30)
            }
        };

        public List<Player> PlayerPerLevel = new List<Player>()
        {
                new Player(new Vector2(100, 240)),
                new Player(new Vector2(100, 240)),
                new Player(new Vector2(400, 240))
        };

        public List<Enemy> GetEnemiesPerLevel(int level, Player player)
        {
            switch (level)
            {
                case 0:
                    return new List<Enemy>() { new Enemy(new Vector2(700, 220), 0f, player) };
                case 1:
                    return new List<Enemy>() 
                    {
                        new Enemy(new Vector2(500, 300), 0f, player), 
                        new Enemy(new Vector2(550, 300), 0f, player) { BulletCooldownTimer = 4.5}
                    };
                case 2:
                    return new List<Enemy>()
                    {
                        new Enemy(new Vector2(40, 40), 0f, player),
                        new Enemy(new Vector2(40, 440), 0f, player) { BulletCooldownTimer = 1},
                        new Enemy(new Vector2(760, 40), 0f, player) { BulletCooldownTimer = 2},
                        new Enemy(new Vector2(760, 440), 0f, player) { BulletCooldownTimer = 4}
                    };
                default:
                    return new List<Enemy>();
            }
        }
    }
}
