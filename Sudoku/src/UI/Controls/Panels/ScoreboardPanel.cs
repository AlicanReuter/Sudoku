#region Imports
using System;
using System.Drawing;
using System.Windows.Forms;
using static Shared.Configs.UI.Controls;
using static UI.Controls.Helpers.Helper;
#endregion
namespace UI.Controls.Panels;
internal class ScoreboardPanel : Panel {
	internal PanelType panelType;
	internal ScoreboardPanel(PanelType type) {
		this.panelType = type;
		InitializeControl();
	}
	internal void InitializeControl() {
		this.Visible = false;
		this.BackColor = Color.Transparent;
		AddSubControls();
		this.Size = GetSize();
	}
	private void AddSubControls() {
		int buttonIndex = 0;
		foreach ((Enum, string) childData in MenuPanelChilds[panelType]) {
			this.Controls.Add(CreateChildControl(childData, buttonIndex));
			buttonIndex++;
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
