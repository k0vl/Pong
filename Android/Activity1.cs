using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Content.PM;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Hardware;

#if OUYA
using Ouya.Console.Api;
#endif

using Microsoft.Xna.Framework;

namespace game1
{
	[Activity (Label = "Android", 
		MainLauncher = true,
		Icon = "@drawable/icon",
		Theme = "@android:style/Theme.NoTitleBar.Fullscreen",
		AlwaysRetainTaskState = true,
		LaunchMode = LaunchMode.SingleInstance,
		ConfigurationChanges = ConfigChanges.Orientation |
		ConfigChanges.KeyboardHidden |
		ConfigChanges.Keyboard |
		ConfigChanges.ScreenSize)]
	#if OUYA
	[IntentFilter(new[] { Intent.ActionMain }
		, Categories = new[] { Intent.CategoryLauncher, OuyaIntent.CategoryGame })]
	#endif
	public class Activity1 : AndroidGameActivity, ISensorEventListener
	{
		private SensorManager _sensorManager;
		private static readonly object _syncLock = new object();
		public static float[] Accl = new float[5];

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			//Window.AddFlags(WindowManagerFlags.Fullscreen);
			//Window.ClearFlags(WindowManagerFlags.ForceNotFullscreen);
			_sensorManager = (SensorManager) GetSystemService(Context.SensorService);
			var g = new Game1 ();
			SetContentView (g.Services.GetService<View> ());
			g.Run ();
		}

		protected override void OnResume()
		{
			base.OnResume();
			_sensorManager.RegisterListener(this, _sensorManager.GetDefaultSensor(SensorType.Accelerometer), SensorDelay.Ui);
		}

		protected override void OnPause()
		{
			base.OnPause();
			_sensorManager.UnregisterListener(this);
		}

		public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy)
		{
			// We don't want to do anything here.
		}

		public void OnSensorChanged(SensorEvent e)
		{
			//lock (_syncLock)
			//{
				e.Values.CopyTo(Accl, 0);
			//}
		}
	}
}


