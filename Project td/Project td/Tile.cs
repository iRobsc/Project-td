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
    public class Tile
    {
        public static int trueWidth = 32; // Width and height of the tile
        public static int trueHeight = 32;
        public static float width = 1; // This makes the pictures as big as the files say they are, e.g. if it's a 32x32 picture then it's 32x32 ingame (Don't change these numbers)
        public static float height = 1;
        public int tileType = 0;
        public Vector2 position;
    }
}
