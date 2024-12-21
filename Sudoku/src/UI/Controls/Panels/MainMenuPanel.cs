#region Imports
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using UI.Controls.Buttons;
using static Shared.Configs.UI.Controls;
using static UI.Controls.Helpers.Helper;
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
		this.Region = GetRegion(this);
	}
	private void AddChilds() {
		List<ButtonType> types = [ButtonType.MainMenuPlay, ButtonType.MainMenuOption, ButtonType.MainMenuScoreboard];
		foreach (ButtonType type in types) {
			MainMenuButton btn = new(type, types.IndexOf(type));
			this.Controls.Add(btn);
		}
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
