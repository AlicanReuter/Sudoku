#region Imports
using System.Drawing;
using System.Windows.Forms;
using UI.Controls.Buttons;
using static Shared.Configs.UI.Controls;
#endregion

namespace UI.Controls.Panels;
internal class TaskMenuPanel : Panel {
	internal PanelType panelType;
	internal TaskMenuPanel(PanelType type) {
		this.panelType = type;
		InitializeControl();
	}
	private void InitializeControl() {
		this.Visible = true;
		this.BackColor = Color.Transparent;
		this.Size = new Size(TaskPanelWidth, TaskPanelHeight);
		this.Location = new Point(0, TaskPanelHeight);
		AddChilds();
	}
	private void AddChilds() {
		this.Controls.Add(new GoBackButton(ButtonType.GoBack, 1));
	}
}