#region Imports
using System.Drawing;
using System.Windows.Forms;
using UI.Controls.Buttons;
using static Shared.Configs.UI.Controls;
using static UI.Controls.Helpers.NavigationControler;
#endregion
namespace UI.Controls.Panels;
internal class TaskPanel : Panel {
	internal PanelType panelType;
	private bool isDraging;
	private Point mousePos;
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
		mousePos = new Point(0, 0);
	}
	private void AddChilds() {
		this.Controls.Add(new TaskButton(ButtonType.TaskBarClose, 1));
		this.Controls.Add(new TaskButton(ButtonType.TaskBarMaximize, 2));
		this.Controls.Add(new TaskButton(ButtonType.TaskBarMinimize, 3));
	}
	private void ControlMouseDown(object sender, MouseEventArgs e) {
		isDraging = true;
		mousePos.X = e.X;
		mousePos.Y = e.Y;
	}
	private void ControlMouseUp(object sender, MouseEventArgs e) { isDraging = false; }
	private void ControlMouseMove(object sender, MouseEventArgs e) {
		if (!isDraging) { return; }
		RootCntrl.Location.X += e.X - mousePos.X;
	}
}