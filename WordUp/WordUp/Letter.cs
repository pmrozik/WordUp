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
    /// <summary>
    /// This class represents a single letter in the game. 
    /// </summary>

    class Letter 
    {
        // letter representation
        private char c;
        private Rectangle drawRectangle;
        private Texture2D texture;
        
        // speed of the falling letter
        // velocity is updated via gameSpeed constants
        
        private GameSpeed gameSpeed;
        private bool gameSpeedChanged;
        private Vector2 velocity;
        
        // whether letter has gone off screen
        private bool offScreen;

        /// <summary>
        ///  Initializes a new Letter object with the letter, x location, y location, texture, and initial speed
        /// </summary>
        /// <param name="c">represents a single letter of the alphabet</param>
        /// <param name="x">x location</param>
        /// <param name="y">y location</param>
        /// <param name="texture">texture for the letter</param>
        /// <param name="gameSpeed">game speed represented by GameSpeed constants</param>
        public Letter(char c, int x, int y, Texture2D texture, GameSpeed gameSpeed)
        {
            this.c = c;
            this.texture = texture;
            drawRectangle = new Rectangle(x, y, texture.Width, texture.Height);
            
            // Starting speed
            this.gameSpeed = gameSpeed;
            gameSpeedChanged = true;
            velocity = new Vector2(0, GameConstants.VERY_SLOW);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, drawRectangle, null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 1f);
        }
        /// <summary>
        /// Letter logic for updating the letter during gameplay
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            // Change game speed if necessary
            if(gameSpeedChanged)
            {
                switch(gameSpeed)
                {
                    case GameSpeed.VERY_SLOW:
                        velocity.Y = GameConstants.VERY_SLOW;
                        break;
                    case GameSpeed.SLOW:
                        velocity.Y = GameConstants.SLOW;
                        break;
                    case GameSpeed.NORMAL:
                        velocity.Y = GameConstants.NORMAL;
                        break;
                    case GameSpeed.FAST:
                        velocity.Y = GameConstants.FAST;
                        break;
                    case GameSpeed.VERY_FAST:
                        velocity.Y = GameConstants.VERY_FAST;
                        break;
                    case GameSpeed.ULTRA_FAST:
                        velocity.Y = GameConstants.ULTRA_FAST;
                        break;
                    default:
                        break;
                }
                gameSpeedChanged = false;
               
            }
            
            // Update letter location
            drawRectangle.Y += (int)(velocity.Y * gameTime.ElapsedGameTime.TotalMilliseconds);

            // Check whether letter has gone off screen
            if (drawRectangle.Y > (GameConstants.WINDOW_HEIGHT - GameConstants.DASHBOARD_HEIGHT))
            {
                offScreen = true;
                drawRectangle.Y = 0;
            }
            
        }
        /// <summary>
        /// Allows the user to get and set the game speed using a GameSpeed constant
        /// </summary>
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
        /// <summary>
        /// Allows user to access the character representation of the letter
        /// </summary>
        public char Character
        {
            get
            {
                return c;
            }
        }
        /// <summary>
        /// Allows user to change X location of the letter
        /// </summary>
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
        /// <summary>
        /// Allows user to change y location of the letter
        /// </summary>
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
        /// <summary>
        /// Allows user to change the letter image
        /// </summary>
        public Texture2D Texture
        {
            set
            {
                texture = value;
            }
        }
        /// <summary>
        /// Returns the draw rectangle
        /// </summary>
        public Rectangle DrawRectangle
        {
            get
            {
                return drawRectangle;
            }
    
        }
        /// <summary>
        /// Returns information on whether the letter has gone off screen
        /// </summary>
        public bool OffScreen
        {
            get
            {
                return offScreen;
            }
        }
    }
}
