using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace WordUp
{
    /// <summary>
    /// This static class holds all the important game constants 
    /// </summary>
    public static class GameConstants
    {
        // resolution
     
        public const int WINDOW_WIDTH = 800;
     
        public const int WINDOW_HEIGHT = 600;

        // speed constants

        public const float VERY_SLOW = 0.1f;

        public const float SLOW = 0.15f;

        public const float NORMAL = 0.2f;

        public const float FAST = 0.25f;

        public const float VERY_FAST = 0.3f;

        public const float ULTRA_FAST = 0.35f;
       
        // Dashboard sizes

        public const int DASHBOARD_WIDTH = WINDOW_WIDTH;
        public const int DASHBOARD_HEIGHT = 60;

        // Font sizes in pixels
        public const int ARIAL20_PIXELS = 15;

        // Score location

        public static readonly Vector2 SCORE_LOCATION = new Vector2(725, 25);
    }
    
}
