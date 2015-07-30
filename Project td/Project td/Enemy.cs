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
    public class Enemy
    {
        public int type;
        public float hp = 10;
        public float speed = 1;
        public Vector2 position;
        public Vector2 target;
        public List<Vector2> wavePath;
        public int currentNode = 0;
        public bool reachedGoal = false;

        public Enemy(int enemyType, List<Vector2> wavePath)
        {
            this.type = enemyType;
            this.wavePath = wavePath;
        }

        public double distance()
        {
            float deltaY = position.Y - target.Y;
            float deltaX = position.X - target.X;

            return Math.Abs(Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2)));
        }

        public Vector2 calculateDirection()
        {
            float deltaY = position.Y - target.Y;
            float deltaX = position.X - target.X;

            return -(new Vector2((float)Math.Cos(Math.Atan2(deltaY, deltaX)),
                               (float)Math.Sin(Math.Atan2(deltaY, deltaX))));
        }

        public void move()
        {
            if (distance() <= speed && currentNode + 1 < wavePath.Count)
            {
                target = main.tiles[(int)wavePath[currentNode + 1].X,
                                    (int)wavePath[currentNode + 1].Y].position;
                currentNode += 1;
            }

            else if (distance() <= speed && currentNode + 1 == wavePath.Count)
            {
                reachedGoal = true;
            }

            position += speed * calculateDirection();

        }
    }
}
