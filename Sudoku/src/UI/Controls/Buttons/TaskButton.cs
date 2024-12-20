#region Imports
using System;
using System.Drawing;
using System.Windows.Forms;
using static Shared.Configs.UI.Controls;
#endregion
namespace UI.Controls.Buttons;
internal class TaskButton : Button {
	internal ButtonType buttonType;
	internal int buttonIndex;
	internal TaskButton(ButtonType type, int index) {
		this.buttonType = type;
		this.buttonIndex = index;
		InitializeControl();
	}
	private void InitializeControl() {
		this.Visible = true;
		this.BackColor = Color.Transparent;
		this.ForeColor = Color.White;
		this.FlatAppearance.BorderSize = 0;
		this.FlatStyle = FlatStyle.Flat;
		this.Size = new(TaskButtonSize, TaskButtonSize);
		this.Location = GetLocation();
		this.Text = GetText();
		this.MouseClick += new MouseEventHandler(ClickControl);
		this.MouseEnter += EnterControl;
		this.MouseLeave += LeaveControl;
	}
	private Point GetLocation() {
		int xCoordinate = TaskPanelWidth - (this.Width * buttonIndex);
		int yCoordinate = 0;
		return new Point(xCoordinate, yCoordinate);
	}
	private string GetText() {
		return buttonType switch {
			ButtonType.TaskBarClose => "\U0001F5D9",
			ButtonType.TaskBarMaximize => "\U0001F5D6",
			ButtonType.TaskBarMinimize => "\U0001F5D5",
			_ => ""
		};
	}
	private void ClickControl(object sender, MouseEventArgs e) {
		switch (buttonType) {
			case ButtonType.TaskBarClose:
				Application.Exit();
				break;
			case ButtonType.TaskBarMaximize:
				//this.FindForm().WindowState = FormWindowState.Maximized;
				break;
			case ButtonType.TaskBarMinimize:
				this.FindForm().WindowState = FormWindowState.Minimized;
				break;
		}
	}
	private void EnterControl(object sender, EventArgs e) {
		switch (buttonType) {
			case ButtonType.TaskBarClose:
				this.BackColor = Color.Red;
				this.ForeColor = Color.Black;
				break;
			case ButtonType.TaskBarMaximize:
				this.BackColor = Color.Orange;
				this.ForeColor = Color.Black;
				break;
			case ButtonType.TaskBarMinimize:
				this.BackColor = Color.Green;
				this.ForeColor = Color.Black;
				break;
		}
	}
	private void LeaveControl(object sender, EventArgs e) {
		this.BackColor = Color.Transparent;
		this.ForeColor = Color.White;
	}
}
