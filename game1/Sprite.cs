#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

#endregion

namespace game1
{

	public abstract class Sprite
	{
		protected readonly Texture2D texture;
		public Vector2 Location;
		protected readonly Rectangle gameBoundries;
		public int Width {get {return (int)(texture.Width * Game1.DeviceScale);}}
		public int Height {get {return (int)(texture.Height * Game1.DeviceScale);}}
		public Rectangle BoundingBox {get {return new Rectangle((int)Location.X, (int)Location.Y, Width, Height);}}
		public Vector2 Velocity {get; set;}

		public Sprite(Texture2D texture, Vector2 location, Rectangle gameBoundries)
		{
			this.texture = texture;
			this.Location = location;
			this.gameBoundries = gameBoundries;
			this.Velocity = Vector2.Zero;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			//spriteBatch.Draw (texture, Location, Color.White);
			spriteBatch.Draw (texture, Location, null, Color.White, 0f, new Vector2(0,0), Game1.DeviceScale, SpriteEffects.None, 0);
		}

		public virtual void Update(GameTime gameTime, GameObjects gameObjects)
		{
			Location += (Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds * Game1.DeviceScale);
			CheckBounds();
		}

		protected abstract void CheckBounds();
	}
}
