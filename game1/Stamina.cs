#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

#endregion

namespace game1 {
	public class Stamina {
		private readonly SpriteFont font;
		Vector2 position;
		private float rotation, Height, Width;
		private string scoreText = "STAMINA";
		private int lastTime = 0;
		private GameTime gameTime;

		public Stamina(SpriteFont font, Rectangle gameBoundries) {
			this.font = font;
			this.Height = font.MeasureString("STAMINA").Y*Game1.DeviceScale;
			this.Width =font.MeasureString("STAMINA").X*Game1.DeviceScale;
			position = new Vector2(0, 50);
			rotation = 0;
		}

		public void Draw(SpriteBatch spriteBatch) {
			//spriteBatch.DrawString(font, scoreText, position, Color.Black);
			spriteBatch.DrawString (font,scoreText,position,Color.Black,rotation,new Vector2(0,0), Game1.DeviceScale, SpriteEffects.None,0);
		}

		public void Update(GameTime gameTime, GameObjects gameObjects) {
			this.gameTime = gameTime;
			if (gameObjects.Controller.HasGameStarted == true)
				UpdateStamina();
			else {
				UpdateStamina("");
				this.Reset();
			}
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
			this.position = new Vector2((gameObjects.GameBoundries.Width /2) - (Width/2), 50);
			#endif
		}

		public void Reset() {
			this.lastTime = (int)this.gameTime.TotalGameTime.TotalSeconds;
		}

		public void UpdateStamina(string customText = null)
		{
			if (customText == null) {
				this.scoreText = string.Format("{0}", (int)this.gameTime.TotalGameTime.TotalSeconds - this.lastTime);
			}
			else {
				this.scoreText = string.Format("{0}", customText);
			}
			this.Width = font.MeasureString(scoreText).X*Game1.DeviceScale;
		}
	}
}
