#region Imports
using System.Drawing;
using System.Windows.Forms;
using UI.Controls.Panels;
using static Shared.Configs.UI.Controls;
using static UI.Controls.Helpers.Helper;
using static UI.Controls.Helpers.NavigationControler;
#endregion
namespace UI.Controls.Forms;
internal class Window : Form {
	internal FormType formType;
	internal Window(FormType type) {
		this.formType = type;
		InitializeControl();
	}
	private void InitializeControl() {
		this.Size = new(MainFormWidth, MainFormHeight);
		this.FormBorderStyle = FormBorderStyle.None;
		this.StartPosition = FormStartPosition.CenterScreen;
		this.Region = GetRegion(this);
		this.BackColor = Color.Gray;
		AddChilds();
		SetRootControl(this);
	}
	private void AddChilds() {
		this.Controls.Add(new TaskPanel(PanelType.TaskBar));
		this.Controls.Add(new TaskMenuPanel(PanelType.TaskMenuBar));
		this.Controls.Add(new FormPanel(PanelType.FormPanel));
	}
}