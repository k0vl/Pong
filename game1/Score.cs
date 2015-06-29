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
		private readonly Rectangle gameBoundries;
		private float yposition, xposition, rotation, Height, Width;

		public int PlayerScore { get; set; }
		public int ComputerScore { get; set; }

		public Score(SpriteFont font, Rectangle gameBoundries)
		{
			this.font = font;
			this.gameBoundries = gameBoundries;
			this.Height = font.MeasureString("0:0").Y*Game1.DeviceScale;
			this.Width =font.MeasureString("0:0").X*Game1.DeviceScale;
			xposition = (gameBoundries.Width /2) - (Width/2);
			yposition = gameBoundries.Height - 100;
			rotation = 0;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			string scoreText = string.Format("{0}:{1}", PlayerScore, ComputerScore);

			#if __ANDROID__
			if(Activity1.Accl[0] > 3)
			{yposition = gameBoundries.Height - 100 - Height/2; rotation = 0; xposition = (gameBoundries.Width /2)- (Width/2);} //- (Width/2)
			else if(Activity1.Accl[0] < -3)
			{yposition = 100 + Height/2; rotation = (float)Math.PI; xposition = (gameBoundries.Width /2)+ (Width/2);}
			else
			{}
			#else
			yposition = gameBoundries.Height - 100;
			#endif

			Vector2 position = new Vector2(xposition, yposition);

			//spriteBatch.DrawString(font, scoreText, position, Color.Black);
			spriteBatch.DrawString (font,scoreText,position,Color.Black,rotation,new Vector2(0,0), Game1.DeviceScale, SpriteEffects.None,0);
		}

		public void Update(GameTime gameTime, GameObjects gameObjects)
		{
			if(gameObjects.Ball.Location.X + gameObjects.Ball.Width < 0)
			{
				ComputerScore++;
				gameObjects.PlayerPaddle.Reset();
				gameObjects.ComputerPaddle.Reset();
				gameObjects.Ball.AttachTo(gameObjects.PlayerPaddle);
				this.Width = font.MeasureString(string.Format("{0}:{1}", PlayerScore, ComputerScore)).X*Game1.DeviceScale;
			}
			else if(gameObjects.Ball.Location.X > gameBoundries.Width)
			{
				PlayerScore++;
				gameObjects.PlayerPaddle.Reset();
				gameObjects.ComputerPaddle.Reset();
				gameObjects.Ball.AttachTo(gameObjects.PlayerPaddle);
				this.Width = font.MeasureString(string.Format("{0}:{1}", PlayerScore, ComputerScore)).X*Game1.DeviceScale;
			}

		}
	}
}
