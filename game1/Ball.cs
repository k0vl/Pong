#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

#endregion

namespace game1
{

	public class Ball : Sprite
	{
		public Paddle attachedToPaddle {get; set;}
		public int speedFlag {get; set;}

		public Ball(Texture2D texture, Vector2 location, Rectangle gameBoundries) : base(texture, location, gameBoundries)
		{
			speedFlag = 0;
		}

		public Ball(Texture2D texture, Vector2 location, Rectangle gameBoundries, int SpeedFlag) : base(texture, location, gameBoundries)
		{
			this.speedFlag = SpeedFlag;
		}

		protected override void CheckBounds()
		{
			if(Location.Y >= (gameBoundries.Height - this.Height))
			{
				Velocity = new Vector2(Velocity.X, -Math.Abs(Velocity.Y));
			}else if(Location.Y <= 0)
			{
				Velocity = new Vector2(Velocity.X, Math.Abs(Velocity.Y));
			}
		}

		public override void Update(GameTime gameTime, GameObjects gameObjects)
		{
			//first, Check if firing ball
			if( (Keyboard.GetState().IsKeyDown(Keys.Space) || gameObjects.TouchInput.Tapped) && attachedToPaddle != null)
			{
				var newVelocity = new Vector2(500f, attachedToPaddle.Velocity.Y * 0.75f);
				Velocity = newVelocity;
				attachedToPaddle = null;
			}

			//second, update postion if still attached to paddle
			else if(attachedToPaddle != null)
			{
				Location.X = attachedToPaddle.Location.X + attachedToPaddle.Width;
				Location.Y = attachedToPaddle.Location.Y + (attachedToPaddle.Height-this.Height)/2;
				Velocity = Vector2.Zero;
			}

			//third, check bounce and boost if fired
			else
			{
				CheckBounce(gameObjects);
			}

			base.Update(gameTime, gameObjects);
		}

		public void CheckBounce(GameObjects gameObjects)
		{
			if(BoundingBox.Intersects(gameObjects.PlayerPaddle.BoundingBox))
			{
				float paddleCenter = gameObjects.PlayerPaddle.Location.Y+(gameObjects.PlayerPaddle.Height/2);
				float ballCenter = Location.Y+(this.Height/2);
				float VelocityY =  Velocity.Y + Paddle.PADDLE_SPEED * (ballCenter - paddleCenter)/gameObjects.PlayerPaddle.Height;

				Velocity = new Vector2(Math.Abs(Velocity.X), VelocityY);
			}
			else if(BoundingBox.Intersects(gameObjects.ComputerPaddle.BoundingBox))
			{
				float paddleCenter = gameObjects.ComputerPaddle.Location.Y+(gameObjects.ComputerPaddle.Height/2);
				float ballCenter = Location.Y+(this.Height/2);
				float VelocityY =  Velocity.Y + Paddle.PADDLE_SPEED * (ballCenter - paddleCenter)/gameObjects.ComputerPaddle.Height;

				Velocity = new Vector2(-Math.Abs(Velocity.X), VelocityY);
			}
		}

		public void AttachTo(Paddle paddle)
		{
			attachedToPaddle = paddle;
		}
	}
	
}
