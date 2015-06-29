#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

#endregion

namespace game1
{

	public class GameObjects
	{
		public Paddle PlayerPaddle { get; set; }
		public Paddle ComputerPaddle { get; set; }
		public Ball Ball { get; set; }
		public Score Score { get; set; }
		public TouchInput TouchInput { get; set; }
	}
		
	public class TouchInput
	{
		public bool Up { get; set; }
		public bool Down {get; set; }
		public bool Tapped { get; set; }
	}
}
