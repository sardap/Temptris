using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using TemptureBlockGameGame.src;

namespace TemptureBlockGame.src
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class TemptureGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GameRunner _gameRunner = new GameRunner();

        public TemptureGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

			graphics.PreferredBackBufferWidth = Const.WINDOW_WIDTH;  // set this value to the desired width of your window
			graphics.PreferredBackBufferHeight = Const.WINDOW_HEIGHT;   // set this value to the desired height of your window
			graphics.ApplyChanges();
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

			_gameRunner.Initlise(Content);

			IsMouseVisible = true;

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

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

			var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

			_gameRunner.Step(deltaTime);

			if(_gameRunner.IsGameOver())
			{
				_gameRunner = new GameRunner();
				_gameRunner.Initlise(Content);
			}

			base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

			spriteBatch.Begin();

			_gameRunner.Draw(spriteBatch);

			spriteBatch.End();

			base.Draw(gameTime);
        }
    }
}
