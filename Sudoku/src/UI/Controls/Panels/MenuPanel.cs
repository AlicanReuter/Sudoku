#region Imports
using System;
using System.Drawing;
using System.Windows.Forms;
using static Shared.Configs.UI.Controls;
using static UI.Controls.Helpers.Helper;
#endregion

namespace UI.Controls.Panels;
internal class MenuPanel : Panel {
	internal PanelType panelType;
	internal MenuPanel(PanelType type) {
		this.panelType = type;
		InitializePanel();
	}
	private void InitializePanel() {
		this.Visible = false;
		this.BackColor = Color.Transparent;
		AddChildControls();
		this.Size = GetSize();
		this.Location = GetLocation();
		this.Region = GetRegion(this);
	}
	private Size GetSize() {
		int widthMenu = MainMenuButtonWidth;
		int heightMenu = (this.Controls.Count * (MainMenuButtonHeight + MainMenuButtonMargin)) - MainMenuButtonMargin;
		return panelType switch {
			PanelType.MainMenu => new Size(widthMenu, heightMenu),
			PanelType.PlayMenu => new Size(widthMenu, heightMenu),
			PanelType.OptionMenu => new Size(widthMenu, heightMenu),
			PanelType.Scoreboard => new Size(widthMenu, heightMenu),
			_ => new Size(0, 0)
		};
	}
	private Point GetLocation() {
		int xCoordinate = (MainFormWidth - this.Width) / 2;
		int yCoordinate = (MainFormHeight - this.Height) / 2;
		return new Point(xCoordinate, yCoordinate);
	}
	private void AddChildControls() {
		int buttonIndex = 0;
		foreach ((Enum, string) childData in MenuPanelChilds[panelType]) {
			this.Controls.Add(CreateChildControl(childData, buttonIndex));
			buttonIndex++;
		}
	}
}