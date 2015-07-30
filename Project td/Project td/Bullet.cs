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
    public class Bullet
    {
        public Texture2D texture;
        public Enemy target;
        public Tower parent;
        public Vector2 position;
        public float speed;

        public Bullet(Tower parent, Texture2D bulletTexture)
        {
            this.parent = parent;
            target = parent.target;
            texture = bulletTexture;
            position = parent.realPosition;
            speed = 5;
        }

        public Vector2 calculateDirection()
        {
            float deltaY = position.Y - target.position.Y;
            float deltaX = position.X - target.position.X;

            return -(new Vector2((float)Math.Cos(Math.Atan2(deltaY, deltaX)),
                               (float)Math.Sin(Math.Atan2(deltaY, deltaX))));
        }

        public double distance()
        {
            float deltaY = position.Y - target.position.Y;
            float deltaX = position.X - target.position.X;

            return Math.Abs(Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2)));
        }

        public void move()
        {
            if (distance() <= speed)
            {
                target.hp -= parent.damage;
                main.removedBullets.Add(this);
            }

            position += speed * calculateDirection();
        }
    }
}
