#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

#endregion

namespace game1
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		private GameObjects gameObjects;
		private Paddle playerPaddle;
		private Paddle computerPaddle;
		private Ball ball;
		private Score score;
		private Rectangle gameBoundries;
		#if __ANDROID__
		public const float DeviceScale = 2.5f;
		#else
		public const float DeviceScale = 1;
		#endif
		//private Texture2D paddle;

		public Game1 ()
		{
			graphics = new GraphicsDeviceManager (this);
			Content.RootDirectory = "Content";	            
			graphics.IsFullScreen = false;	
			graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft;
			graphics.ApplyChanges();
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize ()
		{
			// TODO: Add your initialization logic here

			IsMouseVisible = true;
			TouchPanel.EnabledGestures = GestureType.VerticalDrag | GestureType.Flick | GestureType.Tap;

			base.Initialize ();
				
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent ()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch (GraphicsDevice);

			#if __ANDROID__
			gameBoundries = new Rectangle(0, 0, Window.ClientBounds.Height, Window.ClientBounds.Width);
			#else
			gameBoundries = new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height);
			#endif
			Texture2D paddleTexture = Content.Load<Texture2D>("Paddle");

			Vector2 humanPaddleLocation = new Vector2(30, (gameBoundries.Height - paddleTexture.Height * Game1.DeviceScale)/2);
			playerPaddle = new Paddle(paddleTexture, humanPaddleLocation, gameBoundries, PlayerTypes.Human);

			Vector2 computerPaddleLocation = new Vector2(gameBoundries.Width - paddleTexture.Width * Game1.DeviceScale - 30 , (gameBoundries.Height - paddleTexture.Height * Game1.DeviceScale)/2);
			computerPaddle = new Paddle(paddleTexture, computerPaddleLocation , gameBoundries, PlayerTypes.Computer);

			ball = new Ball(Content.Load<Texture2D>("Ball"), Vector2.Zero, gameBoundries);
			ball.AttachTo(playerPaddle);

			score = new Score(Content.Load<SpriteFont>("fonts/Arial"), gameBoundries);

			gameObjects = new GameObjects{
				PlayerPaddle = playerPaddle, 
				ComputerPaddle = computerPaddle, 
				Ball = ball,
				Score = score
			};
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update (GameTime gameTime)
		{
			// For Mobile devices, this logic will close the Game when the Back button is pressed
			// Exit() is obsolete on iOS
			#if !__IOS__
			if (GamePad.GetState (PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
			    Keyboard.GetState ().IsKeyDown (Keys.Escape)) {
				Exit ();
			}
			#endif

			gameObjects.TouchInput = new TouchInput();
			GetTouchInput();

			ball.Update(gameTime, gameObjects);
			playerPaddle.Update(gameTime, gameObjects);
			computerPaddle.Update(gameTime, gameObjects);

			score.Update(gameTime, gameObjects);

			base.Update (gameTime);
		}

		private void GetTouchInput()
		{
			while(TouchPanel.IsGestureAvailable)
			{
				var gesture = TouchPanel.ReadGesture();
				if(gesture.Delta.Y > 0) {gameObjects.TouchInput.Down = true;}
				if(gesture.Delta.Y < 0) {gameObjects.TouchInput.Up = true;}
				if(gesture.GestureType == GestureType.Tap)
				{gameObjects.TouchInput.Tapped = true;}
			}
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw (GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear (Color.White);
		
			//TODO: Add your drawing code here
			spriteBatch.Begin ();
			playerPaddle.Draw(spriteBatch);
			computerPaddle.Draw(spriteBatch);
			ball.Draw(spriteBatch);
			score.Draw(spriteBatch);
			//spriteBatch.Draw (paddle, Vector2.Zero, Color.White);
			spriteBatch.End ();
            
			base.Draw (gameTime);
		}
	}

}

