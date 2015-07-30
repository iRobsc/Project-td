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
        public Vector2 tilePosition;
        public Vector2 realPosition;
        public Texture2D texture, bulletTexture;
        public Enemy target = null;

        public Tower()
        {
        }

        public void shoot()
        {
            Bullet bullet = new Bullet(this, bulletTexture);
            main.bullets.Add(bullet);
        }

        public void setTarget(int index)
        {
            float enemyPos = (float)(Math.Pow(main.enemies[index].position.Y - realPosition.Y, 2) +
                                     Math.Pow(main.enemies[index].position.X - realPosition.X, 2));

            double calculatedRange = Math.Pow(range, 2);

            if (target != null)
            {
                if (!main.enemies.Contains(target))
                {
                    target = null;
                }
            }

            if (calculatedRange > enemyPos)
            {
                if (target == null)
                {
                    target = main.enemies[index];
                }
            }
            else if (target == main.enemies[index])
            {
                target = null;
            }
        }

    }
}

