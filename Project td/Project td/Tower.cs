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
    public abstract class Tower
    {
        public float damage, range, fireRate, cooldown = 0;
        public Vector2 tilePosition; // The tile the tower is placed on
        public Vector2 realPosition; // The coordinates where the tower is located
        public Texture2D texture, bulletTexture;
        public Enemy target = null;

        public Tower() // Just an empty constructor
        {
        }

        public void shoot() // Adds a bullet to the field
        {
            Bullet bullet = new Bullet(this, bulletTexture);
            main.bullets.Add(bullet);
        }

        public void setTarget(int index)
        {
            float enemyPos = (float)(Math.Pow(main.enemies[index].position.Y - realPosition.Y, 2) + // This is the circle equation, but just the left side of the equation where the enemies position is subtracted by the towers position. The circle equation: https://upload.wikimedia.org/math/a/7/7/a7714e2972d817c45f9615006c5242cb.png
                                     Math.Pow(main.enemies[index].position.X - realPosition.X, 2));

            double calculatedRange = Math.Pow(range, 2); // The right side of the circle equation where Math.Pow is used as range to the power of 2. The circle equation: https://upload.wikimedia.org/math/a/7/7/a7714e2972d817c45f9615006c5242cb.png

            if (target != null) // If the tower has a target and the target isn't an enemy on the field, then set the target to nothing (null)
            {
                if (!main.enemies.Contains(target)) // the " ! " means that main.enemies does NOT contain target
                {
                    target = null;
                }
            }

            if (calculatedRange > enemyPos) // If the enemys position is inside the towers range, then set the target of the tower to the enemy (This is where the circle equation gets checked)
            {
                if (target == null)
                {
                    target = main.enemies[index];
                }
            }
            else if (target == main.enemies[index]) // Else if the target is outside the range and the tower has the enemy as a target, then set the towers target to nothing (null)
            {
                target = null;
            }
        }

    }
}

