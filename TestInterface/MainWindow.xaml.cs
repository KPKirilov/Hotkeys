using Hotkeys;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestInterface
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private HotkeyHost host;
		private Hotkey hk;
		private Hotkey hk2;

		public MainWindow()
		{
			InitializeComponent();
			host = new HotkeyHost();
			hk = new Hotkey();
			hk.Modifiers = Modifiers.Alt;
			hk.Key = Key.OEM3;
			host.RegisterHotkey(hk);
			hk2 = new Hotkey();
			hk2.Key = Key.G;
			host.RegisterHotkey(hk2);

			hk.Fired += Hk_Fired;
			hk2.Fired += Hk2_Fired; ;
		}

		private void Hk2_Fired(object sender, HotkeyFiredEventArgs e)
		{
			this.Focus();
			this.Activate();
			this.WindowState = WindowState.Normal;


			Dispatcher.Invoke(() => Background = Brushes.PaleVioletRed);

		}

		private void Hk_Fired(object sender, HotkeyFiredEventArgs e)
		{
			this.Focus();
			this.Activate();
			this.WindowState = WindowState.Normal;
			Dispatcher.Invoke(() => Background = Brushes.DarkOliveGreen);
		}

		protected override void OnClosed(EventArgs e)
		{
			host.Dispose();

			base.OnClosed(e);
		}
	}
}
