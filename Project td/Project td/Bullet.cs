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
        public Tower parent; // This is the tower that owns this bullet
        public Vector2 position;
        public float speed;

        public Bullet(Tower parent, Texture2D bulletTexture)
        {
            this.parent = parent; // Sets the input parent to the class parent
            target = parent.target; // The towers target is also the bullets target
            texture = bulletTexture;
            position = parent.realPosition; // Puts the bullet in the middle of the towers position (in the origin of the tower)
            speed = 5; // bullet speederino
        }

        public Vector2 calculateDirection() // Uses trigonometry to calculate what direction the bullet should be travelling and the return value is given as a Vector2 ( which means a 2D vector )
        {
            float deltaY = target.position.Y - position.Y; // The distance between the target and the current position (Y)
            float deltaX = target.position.X - position.X; // The distance between the target and the current position (X)

            return new Vector2((float)Math.Cos(Math.Atan2(deltaY, deltaX)), // Returns the direction vector calculated with (Math.Atan2(deltaY, deltaX)) Which is the angle of the bullets direction. Explanation: http://i.imgur.com/jlC8F3J.png
                               (float)Math.Sin(Math.Atan2(deltaY, deltaX))); 
        }

        public double distance()
        {
            float deltaY = position.Y - target.position.Y;
            float deltaX = position.X - target.position.X;

            return Math.Abs(Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2))); // This is just the regular Pythagoras (Math.Pow) is a method to write deltaY/deltaX to the power of 2
        }

        public void move()
        {
            if (distance() <= speed) // If the speed is higher or equal than the distance left to the object then reduce the targets hp and remove the bullet (It's a hit!)
            {
                target.hp -= parent.damage;
                main.removedBullets.Add(this);
            }

            position += speed * calculateDirection(); // This is the actual movement, it moves with speed times the direction (This is also a clever use of the return function)
        }
    }
}
