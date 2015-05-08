using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
using System.Diagnostics;

namespace WordUp
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        String currentWord;
        Dictionary<char, Texture2D> letterDictionary = new Dictionary<char, Texture2D>();
        List<Letter> wordLetterList = new List<Letter>();

        GameSpeed gameSpeed = GameSpeed.VERY_SLOW;
        bool gameSpeedChanged = false;
        bool keyDownPressed = false;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";


            // set resolution
            graphics.PreferredBackBufferWidth = GameConstants.WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = GameConstants.WINDOW_HEIGHT;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load letter textures
            for (char c = 'a'; c <= 'z'; c++)
            {

                letterDictionary.Add(c,  Content.Load<Texture2D>(c + "black_1"));
            }

            currentWord = "unbelievable";
            stringToList(currentWord);

            /*
            // Load word list

            string lineOfText;
            int i = 0;
            using (var stream = TitleContainer.OpenStream("words.txt"))
            {
                using (var reader = new StreamReader(stream))
                {
                    while ((lineOfText = reader.ReadLine()) != null)
                    {
                        words[i++] = lineOfText;
                    }
                }
            }  
             */
             

             
            

            // TODO: use this.Content to load your game content here
        }
        private void stringToList(String word)
        {
            int xLoc = 10;

            foreach(char c in word)
            {
                wordLetterList.Add(new Letter(c, xLoc, 0, letterDictionary[c]));
                xLoc += (letterDictionary[c].Width / 4);
            }
        }
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            KeyboardState keyboard = Keyboard.GetState();

            if(keyboard.IsKeyDown(Keys.Up))
            {
                keyDownPressed = true;
            }

            if(keyDownPressed == true)
            {

                if (keyboard.IsKeyUp(Keys.Up))
                {
                    if (gameSpeed != GameSpeed.ULTRA_FAST)
                    {
                        gameSpeed++;
                    }
                    else
                    {
                        gameSpeed = GameSpeed.VERY_SLOW;
                    }

                    foreach (Letter letter in wordLetterList)
                    {
                        letter.Speed = gameSpeed;
                    }
                    keyDownPressed = false;
                    Debug.WriteLine("Game speed changed to " + gameSpeed.ToString());
                } 
           }
      

            // TODO: Add your update logic here

            foreach(Letter letter in wordLetterList)
            {
                letter.Update(gameTime);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            // TODO: Add your drawing code here
            
            foreach(Letter letter in wordLetterList)
            {
                letter.Draw(spriteBatch);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
        // change the game speed
        private void ChangeSpeed(GameSpeed speed)
        {
            gameSpeed = speed;
            foreach(Letter letter in wordLetterList)
            {
                letter.Speed = gameSpeed;
            }
        }

    }
}
