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
        public float speed = 1; // Moves x pixels each frame
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

        public double distance() // Calculates the distance between the enemy and the target node
        {
            float deltaY = position.Y - target.Y;
            float deltaX = position.X - target.X;

            return Math.Abs(Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2)));
        }

        public Vector2 calculateDirection() // Uses trigonometry to calculate what direction the enemy should be travelling and the return value is given as a Vector2 ( which means a 2D vector )
        {
            float deltaY = target.Y - position.Y; // The distance between the target and the current position (Y)
            float deltaX = target.X - position.X; // The distance between the target and the current position (X)

            return new Vector2((float)Math.Cos(Math.Atan2(deltaY, deltaX)), // Returns the direction vector calculated with (Math.Atan2(deltaY, deltaX)) Which is the angle of the enemys direction. Explanation: http://i.imgur.com/jlC8F3J.png
                               (float)Math.Sin(Math.Atan2(deltaY, deltaX)));
        }

        public void move()
        {
            if (distance() <= speed && currentNode + 1 < wavePath.Count) // If the enemy reached the desired destination (the node), then move to the next node in the wavePath and set the new node as the target
            {
                currentNode += 1;
                target = main.tiles[(int)wavePath[currentNode].X,
                                    (int)wavePath[currentNode].Y].position;
            }

            else if (distance() <= speed && currentNode + 1 == wavePath.Count) // if the enemy has reached the last node and the distance left is less or equal to the enemies speed, then set reachedGoal to true so it can be handled elsewhere
            {
                reachedGoal = true;
            }

            position += speed * calculateDirection(); // This is the actual movement, it moves with speed times the direction (This is also a clever use of the return function)
        }
    }
}
