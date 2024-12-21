#region Imports
using System.Drawing;
using System.Windows.Forms;
using UI.Controls.Buttons;
using static Shared.Configs.UI.Controls;
#endregion
namespace UI.Controls.Panels;
internal class TaskPanel : Panel {
	internal PanelType panelType;
	internal TaskPanel(PanelType type) {
		this.panelType = type;
		InitializeControl();
	}
	private void InitializeControl() {
		this.Visible = true;
		this.BackColor = Color.Transparent;
		this.Size = new Size(TaskPanelWidth, TaskPanelHeight);
		this.Location = new Point(0, 0);
		AddChilds();
	}
	private void AddChilds() {
		this.Controls.Add(new TaskButton(ButtonType.TaskBarClose, 1));
		this.Controls.Add(new TaskButton(ButtonType.TaskBarMaximize, 2));
		this.Controls.Add(new TaskButton(ButtonType.TaskBarMinimize, 3));
	}
}