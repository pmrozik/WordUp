using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;


namespace WordUp
{
    class Letter
    {
        private char c;
        private Rectangle drawRectangle;
        private Texture2D texture;
        private TimeSpan elapsedFrameTime;
        private GameSpeed gameSpeed;
        private bool gameSpeedChanged;
        private bool offScreen;
        private long letterFallSpeed;
        Vector2 velocity;
        


        
        public Letter(char c, int x, int y, Texture2D texture)
        {
            this.c = c;
            this.texture = texture;
            drawRectangle = new Rectangle(x, y, texture.Width, texture.Height);
            gameSpeed = GameSpeed.NORMAL;
            gameSpeedChanged = true;
            velocity = new Vector2(0, GameConstants.VERY_SLOW_INTERVAL);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, drawRectangle, Color.White);
        }
        public void Update(GameTime gameTime)
        {
           
            if(gameSpeedChanged)
            {
                switch(gameSpeed)
                {
                    case GameSpeed.VERY_SLOW:
                        velocity.Y = GameConstants.VERY_SLOW_INTERVAL;
                        break;
                    case GameSpeed.SLOW:
                        velocity.Y = GameConstants.SLOW_INTERVAL;
                        break;
                    case GameSpeed.NORMAL:
                        velocity.Y = GameConstants.NORMAL_INTERVAL;
                        break;
                    case GameSpeed.FAST:
                        velocity.Y = GameConstants.FAST_INTERVAL;
                        break;
                    case GameSpeed.VERY_FAST:
                        velocity.Y = GameConstants.VERY_FAST_INTERVAL;
                        break;
                    case GameSpeed.ULTRA_FAST:
                        velocity.Y = GameConstants.ULTRA_FAST_INTERVAL;
                        break;
                    default:
                        break;
                }
                gameSpeedChanged = false;
               
            }
            
            drawRectangle.Y += (int)(velocity.Y * gameTime.ElapsedGameTime.TotalMilliseconds);

            if (drawRectangle.Y > GameConstants.WINDOW_HEIGHT)
            {
                offScreen = true;
                drawRectangle.Y = 0;
            }
            
        }
        public GameSpeed Speed
        {
            get
            {
                return gameSpeed;
            }
            set
            {
                gameSpeedChanged = true;
                gameSpeed = value; 
            }
        }
        public char Character
        {
            get
            {
                return c;
            }
        }
        public int X
        {
            get
            {
                return drawRectangle.X;
            }
            set
            {
                drawRectangle.X = value;
            }
        }
        public int Y
        {
            get
            {
                return drawRectangle.Y;
            }
            set
            {
                drawRectangle.Y = value;
            }
        }
        public Texture2D Texture
        {
            set
            {
                texture = value;
            }
        }
        public Rectangle DrawRectangle
        {
            get
            {
                return drawRectangle;
            }
    
        }
        public bool OffScreen
        {
            get
            {
                return offScreen;
            }
        }
    }
}
