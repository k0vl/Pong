#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

#endregion


namespace game1
{
	public enum PlayerTypes {Human, Computer}

	public class Paddle : Sprite
	{
		public const float PADDLE_SPEED = 300f;
		static Random random = new Random();
		float reactionTreshold = random.Next(10, 30);
			
		private readonly PlayerTypes playerType;

		public Paddle(Texture2D texture, Vector2 location, Rectangle gameBoundries, PlayerTypes playerType)
			: base(texture, location, gameBoundries)
		{
			this.playerType = playerType;
		}

		public override void Update(GameTime gameTime, GameObjects gameObjects)
		{
			if(playerType == PlayerTypes.Human)
			{
				if(Keyboard.GetState().IsKeyDown(Keys.Left) || gameObjects.TouchInput.Up)
				{
					Velocity = new Vector2(0, -PADDLE_SPEED);
				}

				if(Keyboard.GetState().IsKeyDown(Keys.Right) || gameObjects.TouchInput.Down)
				{
					Velocity = new Vector2(0, PADDLE_SPEED);
				}
			}
			else if(playerType == PlayerTypes.Computer)
			{
				if(gameObjects.Ball.Location.Y + gameObjects.Ball.Height < Location.Y - reactionTreshold)
				{
					Velocity = new Vector2(0, -PADDLE_SPEED);
				}
				if(gameObjects.Ball.Location.Y > Location.Y + Height + reactionTreshold)
				{
					Velocity = new Vector2(0, PADDLE_SPEED);
				}
			}

			base.Update(gameTime, gameObjects);
		}

		protected override void CheckBounds()
		{
			Location.Y = MathHelper.Clamp(Location.Y, 0, gameBoundries.Height - this.Height);
		}

		public void Reset()
		{
			this.Location = new Vector2(this.Location.X, (gameBoundries.Height - Height)/2);
			this.Velocity = Vector2.Zero;
		}
	}

}
