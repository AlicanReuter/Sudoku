#region Imports
using System;
using System.Drawing;
using System.Windows.Forms;
using static Shared.Configs.UI.Controls;
using static UI.Controls.Helpers.Helper;
#endregion
namespace UI.Controls.Buttons;
internal class UndoButton : Button {
	internal ButtonType buttonType;
	internal UndoButton(ButtonType type) {
		this.buttonType = type;
		InitializeControl();
	}
	private void InitializeControl() {
		this.Visible = true;
		this.BackColor = Color.DarkGray;
		this.Enabled = true;
		this.ForeColor = Color.White;
		this.FlatAppearance.BorderSize = 0;
		this.Font = new Font(this.Font.FontFamily, 14, FontStyle.Bold);
		this.FlatStyle = FlatStyle.Flat;
		this.Size = new(GoBackButtonWidth, GoBackButtonHeight);
		this.Location = new Point(TaskPanelWidth - GoBackButtonWidth - GoBackButtonMargin, 0);
		this.Text = "Undo";
		this.Region = GetRegion(this);
		this.MouseClick += new MouseEventHandler(ClickControl);
		this.MouseEnter += EnterControl;
		this.MouseLeave += LeaveControl;
	}
	internal void ClickControl(object sender, MouseEventArgs e) {
		LoadHistory();
		Control sudokuField = this.Parent.Parent.Controls[2].Controls[4].Controls[0];
		foreach (Control child in sudokuField.Controls) {
			GameButtonField field = child as GameButtonField;
			field.LoadText();

		}
	}
	private void EnterControl(object sender, EventArgs e) {
		this.BackColor = Color.White;
		this.ForeColor = Color.Black;
	}
	private void LeaveControl(object sender, EventArgs e) {
		this.BackColor = Color.DarkGray;
		this.ForeColor = Color.White;
	}
}