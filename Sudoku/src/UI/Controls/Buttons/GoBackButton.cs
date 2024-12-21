#region Imports
using System;
using System.Drawing;
using System.Windows.Forms;
using static Shared.Configs.UI.Controls;
using static UI.Controls.Helpers.Helper;
using static UI.Controls.Helpers.NavigationControler;
#endregion
namespace UI.Controls.Buttons;
internal class GoBackButton : Button {
	internal ButtonType buttonType;
	internal int buttonIndex;
	internal GoBackButton(ButtonType type, int index) {
		this.buttonType = type;
		this.buttonIndex = index;
		InitializeControl();
	}
	private void InitializeControl() {
		this.Visible = true;
		this.BackColor = Color.Transparent;
		this.Enabled = false;
		this.ForeColor = Color.White;
		this.FlatAppearance.BorderSize = 0;
		this.Font = new Font(this.Font.FontFamily, 14, FontStyle.Bold);
		this.FlatStyle = FlatStyle.Flat;
		this.Size = new(GoBackButtonWidth, GoBackButtonHeight);
		this.Location = new Point(GoBackButtonMargin, 0);
		this.Text = "\U00002B05";
		this.Region = GetRegion(this);
		this.MouseClick += new MouseEventHandler(ClickControl);
		this.MouseEnter += EnterControl;
		this.MouseLeave += LeaveControl;
	}
	private void ClickControl(object sender, MouseEventArgs e) {
		VisitPreviousScreen();
	}
	private void EnterControl(object sender, EventArgs e) {
		this.BackColor = Color.White;
		this.ForeColor = Color.Black;
	}
	private void LeaveControl(object sender, EventArgs e) {
		this.BackColor = Color.Transparent;
		this.ForeColor = Color.White;
	}
}