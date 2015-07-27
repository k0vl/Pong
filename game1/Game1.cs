#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;

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
		private Rectangle gameBoundries;
		static Random random = new Random();

		public const int PADDLE_OFFSET = 30;

		#if __ANDROID__
		public const float DeviceScale = 2.5f;
		#else
		public const float DeviceScale = 1f;
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

			Vector2 humanPaddleLocation = new Vector2(
				PADDLE_OFFSET
				, (gameBoundries.Height - paddleTexture.Height * Game1.DeviceScale)/2
			);
			Vector2 computerPaddleLocation = new Vector2(
				gameBoundries.Width - paddleTexture.Width * Game1.DeviceScale - PADDLE_OFFSET 
				, (gameBoundries.Height - paddleTexture.Height * Game1.DeviceScale)/2
			);

			List<Texture2D> boostTexture = new List<Texture2D>();
			for(int i = 1; i <= 7; i++)
			{
				boostTexture.Add(Content.Load<Texture2D>("Boost/Boost"+i));
			}

			gameObjects = new GameObjects{
				PlayerPaddle = new Paddle(paddleTexture, humanPaddleLocation, gameBoundries, PlayerTypes.Human), 
				ComputerPaddle = new Paddle(paddleTexture, computerPaddleLocation , gameBoundries, PlayerTypes.Computer), 
				Ball = new List<Ball>{new Ball(Content.Load<Texture2D>("Ball"), Vector2.Zero, gameBoundries)},
				Score = new Score(Content.Load<SpriteFont>("fonts/Arial"), gameBoundries),
				Controller = new Controller(),
				Boost = new List<Boost>{},
				GameBoundries = gameBoundries,
				BoostTexture = boostTexture,
				BallTexture = Content.Load<Texture2D>("Ball")
			};

			gameObjects.Ball[0].AttachTo(gameObjects.PlayerPaddle);
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

			gameObjects.Controller.Update(gameTime, gameObjects);
			foreach (Ball ball in gameObjects.Ball)
			{
				ball.Update(gameTime, gameObjects);
			}
			gameObjects.PlayerPaddle.Update(gameTime, gameObjects);
			gameObjects.ComputerPaddle.Update(gameTime, gameObjects);
			gameObjects.Score.Update(gameTime, gameObjects);
			if(gameObjects.Boost.Count < 7)
			{
				if(gameObjects.Ball[0].attachedToPaddle == null && random.Next(10, 100) == 20)
				{
					int LocX = random.Next(PADDLE_OFFSET,(int)(gameBoundries.Width-gameObjects.BoostTexture[1].Width*DeviceScale-PADDLE_OFFSET));
					int LocY = random.Next(0,(int)(gameBoundries.Height-gameObjects.BoostTexture[1].Height*DeviceScale));
					int boostTypes = random.Next(0,Enum.GetNames(typeof(BoostTypes)).Length);
					gameObjects.Boost.Add(new Boost(gameObjects.BoostTexture[boostTypes], new Vector2(LocX, LocY), gameBoundries, (BoostTypes)boostTypes));
				}
			}
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
			gameObjects.PlayerPaddle.Draw(spriteBatch);
			gameObjects.ComputerPaddle.Draw(spriteBatch);
			foreach (Ball ball in gameObjects.Ball)
			{
				ball.Draw(spriteBatch);
			}
			gameObjects.Score.Draw(spriteBatch);
			foreach (Boost boost in gameObjects.Boost)
			{
				boost.Draw(spriteBatch);
			}
			//spriteBatch.Draw (paddle, Vector2.Zero, Color.White);
			spriteBatch.End ();
            
			base.Draw (gameTime);
		}
	}

}

