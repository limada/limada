using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gecko.Interop;

namespace Gecko
{
	public static class ScreenManager
	{
		private static ComPtr<nsIScreenManager> _screenManager;

		static ScreenManager()
		{
			_screenManager = Xpcom.GetService2<nsIScreenManager>(Contracts.ScreenManager);
		}

		public static int NumberOfScreens
		{
			get { return (int)_screenManager.Instance.GetNumberOfScreensAttribute(); }
		}

		public static Screen PrimaryScreen
		{
			get { return new Screen( _screenManager.Instance.GetPrimaryScreenAttribute() ); }
		}

		public static Screen ScreenForRect(int left,int top,int width,int height)
		{
			return new Screen( _screenManager.Instance.ScreenForRect( left, top, width, height ) );
		}
	}


	public sealed class Screen
	{
		private ComPtr<nsIScreen> _screen;

		public Screen(nsIScreen screen)
		{
			_screen = new ComPtr<nsIScreen>( screen );
		}

		public int ColorDepth
		{
			get { return _screen.Instance.GetColorDepthAttribute(); }
		}

		public int PixelDepth
		{
			get { return _screen.Instance.GetPixelDepthAttribute(); }
		}
	}
}
