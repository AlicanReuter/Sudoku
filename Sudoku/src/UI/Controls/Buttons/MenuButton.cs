#region Imports
using System.Drawing;
using System.Windows.Forms;
using UI.Controls.Helpers;
using static Shared.Configs.UI.Controls;
using static UI.Controls.Helpers.Helper;
using static UI.Controls.Helpers.NavigationControler;
#endregion
namespace UI.Controls.Buttons;
internal class MenuButton : Button {
	internal readonly int buttonIndex;
	internal readonly ButtonType buttonType;

	internal MenuButton(ButtonType type, int buttonIndex, string buttonText) {
		this.buttonIndex = buttonIndex;
		this.buttonType = type;
		InitializeButton(buttonText);
	}
	private void InitializeButton(string buttonText) {
		this.BackColor = Color.Transparent;
		this.FlatAppearance.BorderSize = 0;
		this.FlatStyle = FlatStyle.Flat;
		this.Size = new(MainMenuButtonWidth, MainMenuButtonHeight);
		this.Location = GetLocation();
		this.MouseClick += ButtonClick();
		this.Region = GetRegion(this);
		this.Text = buttonText;
	}
	private Point GetLocation() {
		int xCoordinate = 0;
		int yCoordinate = (this.Height + MainMenuButtonMargin) * buttonIndex;
		return new Point(xCoordinate, yCoordinate);
	}
	internal void SetText(string text) {
		if (text == null) { return; }
		this.Text = text;
	}
	private MouseEventHandler ButtonClick() {
		NavigationControler instance = NavigationControler.Instance;
		return buttonType switch {
			ButtonType.MainMenuPlay => new MouseEventHandler(ButtonClickMenuNavigate),
			ButtonType.MainMenuOption => new MouseEventHandler(ButtonClickMenuNavigate),
			ButtonType.MainMenuScoreboard => new MouseEventHandler(ButtonClickMenuNavigate),
			ButtonType.PlayMenuContinue => new MouseEventHandler(ButtonClickStartGame),
			ButtonType.PlayMenuEasy => new MouseEventHandler(ButtonClickStartGame),
			ButtonType.PlayMenuNormal => new MouseEventHandler(ButtonClickStartGame),
			ButtonType.PlayMenuHard => new MouseEventHandler(ButtonClickStartGame),
			ButtonType.PlayMenuExpert => new MouseEventHandler(ButtonClickStartGame),
			_ => ButtonClickMenuNavigate
		};
	}
	private void ButtonClickMenuNavigate(object sender, MouseEventArgs mouseEventArgs) {
		while (true) {
			VisitNextScreen(sender as Control);
			if (this.buttonType == ButtonType.MainMenuPlay) {
				(this.Parent.Parent.Controls[1].Controls[1] as GoBackButton).ClickControl(this, new MouseEventArgs(MouseButtons.Left, 1, 100, 50, 0));
			}
		}
	}
	private void ButtonClickStartGame(object sender, MouseEventArgs mouseEventArgs) {
		CreateSudoku(GetDifficult(this.buttonType));
		//Control game = CreateGamePanel((sender as Control));
		//AddToRootControl(game);
		//SetLocation(game);
		//VisitScreen(game);
	}
}