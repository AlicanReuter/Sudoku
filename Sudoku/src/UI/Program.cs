#region Imports
using System;
using System.Windows.Forms;
using UI.Controls.Forms;
using static Shared.Configs.UI.Controls;
#endregion
namespace UI;
public class Program {
	/// <summary>
	/// Main entry of Project UI
	/// </summary>
	[STAThread]
	public static void Main() {
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(false);
		Application.Run(new Window(FormType.MainForm));
	}
}