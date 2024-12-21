#region Imports
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using UI.Controls.Buttons;
using static Shared.Configs.UI.Controls;
using static UI.Controls.Helpers.Helper;
#endregion
namespace UI.Controls.Panels;
internal class PlayMenuPanel : Panel {
	internal PanelType panelType;
	internal PlayMenuPanel(PanelType type) {
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
		List<ButtonType> types = [ButtonType.PlayMenuContinue, ButtonType.PlayMenuEasy, ButtonType.PlayMenuNormal, ButtonType.PlayMenuHard, ButtonType.PlayMenuExpert];
		foreach (ButtonType type in types) {
			PlayMenuButton btn = new(type, types.IndexOf(type));
			this.Controls.Add(btn);
		}
	}
	private Size GetSize() {
		int widthMenu = PlayMenuButtonWidth;
		int heightMenu = (this.Controls.Count * (PlayMenuButtonHeight + PlayMenuButtonMargin)) - PlayMenuButtonMargin;
		return new Size(widthMenu, heightMenu);
	}
	internal void SetLocation() {
		int xCoordinate = (this.Parent.Width - this.Width) / 2;
		int yCoordinate = (this.Parent.Height - this.Height) / 2;
		this.Location = new Point(xCoordinate, yCoordinate);
	}
}