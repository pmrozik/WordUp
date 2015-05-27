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
    /// WordUp Game.
    /// Designed and created by Pawel Mrozik.
    /// pmrozik@gmail.com
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        // Game states

        enum GameState
        {
            MainMenu,
            Options,
            Playing,
        }

        // Starting game state - menu
        GameState currentGameState = GameState.MainMenu;

        // Buttons for the menu
        cButton btnPlay;
        cButton btnOptions;
        cButton btnExit;

        // Background for the menu
        Texture2D menuBackground;

        // The word that is currently falling
        private String currentWord;

        // Game dashboard
        private Texture2D blackRectangleTexture;
        private Rectangle blackRectangle;
        private bool rectangleDrawn = false;

        // Holds letters typed by user
        private List<char> typedLetters = new List<char>();

        // Fonts
        private SpriteFont arialFont;
  
        // Allows letter graphics retrieval
        private Dictionary<char, List<Texture2D>> letterDictionary = new Dictionary<char, List<Texture2D>>();
        
        // Word dictionary
        private Dictionary<string, bool> wordDictionary = new Dictionary<string, bool>();

        // Contains each letter of the word
        private List<Letter> wordLetterList = new List<Letter>();
        
        // Game lives textures

        private Texture2D appleTexture;
 
        private List<Texture2D> livesList = new List<Texture2D>();

        // This is set in LoadContent()
        private int letterWidth;

        // Initial game speed
        private GameSpeed gameSpeed = GameSpeed.VERY_SLOW;
        
        private bool keyDownPressed = false;
        private bool backspaceDown = false;
        private bool shiftBackspaceDown = false;
        private bool enterDown = false;
        private bool offScreen = false;
       
        // Random seed for the game
        private Random rand = new Random();
        
        // Game score
        private int score;

        // Sounds

        private SoundEffect keyPressSound;
        private SoundEffect wordSuccessSound;
        private SoundEffect wordDeleteSound;
        private SoundEffect letterDeleteSound;
        private SoundEffect lifeLostSound;
        private SoundEffect errorSound;

        private Song gameMusic; 

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

            // Load menu items
            menuBackground = Content.Load<Texture2D>("menu\\background");
            
            btnPlay = new cButton(Content.Load<Texture2D>("menu\\play"), graphics.GraphicsDevice);
            btnPlay.setPosition(new Vector2(350, 250));
            
            btnOptions = new cButton(Content.Load <Texture2D>("menu\\options"), graphics.GraphicsDevice);
            btnOptions.setPosition(new Vector2(350, 350));
            
            btnExit = new cButton(Content.Load<Texture2D>("menu\\exit"), graphics.GraphicsDevice);
            btnExit.setPosition(new Vector2(350, 450));

            // Dashboard items

            blackRectangleTexture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            blackRectangleTexture.SetData(new[] { Color.Navy });
            blackRectangle = new Rectangle(0, GameConstants.WINDOW_HEIGHT - GameConstants.DASHBOARD_HEIGHT,
                                             GameConstants.DASHBOARD_WIDTH, GameConstants.DASHBOARD_HEIGHT);

            // Load fonts

            arialFont = Content.Load<SpriteFont>("fonts\\Arial20");

            // Load sounds

            keyPressSound = Content.Load<SoundEffect>("sounds\\keypress");
            wordSuccessSound = Content.Load<SoundEffect>("sounds\\wordsuccess");
            lifeLostSound = Content.Load<SoundEffect>("sounds\\lifelost");
            // Source: http://freesound.org/people/Autistic%20Lucario/sounds/142608/
            errorSound = Content.Load<SoundEffect>("sounds\\error");
            // Source: http://www.freesfx.co.uk/download/?type=mp3&id=9630
            wordDeleteSound = Content.Load<SoundEffect>("sounds\\worddelete");
            // Source also freesfx
            letterDeleteSound = Content.Load<SoundEffect>("sounds\\deleteletter");

            // Load and play game music

            gameMusic = Content.Load<Song>("sounds\\shiningstar");

            // Load letter textures
            List<Texture2D> textureList; 

            for (char c = 'a'; c <= 'z'; c++)
            {
                textureList = new List<Texture2D>(10);
                textureList.Add(Content.Load<Texture2D>("letters\\64\\" + c + "_black"));
                textureList.Add(Content.Load<Texture2D>("letters\\64\\" + c + "_blue"));
                textureList.Add(Content.Load<Texture2D>("letters\\64\\" + c + "_dg"));
                textureList.Add(Content.Load<Texture2D>("letters\\64\\" + c + "_gold"));
                textureList.Add(Content.Load<Texture2D>("letters\\64\\" + c + "_grey"));
                textureList.Add(Content.Load<Texture2D>("letters\\64\\" + c + "_lg"));
                textureList.Add(Content.Load<Texture2D>("letters\\64\\" + c + "_orange"));
                textureList.Add(Content.Load<Texture2D>("letters\\64\\" + c + "_pink"));
                textureList.Add(Content.Load<Texture2D>("letters\\64\\" + c + "_red"));
                textureList.Add(Content.Load<Texture2D>("letters\\64\\" + c + "_violet"));
               
                letterDictionary.Add(c,  textureList);
                textureList = null;
            }
            // Set the letter width 
            letterWidth = letterDictionary['a'][0].Width;

            // Load lives texture

            appleTexture = Content.Load<Texture2D>("apple");

            // Add three lives to list

            livesList.Add(appleTexture);
            livesList.Add(appleTexture);
            livesList.Add(appleTexture);

            // Load word list

            string lineOfText;

            using (var stream = TitleContainer.OpenStream("words.txt"))
            {
                using (var reader = new StreamReader(stream))
                {
                    while ((lineOfText = reader.ReadLine()) != null)
                    {
                        // Remove all periods from words
                        if(lineOfText.Trim().Contains('.'))
                        {
                            lineOfText.Replace(".", "");
                        }
                        // Check if duplicates exist in the list that's being loaded
                        lineOfText = lineOfText.ToLower().Trim();
                        if(wordDictionary.ContainsKey(lineOfText))
                        {
                            // Don't add again
                        }
                        else
                        { 
                            wordDictionary.Add(lineOfText.ToLower().Trim(), false);
                        }
                    }
                }
            }

            currentWord = GetRandomLetterCombo();
            stringToList(currentWord);

            // Play game music

            MediaPlayer.Play(gameMusic);
        }
        private string ListToString()
        {
            string wordString = string.Empty;
            
            foreach(char c in typedLetters)
            {
                wordString += c;
                
            }
            
            return wordString;
        }
        private void stringToList(String word)
        {
           
            // total length in pixels of the letters displayed
            int wordPixels = (word.Length * letterWidth);

            // calculate initial location of the first letter so that letters are centered on screen
            int xLoc = (GameConstants.WINDOW_WIDTH - wordPixels) / 2;
             
            wordLetterList.Clear();

            // convert word to lower case
            word = word.ToLower();

            Texture2D tmpTexture;

            foreach(char c in word)
            {
                // retrieves random color
                tmpTexture = GetRandomLetterTexture(c);
                wordLetterList.Add(new Letter(c, xLoc, 0, tmpTexture, gameSpeed));
                xLoc += tmpTexture.Width;
            }

            
        }
        private string GetRandomLetterCombo()
        {
            Random rand = new Random();

            string tmpWord = wordDictionary.ElementAt(rand.Next(0, wordDictionary.Count)).Key;

            return StringTools.Alphabetize(tmpWord);

        }
        /// <summary>
        /// Retrieves random color of letter texture. 
        /// </summary>
        /// <param name="c">letter from a to z</param>
        /// <returns></returns>
        private Texture2D GetRandomLetterTexture(char c)
        { 
            return letterDictionary[c][rand.Next(0, 9)];
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

            MouseState mouse = Mouse.GetState();

            switch (currentGameState)
            {
                case GameState.MainMenu:
                    IsMouseVisible = true;
                    if (btnPlay.isClicked) currentGameState = GameState.Playing;
                    btnPlay.Update(mouse);
                    if (btnOptions.isClicked) currentGameState = GameState.MainMenu;
                    btnOptions.Update(mouse);
                    if (btnExit.isClicked) currentGameState = GameState.MainMenu;
                    btnExit.Update(mouse);
                    break;
                case GameState.Playing:
                    IsMouseVisible = false;
                    Play(gameTime);
                    break;
            }




            

            base.Update(gameTime);
        }
        private void clearLetters()
        {
            // 1. Get new word
            currentWord = GetRandomLetterCombo();
            // 2. Convert new word to list form
            stringToList(currentWord);
            // 3. Clear typed letters
            typedLetters.Clear();
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGray);

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            switch (currentGameState)
            {
                case GameState.MainMenu:
                    spriteBatch.Draw(menuBackground, new Rectangle(0, 0, GameConstants.WINDOW_WIDTH, GameConstants.WINDOW_HEIGHT), 
                        Color.White);
                    btnPlay.Draw(spriteBatch);
                    btnOptions.Draw(spriteBatch);
                    btnExit.Draw(spriteBatch);
                    break;
                case GameState.Options:
                    break;
                case GameState.Playing:
                    DrawPlay();
                    break;
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
        private void DrawPlay()
        {
            // Draw game dashboard

            // Draw rectangle only once
            if (!rectangleDrawn)
            {
                spriteBatch.Draw(blackRectangleTexture, blackRectangle, null,
                                             Color.Navy, 0.0f, Vector2.Zero, SpriteEffects.None, 0.5f);
            }

            // Draw typed letters

            int typedLettersXLoc = (GameConstants.WINDOW_WIDTH - typedLetters.Count * GameConstants.ARIAL20_PIXELS) / 2;
            spriteBatch.DrawString(arialFont, ListToString(), new Vector2(typedLettersXLoc, 550), Color.Yellow);


            // Draw falling letters
            foreach (Letter letter in wordLetterList)
            {
                letter.Draw(spriteBatch);
            }

            // Draw score and health

            spriteBatch.DrawString(arialFont, score.ToString(), GameConstants.SCORE_LOCATION, Color.Red);

            for (int i = 0; i < livesList.Count(); i++)
            {
                int xLoc = (int)GameConstants.LIVES_LOCATION.X + (livesList[i].Width * i);
                int yLoc = (int)GameConstants.LIVES_LOCATION.Y;
                Rectangle drawRectangle = new Rectangle(xLoc, yLoc, livesList[i].Width, livesList[i].Height);
                spriteBatch.Draw(livesList[i], drawRectangle, Color.White);
            }
        }
        private void Play(GameTime gameTime)
        {
            // Allows user to enter words
            KeyboardState keyboard = Keyboard.GetState();
            char pressedChar = KeyboardProcessor.GetLetter(keyboard);

            if (pressedChar != ' ')
            {
                // Check whether the letter is one of the falling ones
                if (currentWord.Contains(pressedChar.ToString()))
                {

                    keyPressSound.Play();
                    typedLetters.Add(pressedChar);
                }
                else
                {
                    errorSound.Play();
                    Debug.WriteLine("Current word: {0}", currentWord);
                    Debug.WriteLine("Current word doesn't have {0}", pressedChar);
                }
            }
            // User presses enter, check if word exists in dictionary
            if (typedLetters.Count > 0 && keyboard.IsKeyDown(Keys.Enter))
            {
                enterDown = true;
            }
            if (enterDown && keyboard.IsKeyUp(Keys.Enter))
            {
                string word = "";

                foreach (char c in typedLetters)
                {
                    word += c;
                }
                Debug.Write("Checking the following word: {0} ...", word);

                // Check if word exists in dictionary
                if (wordDictionary.ContainsKey(word))
                {
                    Debug.WriteLine("found.");
                    // 1. Remove word from word dictionary
                    wordDictionary.Remove(word);
                    // 2. Update score
                    score += ScoreTools.GetWordScore(word);
                    Debug.WriteLine("Total score: {0}", score);
                    wordSuccessSound.Play();
                    // 3. Clear current letters
                    clearLetters();
                }
                else
                {
                    // Word not found in dictionary, play error sound
                    errorSound.Play();
                    // Clear typed letters 
                    typedLetters.Clear();
                    Debug.WriteLine("not found.");
                }
                enterDown = false;
            }

            // Remove one letter with backspace
            if (keyboard.IsKeyDown(Keys.Back))
            {
                backspaceDown = true;
            }
            if (backspaceDown && (typedLetters.Count > 0) && !shiftBackspaceDown)
            {
                if (keyboard.IsKeyUp(Keys.Back))
                {
                    letterDeleteSound.Play();
                    typedLetters.RemoveAt(typedLetters.Count - 1);
                    backspaceDown = false;
                }
            }

            // Left shift + backspace pressed to delete entire typed word
            if (keyboard.IsKeyDown(Keys.LeftShift) && keyboard.IsKeyDown(Keys.Back))
            {
                shiftBackspaceDown = true;
            }
            if (shiftBackspaceDown && (typedLetters.Count > 0))
            {
                if (keyboard.IsKeyUp(Keys.Back) || keyboard.IsKeyUp(Keys.LeftShift))
                {
                    wordDeleteSound.Play();
                    typedLetters.Clear();
                    shiftBackspaceDown = false;
                }
            }


            // Allows user to change the speed of the game via keyboard

            if (keyboard.IsKeyDown(Keys.Up))
            {
                keyDownPressed = true;
            }

            if (keyDownPressed == true)
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


            foreach (Letter letter in wordLetterList)
            {

                if (letter.OffScreen)
                {
                    offScreen = true;
                }
                letter.Update(gameTime);
            }

            // Check if letters have gone off screen
            if (offScreen)
            {
                clearLetters();
                offScreen = false;


                // Life lost

                if (livesList.Count > 0)
                {
                    livesList.RemoveAt(livesList.Count - 1);
                    lifeLostSound.Play();
                }
            }
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
