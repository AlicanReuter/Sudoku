#region Imports
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using UI.Controls.Buttons;
using static Shared.Configs.UI.Controls;
using static UI.Controls.Helpers.Helper;
#endregion
namespace UI.Controls.Panels;
internal class OptionMenuPanel : Panel {
	internal PanelType panelType;
	internal OptionMenuPanel(PanelType type) {
		this.panelType = type;
		InitializeControl();
	}
	internal void InitializeControl() {
		this.Visible = false;
		this.BackColor = Color.DarkGray;//Color.Transparent;
		AddChilds();
		this.Size = GetSize();
		this.Region = GetRegion(this);
	}
	private void AddChilds() {
		//TODO: Add Options
	}
	private Size GetSize() {
		int widthMenu = 300;
		int heightMenu = 50;
		return new Size(widthMenu, heightMenu);
	}
	internal void SetLocation() {
		int xCoordinate = (this.Parent.Width - this.Width) / 2;
		int yCoordinate = (this.Parent.Height - this.Height) / 2;
		this.Location = new Point(xCoordinate, yCoordinate);
	}
}