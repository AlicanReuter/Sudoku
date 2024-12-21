#region Imports
using System;
using System.Drawing;
using System.Windows.Forms;
using static Shared.Configs.Core.SudokuCreation;
using static Shared.Configs.UI.Controls;
using static UI.Controls.Helpers.Helper;
using static UI.Controls.Helpers.NavigationControler;
#endregion
namespace UI.Controls.Buttons;
internal class PlayMenuButton : Button {
	internal ButtonType buttonType;
	internal int buttonIndex;
	internal PlayMenuButton(ButtonType type, int index) {
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
		this.Size = new(MainMenuButtonWidth, MainMenuButtonHeight);
		this.Location = GetLocation();
		this.Text = GetText();
		this.Region = GetRegion(this);
		this.MouseClick += new MouseEventHandler(ClickControl);
		this.MouseEnter += EnterControl;
		this.MouseLeave += LeaveControl;
	}
	private Point GetLocation() {
		int xCoordinate = 0;
		int yCoordinate = (MainMenuButtonHeight + MainMenuButtonMargin) * buttonIndex;
		return new Point(xCoordinate, yCoordinate);
	}
	private string GetText() {
		return buttonType switch {
			ButtonType.PlayMenuContinue => "Continue",
			ButtonType.PlayMenuEasy => "Easy",
			ButtonType.PlayMenuNormal => "Normal",
			ButtonType.PlayMenuHard => "Hard",
			ButtonType.PlayMenuExpert => "Expert",
			_ => ""
		};
	}
	private void ClickControl(object sender, MouseEventArgs e) {
		switch (buttonType) {
			case ButtonType.PlayMenuContinue:
				LoadSudoku();
				break;
			case ButtonType.PlayMenuEasy:
				CreateSudoku(Difficult.Easy);
				break;
			case ButtonType.PlayMenuNormal:
				CreateSudoku(Difficult.Normal);
				break;
			case ButtonType.PlayMenuHard:
				CreateSudoku(Difficult.Hard);
				break;
			case ButtonType.PlayMenuExpert:
				CreateSudoku(Difficult.Expert);
				break;
		}
		CreateGamePanel();
		VisitNextScreen(this);
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
