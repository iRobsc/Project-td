using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Project_td
{
    public class main : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static Texture2D tileTexture1, tileTexture2, enemyTexture1, towerTexture1, bulletTexture1;

        private static int lives = 15;
        private static int tilesY = 15, tilesX = 25;
        private static float gridOffsetX = Tile.trueWidth * Tile.width;
        private static float gridOffsetY = Tile.trueHeight * Tile.height;
        private static bool waveSpawning = false;

        public static Tile[,] tiles;
        public static List<Bullet> bullets = new List<Bullet>();
        public static List<Enemy> enemies = new List<Enemy>();
        private static List<Tower> towers = new List<Tower>();

        private static List<Enemy> removedEnemies = new List<Enemy>();
        public static List<Bullet> removedBullets = new List<Bullet>();

        private static int[,] gridTypes;
        private float waveTimer = 0;
        private int waveCounter = 0;
        private float spawnDelay = 0;

        private static List<int> currentWave = null;
        private static List<Vector2> currentWavePath = new List<Vector2>();
        private static Tower tower1, tower2;


        private static List<int> wave1 = new List<int>();
        private static List<Vector2> wavePath1 = new List<Vector2>();
        private static List<Texture2D> tileTextures = new List<Texture2D>();
        private static List<Texture2D> enemyTextures = new List<Texture2D>();

        public main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        private void startWave(List<int> wave, List<Vector2> wavePath, float spawnDelay)
        {
            currentWave = wave;
            currentWavePath = wavePath;
            this.spawnDelay = spawnDelay;
            waveSpawning = true;
        }

        private void createEnemy(List<int> wave, List<Vector2> wavePath)
        {
            Enemy enemy = new Enemy(wave[waveCounter], wavePath);
            enemy.position = tiles[(int)(wavePath[enemy.currentNode].X), (int)(wavePath[enemy.currentNode].Y)].position;
            enemy.target = tiles[(int)enemy.wavePath[enemy.currentNode + 1].X, (int)enemy.wavePath[enemy.currentNode + 1].Y].position;
            enemy.currentNode += 1;
            enemies.Add(enemy);
            waveCounter += 1;
        }

        protected override void Initialize()
        {
            base.Initialize();
            tiles = new Tile[tilesY, tilesX];

            tileTextures.Add(tileTexture1);
            tileTextures.Add(tileTexture2);

            enemyTextures.Add(enemyTexture1);

            wavePath1.Add(new Vector2(4, 0));
            wavePath1.Add(new Vector2(4, 1));
            wavePath1.Add(new Vector2(13, 1));
            wavePath1.Add(new Vector2(13, 6));
            wavePath1.Add(new Vector2(1, 6));
            wavePath1.Add(new Vector2(1, 11));
            wavePath1.Add(new Vector2(13, 11));
            wavePath1.Add(new Vector2(13, 16));
            wavePath1.Add(new Vector2(1, 16));
            wavePath1.Add(new Vector2(1, 21));
            wavePath1.Add(new Vector2(13, 21));
            wavePath1.Add(new Vector2(13, 24));

            wave1.Add(0);
            wave1.Add(0);
            wave1.Add(0);
            wave1.Add(0);
            wave1.Add(0);
            wave1.Add(0);
            wave1.Add(0);
            wave1.Add(0);
            wave1.Add(0);
            wave1.Add(0);

            gridTypes = new int[,] { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, 
                                     { 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0},
                                     { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0}, 
                                     { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0},
                                     { 1, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0},
                                     { 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0}, 
                                     { 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0},
                                     { 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0}, 
                                     { 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0},
                                     { 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0},
                                     { 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0}, 
                                     { 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0},
                                     { 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0}, 
                                     { 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1},
                                     { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0} };

            for (int x = 0; x < tilesX; x++)
            {
                for (int y = 0; y < tilesY; y++)
                {
                    tiles[y, x] = new Tile();
                    tiles[y, x].position = new Vector2(x * Tile.trueWidth * Tile.width + gridOffsetX, y * Tile.trueHeight * Tile.height + gridOffsetY);
                    tiles[y, x].tileType = gridTypes[y, x];
                }
            }

            tower1 = new Basic_tower(new Vector2(3, 8));
            towers.Add(tower1);

            tower2 = new Basic_tower(new Vector2(8, 8));
            towers.Add(tower2);

            startWave(wave1, wavePath1, 0.5f);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            tileTexture1 = Content.Load<Texture2D>("tile1");
            tileTexture2 = Content.Load<Texture2D>("tile2");

            enemyTexture1 = Content.Load<Texture2D>("enemy1");

            towerTexture1 = Content.Load<Texture2D>("tower1");

            bulletTexture1 = Content.Load<Texture2D>("bullet1");
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (waveSpawning == true && currentWave != null)
            {
                waveTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (waveTimer >= spawnDelay && waveCounter < currentWave.Count)
                {
                    waveTimer = 0;
                    createEnemy(currentWave, currentWavePath);
                }
                else if (waveCounter == currentWave.Count)
                {
                    currentWave = null;
                    currentWavePath = null;
                    waveSpawning = false;
                }
            }

            if (removedEnemies.Count >= 1)
            {
                for (int i = 0; i < removedEnemies.Count; i++)
                {
                    enemies.Remove(removedEnemies[i]);
                    removedEnemies[i] = null;
                    removedEnemies.Remove(removedEnemies[i]);
                }
            }

            if (removedBullets.Count >= 1)
            {
                for (int i = 0; i < removedBullets.Count; i++)
                {
                    bullets.Remove(removedBullets[i]);
                    removedBullets[i] = null;
                    removedBullets.Remove(removedBullets[i]);
                }
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            for (int y = 0; y < tilesY; y++)
            {
                for (int x = 0; x < tilesX; x++)
                {
                    Texture2D currentTexture = tileTextures[tiles[y, x].tileType];
                    spriteBatch.Draw(currentTexture, tiles[y, x].position, null, Color.White, 0, new Vector2(Tile.trueWidth, Tile.trueHeight), new Vector2(Tile.width, Tile.height), SpriteEffects.None, 0);
                }
            }

            foreach (Bullet bullet in bullets)
            {
                spriteBatch.Draw(bullet.texture, bullet.position, null, Color.White, 0, new Vector2(32, 32), new Vector2(1, 1), SpriteEffects.None, 0);
                bullet.move();
            }

            foreach (Enemy enemy in enemies)
            {
                enemy.move();

                if (enemy.reachedGoal == true)
                {
                    removedEnemies.Add(enemy);
                    lives -= 1;
                }

                else if (enemy.hp <= 0)
                {
                    removedEnemies.Add(enemy);
                }

                Texture2D currentTexture = enemyTextures[enemy.type];
                spriteBatch.Draw(currentTexture, enemy.position, null, Color.White, 0, new Vector2(32, 32), new Vector2(1, 1), SpriteEffects.None, 0);
            }

            foreach (Tower tower in towers)
            {
                for (int i = 0; i < enemies.Count; i++)
                {
                    tower.setTarget(i);
                }

                if (tower.target != null && enemies.Count == 0)
                {
                    tower.target = null;
                }

                if (tower.cooldown > 0)
                {

                    tower.cooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (tower.cooldown < 0)
                    {
                        tower.cooldown = 0;
                    }
                }

                if (tower.cooldown == 0 && tower.target != null)
                {
                    tower.shoot();
                    tower.cooldown = tower.fireRate;
                }

                spriteBatch.Draw(tower.texture, tower.realPosition, null, Color.White, 0, new Vector2(32, 32), new Vector2(1, 1), SpriteEffects.None, 0);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
