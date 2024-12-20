#region Imports
using System.Drawing;
using System.Windows.Forms;
using static Shared.Configs.UI.Controls;
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
		AddSubControls();
		this.Size = GetSize();
	}
	private void AddSubControls() {

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
