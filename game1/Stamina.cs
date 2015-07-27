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
		private int lastTime = 0, maxTime, staminaScore = 0, staminaLevel;
		public float paddleSpeed {get {return (float)staminaLevel/(float)staminaLevel_i;}}
		public const int maxTime_i = 10, staminaLevel_i = 3;
		private GameTime gameTime;

		public Stamina(SpriteFont font, Rectangle gameBoundries) {
			this.maxTime = maxTime_i;
			this.staminaLevel = staminaLevel_i;
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
			this.staminaScore = this.maxTime - ((int)this.gameTime.TotalGameTime.TotalSeconds - this.lastTime);
			if (gameObjects.Controller.HasGameStarted == true) {	// update Stamina on screen
				if (this.staminaScore <= 0 && this.staminaLevel >= 1) {
					this.staminaLevel -= 1;
					this.maxTime += maxTime_i;
				}
				UpdateStamina();
			}
			else {	// game hasn't started yet
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

		public void AddMaxTime(int addTime) {
			this.maxTime += addTime;
		}

		public void Reset() {
			this.lastTime = (int)this.gameTime.TotalGameTime.TotalSeconds;
			this.maxTime = maxTime_i;
			this.staminaLevel = staminaLevel_i;
		}

		public void UpdateStamina(string customText = null)
		{
			if (customText == null) {
				this.scoreText = string.Format("{0}\n{1}", this.staminaScore, "Level "+this.staminaLevel);
			}
			else {
				this.scoreText = string.Format("{0}", customText);
			}
			this.Width = font.MeasureString(scoreText).X*Game1.DeviceScale;
		}
	}
}
