#region Imports
using System.Drawing;
using System.Windows.Forms;
using UI.Controls.Buttons;
using static Shared.Configs.UI.Controls;
#endregion
namespace UI.Controls.Panels;
internal class MainMenuPanel : Panel {
	internal PanelType panelType;
	internal MainMenuPanel(PanelType type) {
		this.panelType = type;
		InitializeControl();
	}
	internal void InitializeControl() {
		this.Visible = false;
		this.BackColor = Color.Transparent;
		AddChilds();
		this.Size = GetSize();
	}
	private void AddChilds() {
		MainMenuButton play = new(ButtonType.MainMenuPlay, 0);
		MainMenuButton option = new(ButtonType.MainMenuOption, 1);
		MainMenuButton scoreboard = new(ButtonType.MainMenuScoreboard, 2);
		this.Controls.Add(play);
		this.Controls.Add(option);
		this.Controls.Add(scoreboard);
	}
	private Size GetSize() {
		int widthMenu = MainMenuButtonWidth;
		int heightMenu = (this.Controls.Count * (MainMenuButtonHeight + MainMenuButtonMargin)) - MainMenuButtonMargin;
		return new Size(widthMenu, heightMenu);
	}
	internal void SetLocation() {
		int xCoordinate = (this.Parent.Width - this.Width) / 2;
		int yCoordinate = (this.Parent.Height - this.Height) / 2;
		this.Location = new Point(xCoordinate, yCoordinate);
	}
}
