
using System;
using NUnit.Framework;


namespace Limaki.Tests.MonoGenericError2
{

	[TestFixture]
	public class GenericErrorTest
	{

		[Test]
		public void Test ()
		{
			var display = new MyDisplay<IMyClass<int, int>> ();
		}
		
	}

	public interface IMyClass<T1, T2>
	{
	}

	public class MyDisplay<T> : WinformDisplay<T>
	{
		public override DisplayFactory<T> CreateDisplayFactory (WinformDisplay<T> device)
		{
			var result = new DisplayFactory<T> ();
			
			var deviceInstrumenter = new WinformDeviceInstrumenter<T> ();
			deviceInstrumenter.Device = device;
			result.DeviceInstrumenter = deviceInstrumenter;
			
			result.DisplayInstrumenter = new DisplayInstrumenter<T> ();
			
			return result;
		}
		
	}

	public interface IDisplayDevice
	{
		IDisplay Display { get; set; }
	}
	
	public interface IDisplayDevice<T>:IDisplayDevice 
	{
		new IDisplay<T> Display { get; set; }
	}

	public interface IInstrumenter<T>
	{
		void Instrument (T display);
		void Factor (T display);
	}

	public class DisplayFactory<TData>
	{

		public virtual Display<TData> Create ()
		{
			var result = new Display<TData> ();
			return result;
		}

		protected IInstrumenter<Display<TData>> _displayInstrumenter = null;
		public virtual IInstrumenter<Display<TData>> DisplayInstrumenter {
			get {
				if (_displayInstrumenter == null) {
					_displayInstrumenter = new DisplayInstrumenter<TData> ();
				}
				return _displayInstrumenter;
			}
			set { _displayInstrumenter = value; }
		}

		public virtual IInstrumenter<Display<TData>> DeviceInstrumenter { get; set; }



		public virtual void Instrument (Display<TData> display)
		{
			DisplayInstrumenter.Factor (display);
			DeviceInstrumenter.Factor (display);
			
			DeviceInstrumenter.Instrument (display);
			DisplayInstrumenter.Instrument (display);
			
		}
	}

	public abstract class WinformDisplay<T> : MarshalByRefObject, IDisplayDevice<T>
	{
		public WinformDisplay ()
		{
			Initialize ();
		}

		public abstract DisplayFactory<T> CreateDisplayFactory (WinformDisplay<T> device);

		protected void Initialize ()
		{
			var factory = CreateDisplayFactory (this);
			var display = factory.Create ();
			factory.Instrument (display);
		}

		public IDisplay<T> Display { get; set; }

		IDisplay IDisplayDevice.Display {
			get { return this.Display; }
			set { this.Display = value as IDisplay<T>; }
		}
		
	}

	public class Display<TData> : IDisplay<TData>
	{
		public TData Data { get; set; }

		public void Invoke ()
		{
		}
		public void Execute ()
		{
		}

		public object ActiveControl { get; set; }
	}


	public class DisplayInstrumenter<TData> : IInstrumenter<Display<TData>>
	{

		public virtual void Factor (Display<TData> display)
		{
		}
		public virtual void Instrument (Display<TData> display)
		{
		}
	}

	public interface IDisplay
	{
		void Invoke ();
		void Execute ();

		object ActiveControl { get; set; }
	}

	public interface IDisplay<T> : IDisplay
	{
		T Data { get; set; }
		
	}

	public abstract class DeviceInstrumenter<TData, TDevice> : IInstrumenter<Display<TData>>
	{

		public virtual TDevice Device { get; set; }


		public abstract void Factor (Display<TData> display);
		public abstract void Instrument (Display<TData> display);
		
	}

	public class WinformDeviceInstrumenter<TData> : DeviceInstrumenter<TData, WinformDisplay<TData>>
	{


		public override void Factor (Display<TData> display)
		{
			
			Device.Display = display;
		}

		public override void Instrument (Display<TData> display)
		{
		}
	}
}
