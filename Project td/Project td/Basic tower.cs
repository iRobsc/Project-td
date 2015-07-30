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
    public class Basic_tower : Tower
    {
        public Basic_tower(Vector2 pos)
        {
            damage = 3;
            range = 100;
            fireRate = 0.5f;
            texture = main.towerTexture1;
            bulletTexture = main.bulletTexture1;
            tilePosition = pos;
            realPosition = main.tiles[(int)tilePosition.Y, (int)tilePosition.X].position;
        }
    }
}