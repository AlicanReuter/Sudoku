#region Imports
using System.Windows.Forms;
using UI.Controls.Forms;
#endregion
namespace UI;
public class Program {
	/// <summary>
	/// Main entry of Project UI
	/// </summary>
	public static void Main() {
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(false);
		Application.Run(new Window());
	}
}