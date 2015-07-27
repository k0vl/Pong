#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

#endregion

namespace game1
{
//	public class Menu
//	{
//		private readonly SpriteFont font;
//		public Menu()
//		{
//			this.font = font;
//		}
//		public void Update(GameTime gameTime, GameObjects gameObjects)
//		{
//		}
//
//		public void Draw(SpriteBatch spriteBatch)
//		{
//			spriteBatch.DrawString (font,scoreText,position,Color.Black,rotation,new Vector2(0,0), Game1.DeviceScale, SpriteEffects.None,0);
//			spriteBatch.DrawString (font,scoreText,position,Color.Black,rotation,new Vector2(0,0), Game1.DeviceScale, SpriteEffects.None,0);
//		}
//	}
	public class Controller
	{
		public int PlayerScore { get; set; }
		public int ComputerScore { get; set; }
		public bool HasGameStarted = false;

		public Controller()
		{
			
		}

		public void Update(GameTime gameTime, GameObjects gameObjects)
		{
			CheckWin(gameObjects);
			CheckBoost(gameObjects);
		}

		private void CheckWin(GameObjects gameObjects)
		{
			List<Ball> toRemove = new List<Ball>();

			foreach(Ball ball in gameObjects.Ball)
			{
				if(ball.Location.X + ball.Width < 0)
				{ComputerScore++; toRemove.Add(ball); gameObjects.Score.UpdateScore(PlayerScore, ComputerScore);}
				else if(ball.Location.X > gameObjects.GameBoundries.Width)
				{PlayerScore++; toRemove.Add(ball); gameObjects.Score.UpdateScore(PlayerScore, ComputerScore);}
			}

			gameObjects.Ball.RemoveAll(x => toRemove.Contains(x));

			if(gameObjects.Ball.Count == 0)
			{
				Reset(gameObjects);
			}
		}

		public void CheckBoost(GameObjects gameObjects)
		{
			if(gameObjects.Boost.Count != 0 && gameObjects.Ball.Count != 0){
				
				List<Ball> toAdd = new List<Ball>();
				List<Boost> toRemove = new List<Boost>();
				foreach(Ball ball in gameObjects.Ball)
				{
					foreach(Boost boost in gameObjects.Boost)
					{
						if(ball.BoundingBox.Intersects(boost.BoundingBox))
						{
							if(boost.boostTypes == BoostTypes.Speed && ball.speedFlag < 1)
							{
								ball.Velocity = new Vector2(ball.Velocity.X * 1.5f, ball.Velocity.Y * 1.5f);

							}
							else if(boost.boostTypes == BoostTypes.Redirect)
							{
								ball.Velocity = new Vector2(ball.Velocity.X, 0);
							}
							else if(boost.boostTypes == BoostTypes.Slow && ball.speedFlag > -1)
							{
								ball.Velocity = new Vector2(ball.Velocity.X / 1.5f, ball.Velocity.Y / 1.5f);
								ball.speedFlag -= 1;
							}
							else if(boost.boostTypes == BoostTypes.Deflect)
							{
								ball.Velocity = new Vector2(-ball.Velocity.X, ball.Velocity.Y);
							}
							else if(boost.boostTypes == BoostTypes.Split)
							{
								Ball newball1 = new Ball(gameObjects.BallTexture, ball.Location, gameObjects.GameBoundries,ball.speedFlag);
								newball1.Velocity=new Vector2(ball.Velocity.X, 0);
								Ball newball2 = new Ball(gameObjects.BallTexture, ball.Location, gameObjects.GameBoundries,ball.speedFlag);
								newball2.Velocity=new Vector2(ball.Velocity.X, -ball.Velocity.Y);
								toAdd.Add(newball1);
								toAdd.Add(newball2);
							}
							else
							{
							}

							toRemove.Add(boost);
						}
					}
				}

				gameObjects.Ball.AddRange(toAdd);
				gameObjects.Boost.RemoveAll(x => toRemove.Contains(x));
			}
		}

		public void Reset(GameObjects gameObjects)
		{
			gameObjects.Ball.Clear();
			gameObjects.Score.UpdateScore(PlayerScore, ComputerScore);
			gameObjects.Stamina.Reset();
			gameObjects.PlayerPaddle.Reset();
			gameObjects.ComputerPaddle.Reset();
			gameObjects.Ball.Add(new Ball(gameObjects.BallTexture, Vector2.Zero, gameObjects.GameBoundries));
			gameObjects.Ball[0].AttachTo(gameObjects.PlayerPaddle);
			gameObjects.Boost.Clear();
			gameObjects.Controller.HasGameStarted = false;
		}
	}
}
