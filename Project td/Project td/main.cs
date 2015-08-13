using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
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
        public static Texture2D tileTexture1, tileTexture2, enemyTexture1, towerTexture1, bulletTexture1; // Adds the textures to the code so we can handle them later

        private static int lives = 15; // Well... lives :P
        private static int tilesY = 15, tilesX = 25; // Sets how many tiles that should be in the x and y dimension
        private static float gridOffsetX = Tile.trueWidth * Tile.width; // Sets the offset between each tile so they don't overlap eachother (THIS SHOULD NOT BE CHANGED)
        private static float gridOffsetY = Tile.trueHeight * Tile.height;
        private static bool waveSpawning = false; // Checks if it's spawning enemies or not currently

        public static Tile[,] tiles; // List of every tile in the game
        public static List<Bullet> bullets = new List<Bullet>(); // List of every bullet in the game
        public static List<Enemy> enemies = new List<Enemy>(); // List of every enemy in the game
        private static List<Tower> towers = new List<Tower>(); // List of every tower in the game

        private static List<Enemy> removedEnemies = new List<Enemy>(); // List of every enemy that will be removed
        public static List<Bullet> removedBullets = new List<Bullet>(); // List of every bullet that will be removed

        private static int[,] tileTypes; // The list that contains the different types of tiles in the level e.g. "0" is dirt and "1" is grass
        private float waveTimer = 0; // Counts up to match the delay between each mob spawn (if waveTimer = 0.5 and spawnDelay = 0.5 then spawn the enemy)
        private int waveCounter = 0; // Counts up to match how many enemies there are on the map (if waveCounter = 10 and currentWave.Count = 10 then the wave is over and the spawning stops)
        private float spawnDelay; // This is the delay between each enemy spawn (Do not change it here because it is changed within the "startWave" function)

        private static List<int> currentWave = null; // The current wave that is spawning different types of enemies
        private static List<Vector2> wavePath = new List<Vector2>(); // The 
        private static Tower tower1, tower2;


        private static List<int> wave1 = new List<int> { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }; // This is the first wave containing enemies of the type "0"

        private static List<Vector2> wavePath1 = new List<Vector2> { new Vector2(4, 0), new Vector2(4, 1), new Vector2(13, 1), new Vector2(13, 6), new Vector2(1, 6), // Creating the path that the enemies walk on ( With nodes )
                                                                     new Vector2(1, 11), new Vector2(13, 11), new Vector2(13, 16), new Vector2(1, 16), new Vector2(1, 21),
                                                                     new Vector2(13, 21), new Vector2(13, 24)  };

        private static List<Texture2D> tileTextures = new List<Texture2D>();
        private static List<Texture2D> enemyTextures = new List<Texture2D>();

        public main() // the constructor for the class but it only contains standard code
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        private void startWave(List<int> wave, List<Vector2> wavePath, float spawnDelay) // The function that starts the waves, this function can be used with startWave( "insert wave", "insert wave path", "insert spawn delay" )
        {
            currentWave = wave; // Setting the current wave to the wave that was set in the function
            main.wavePath = wavePath; // Setting the wave path to the path that was set in the function
            this.spawnDelay = spawnDelay; // // Setting the spawn delay to the delay that was set in the function
            waveSpawning = true; // This starts the spawning
        }

        private void createEnemy(List<int> wave, List<Vector2> wavePath) // The function that creates the enemies, this function can be used with createEnemy( "insert wave", "insert wave path" )
        {
            Enemy enemy = new Enemy(wave[waveCounter], wavePath); // Adds the enemy to the game
            enemy.position = tiles[(int)(wavePath[enemy.currentNode].X), (int)(wavePath[enemy.currentNode].Y)].position; // The spawn position
            enemy.currentNode += 1; // the current node is the second node after the enemy has spawned (the first node is the spawn position)
            enemy.target = tiles[(int)enemy.wavePath[enemy.currentNode].X, (int)enemy.wavePath[enemy.currentNode].Y].position; // The target tile the enemy wants to move to
            enemies.Add(enemy); // Adds the enemy to the list of all the enemies
            waveCounter += 1; // Adds the enemy to the waveCounter
        }

        protected override void Initialize()
        {
            base.Initialize();
            tiles = new Tile[tilesY, tilesX];

            tileTextures.Add(tileTexture1); // Adding the textures to the lists of different textures
            tileTextures.Add(tileTexture2);

            enemyTextures.Add(enemyTexture1);
            

            tileTypes = new int[,] { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, // Drawing the level
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

            for (int x = 0; x < tilesX; x++) // A double for loop to create the level (one for rows and one for columns)
            {
                for (int y = 0; y < tilesY; y++)
                {
                    tiles[y, x] = new Tile();
                    tiles[y, x].position = new Vector2(x * Tile.trueWidth * Tile.width + gridOffsetX, y * Tile.trueHeight * Tile.height + gridOffsetY);
                    tiles[y, x].tileType = tileTypes[y, x];
                }
            }

            tower1 = new Basic_tower(new Vector2(3, 8)); // Hardcoding 2 towers into the level (3, 8) are the coordinates on the level
            towers.Add(tower1);

            tower2 = new Basic_tower(new Vector2(8, 8));
            towers.Add(tower2);

            startWave(wave1, wavePath1, 0.5f); // Starting the wave with 0.5 sec delay between each spawn and including the wavepath they need to go to reach the goal
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            tileTexture1 = Content.Load<Texture2D>("tile1"); // Loading every texture to the game
            tileTexture2 = Content.Load<Texture2D>("tile2");

            enemyTexture1 = Content.Load<Texture2D>("enemy1");

            towerTexture1 = Content.Load<Texture2D>("tower1");

            bulletTexture1 = Content.Load<Texture2D>("bullet1");
        }

        protected override void UnloadContent()
        {
            // This is where we unload unnecessary stuff out of the game (No need at the moment)
        }

        protected override void Update(GameTime gameTime) // This function runs the code every frame so things like movement and other things that need to be updated every frame can work
        {
            if (waveSpawning == true && currentWave != null) // If the wave is still spawning creatures and the currentwave isn't null (null = nothing) then do the things below
            {
                waveTimer += (float)gameTime.ElapsedGameTime.TotalSeconds; // Add the total seconds since the last frame to the waveTimer
 
                if (waveTimer >= spawnDelay && waveCounter < currentWave.Count) // if the waveTimer is larger or equal to the spawnDelay (0.5 seconds in this case) and the wavecounter is smaller than the amount of enemies in the wave then add an enemy to the game
                {
                    waveTimer = 0;
                    createEnemy(currentWave, wavePath);
                }
                else if (waveCounter == currentWave.Count) // if the amount of enemies spawned is equal to the total enemies that the wave holds, then stop the wave spawning
                {
                    currentWave = null;
                    wavePath = null;
                    waveSpawning = false;
                }
            }

            foreach (Tower tower in towers) // Each tower in the list of all towers (it loops through every tower and does all the code below for each tower)
            {
                for (int i = 0; i < enemies.Count; i++) // Set the towers target to the first enemy in the wave
                {
                    tower.setTarget(i);
                }

                if (tower.target != null && enemies.Count == 0) // if every enemy is dead and the tower has a target, then it sets the towers target to nothing (null)
                {
                    tower.target = null;
                }

                if (tower.cooldown > 0) // If the cooldown is above 0 (which means the cooldown is active) then do the things below
                {

                    tower.cooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds; // decrease the cooldown with the time lapsed since the last update (which means it counts down in seconds basically)

                    if (tower.cooldown < 0) // The cooldown can't go below 0, that would be horrificly bad coding
                    {
                        tower.cooldown = 0;
                    }
                }

                if (tower.cooldown == 0 && tower.target != null) // If there's no cooldown active and the tower has a target, then run the method "shoot" and add the fire rate of the tower to the cooldown
                {
                    tower.shoot();
                    tower.cooldown = tower.fireRate;
                }
            }

            foreach (Bullet bullet in bullets) // Loops through every bullet in the list of bullets and runs the move command
            {
                bullet.move();
            }

            foreach (Enemy enemy in enemies) // Loops through every enemy in the list of enemies and does the code below for each enemy
            {
                enemy.move();

                if (enemy.reachedGoal == true) // If they actually get through (leaking) the player lose a life and the enemy gets added to the list of removed enemies
                {
                    removedEnemies.Add(enemy);
                    lives -= 1;
                }

                else if (enemy.hp <= 0) // If the health of the enemy goes below 0 or exactly 0 then the enemy also gets added to the list of removed enemies
                {
                    removedEnemies.Add(enemy);
                }
            }

            if (removedEnemies.Count >= 1) // If the list of removed enemies isn't empty then go through every enemy in the list and remove them from the game (and remove them from the lists they are a member of)
            {
                for (int i = 0; i < removedEnemies.Count; i++)
                {
                    enemies.Remove(removedEnemies[i]);
                    removedEnemies[i] = null;
                    removedEnemies.Remove(removedEnemies[i]);
                }
            }

            if (removedBullets.Count >= 1) // The same thing as the enemies (the bullets gets removed from the game)
            {
                for (int i = 0; i < removedBullets.Count; i++)
                {
                    bullets.Remove(removedBullets[i]);
                    removedBullets[i] = null;
                    removedBullets.Remove(removedBullets[i]);
                }
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) // some standard code added to every monogame project
                this.Exit();

            base.Update(gameTime); // Standard code to make the update function work aswell
        }

        protected override void Draw(GameTime gameTime) // This function also runs every frame but the reason for this one is to draw everything (graphically) to the screen
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(); // Begin drawing stuff

            for (int y = 0; y < tilesY; y++) // Drawing all the tiles in a double "for loop" (loopception)
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
            }

            foreach (Enemy enemy in enemies)
            {
                Texture2D currentTexture = enemyTextures[enemy.type];
                spriteBatch.Draw(currentTexture, enemy.position, null, Color.White, 0, new Vector2(32, 32), new Vector2(1, 1), SpriteEffects.None, 0);
            }

            foreach (Tower tower in towers)
            {
                spriteBatch.Draw(tower.texture, tower.realPosition, null, Color.White, 0, new Vector2(32, 32), new Vector2(1, 1), SpriteEffects.None, 0);
            }

            spriteBatch.End(); // Ends drawing stuff 

            base.Draw(gameTime); // Also some basic code to make the draw function work
        }
    }
}
