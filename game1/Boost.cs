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
	public enum BoostTypes {Speed, Split, Redirect, Slow, Deflect}

	public class Boost : Sprite
	{
		public BoostTypes boostTypes {get; protected set;}

		public Boost(Texture2D texture, Vector2 location, Rectangle gameBoundries, BoostTypes boostTypes) : base(texture, location, gameBoundries)
		{
			this.boostTypes = boostTypes;
		}
		protected override void CheckBounds()
		{
			
		}
		public override void Update(GameTime gameTime, GameObjects gameObjects)
		{
			
		}
	}
	
}
