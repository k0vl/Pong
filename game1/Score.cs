#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

#endregion

namespace game1
{

	public class Score
	{
		private readonly SpriteFont font;
		Vector2 position;
		private float rotation, Height, Width;
		private string scoreText = "0:0";

		public Score(SpriteFont font, Rectangle gameBoundries)
		{
			this.font = font;
			this.Height = font.MeasureString("0:0").Y*Game1.DeviceScale;
			this.Width =font.MeasureString("0:0").X*Game1.DeviceScale;
			position = new Vector2((gameBoundries.Width /2) - (Width/2), gameBoundries.Height - 100);
			rotation = 0;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			//spriteBatch.DrawString(font, scoreText, position, Color.Black);
			spriteBatch.DrawString (font,scoreText,position,Color.Black,rotation,new Vector2(0,0), Game1.DeviceScale, SpriteEffects.None,0);
		}

		public void Update(GameTime gameTime, GameObjects gameObjects)
		{
			#if __ANDROID__
			float xposition = this.position.X, yposition = this.position.Y;
			if(Activity1.Accl[0] > 3)
			{
				yposition = gameObjects.GameBoundries.Height - 100 - Height/2; 
				rotation = 0; 
				xposition = (gameObjects.GameBoundries.Width /2)- (Width/2);
			} //- (Width/2)
			else if(Activity1.Accl[0] < -3)
			{
				yposition = 100 + Height/2; 
				rotation = (float)Math.PI; //in radian, rotate 180 degree
				xposition = (gameObjects.GameBoundries.Width /2)+ (Width/2);
			}
			else
			{}
			this.position = new Vector2(xposition, yposition);
			#else
			this.position = new Vector2((gameObjects.GameBoundries.Width /2) - (Width/2), gameObjects.GameBoundries.Height - 100);
			#endif
		}

		public void UpdateScore(int playerScore, int computerScore)
		{
			this.scoreText = string.Format("{0}:{1}", playerScore, computerScore);
			this.Width = font.MeasureString(scoreText).X*Game1.DeviceScale;
		}
	}
}
