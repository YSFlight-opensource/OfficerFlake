﻿using System;
using System.Threading;
using System.Windows;
using System.Windows.Media.TextFormatting;
using System.Windows.Threading;
using Com.OfficerFlake.Libraries.Logger;

namespace Com.OfficerFlake.Libraries.UserInterfacesWPF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class ConsoleWindow : Window
	{
		public void LinkConsole()
		{
			Logger.Console.LinkConsole(ConsoleOutput);
		}

		public ConsoleWindow()
		{
			InitializeComponent();
		}

		#region Load
		private void OnLoaded(object sender, EventArgs e)
		{
			LoadedSignal.Set();
		}
		public readonly ManualResetEvent LoadedSignal = new ManualResetEvent(false);
		public Boolean IsLoadd => LoadedSignal.WaitOne(0);
		public Boolean WaitForLoad(int timeout = Int32.MaxValue) => LoadedSignal.WaitOne(timeout);
		#endregion
		#region Close
		private void OnClosed(object sender, EventArgs e)
		{
			ClosedSignal.Set();
		}
		public readonly ManualResetEvent ClosedSignal = new ManualResetEvent(false);
		public Boolean IsClosed => ClosedSignal.WaitOne(0);
		public Boolean WaitForClose(int timeout = Int32.MaxValue) => ClosedSignal.WaitOne(timeout);
		#endregion
	}

	public static class ConsoleUI
	{
		public static ConsoleWindow consoleWindow;

		public static void CreateWindow()
		{
			ManualResetEvent ready = new ManualResetEvent(false);
			Thread newThread = new Thread(() =>
			{
				Application newApp = new Application();
				consoleWindow = new ConsoleWindow();
				ready.Set();
				newApp.Run(consoleWindow);
			});
			newThread.SetApartmentState(ApartmentState.STA);
			newThread.Start();
			ready.WaitOne();
			ready.Dispose();
		}

		public static void LinkConsole() => consoleWindow.Dispatcher.Invoke(() => consoleWindow.LinkConsole());

		public static void Show() => consoleWindow.Dispatcher.Invoke(() => consoleWindow.Show());
		public static void Hide() => consoleWindow.Dispatcher.Invoke(() => consoleWindow.Hide());

		public static bool WaitForLoad(int timeout = Int32.MaxValue) => consoleWindow.Dispatcher.Invoke(() => (consoleWindow.IsVisible) || consoleWindow.WaitForLoad(timeout));
		public static bool WaitForClose(int timeout = Int32.MaxValue) => (!consoleWindow.IsVisible) || consoleWindow.WaitForClose(timeout);
	}
}
