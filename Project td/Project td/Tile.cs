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
        public static int trueWidth = 32;
        public static int trueHeight = 32;
        public static float width = 1;
        public static float height = 1;
        public int tileType = 0;
        public Vector2 position;
    }
}
