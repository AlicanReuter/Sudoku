#region Imports
using System.Drawing;
using System.Windows.Forms;
using static Shared.Configs.Core.SudokuCreation;
using static Shared.Configs.UI.Controls;
using static UI.Controls.Helpers.Helper;
#endregion
namespace UI.Controls.Panels;
internal class GamePanelInput : Panel {
	internal PanelType panelType;
	internal GamePanelInput(PanelType type) {
		this.panelType = type;
		InitializePanel();
	}
	private void InitializePanel() {
		this.Visible = true;
		this.BackColor = Color.Transparent;
		this.Size = GetSize();
		AddChildControls();
		this.Region = GetRegion(this);
	}
	private Size GetSize() {
		int width = (GameButtonSize + GameButtonMargin) * SudokuSize;
		int height = (GameButtonSize + GameButtonMargin) * SudokuSquareSize;
		return new Size(width, height);
	}
	private void AddChildControls() {
		GamePanelInputNumbers pnlNumbers = new(PanelType.GamePanelInputNumbers);
		GamePanelInputVariants pnlVariants = new(PanelType.GamePanelInputVariants);
		this.Controls.Add(pnlNumbers);
		this.Controls.Add(pnlVariants);
		pnlNumbers.SetLocation();
		pnlVariants.SetLocation();
	}
	internal void SetLocation() {
		int xCoordinate = (this.Parent.Width - this.Width) / 2;
		int yCoordinate = this.Parent.Height - (this.Parent.Height / 3);
		this.Location = new(xCoordinate, yCoordinate);
	}
}