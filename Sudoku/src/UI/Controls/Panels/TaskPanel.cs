#region Imports
using System.Drawing;
using System.Windows.Forms;
using UI.Controls.Buttons;
using static Shared.Configs.UI.Controls;
#endregion
namespace UI.Controls.Panels;
internal class TaskPanel : Panel {
	internal PanelType panelType;
	private bool isDraging;
	private Point start = new(0, 0);
	internal TaskPanel(PanelType type) {
		this.panelType = type;
		InitializeControl();
	}
	private void InitializeControl() {
		this.Visible = true;
		this.BackColor = Color.Transparent;
		this.Size = new Size(TaskPanelWidth, TaskPanelHeight);
		this.Location = new Point(0, 0);
		this.MouseDown += new MouseEventHandler(OnMouseDown);
		this.MouseUp += new MouseEventHandler(OnMouseUp);
		this.MouseMove += new MouseEventHandler(OnMouseMove);
		AddChilds();
	}
	private void AddChilds() {
		this.Controls.Add(new TaskButton(ButtonType.TaskBarClose, 1));
		this.Controls.Add(new TaskButton(ButtonType.TaskBarMaximize, 2));
		this.Controls.Add(new TaskButton(ButtonType.TaskBarMinimize, 3));
	}
	private void OnMouseDown(object sender, MouseEventArgs e) {
		isDraging = true;
		start = e.Location;
	}
	private void OnMouseUp(object sender, MouseEventArgs e) {
		isDraging = false;
		start = new(0, 0);
	}
	private void OnMouseMove(object sender, MouseEventArgs e) {
		if (!isDraging) { return; }
		this.Parent.Left += e.X - start.X;
		this.Parent.Top += e.Y - start.Y;
	}
}