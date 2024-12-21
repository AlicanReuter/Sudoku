#region Imports
using System.Drawing;
using System.Windows.Forms;
using UI.Controls.Buttons;
using static Shared.Configs.Core.SudokuCreation;
using static Shared.Configs.UI.Controls;
using static UI.Controls.Helpers.Helper;
#endregion

namespace UI.Controls.Panels;
internal class GamePanelInputNumbers : Panel {
	internal PanelType panelType;
	internal GamePanelInputNumbers(PanelType type) {
		this.panelType = type;
		InitializePanel();
	}
	private void InitializePanel() {
		this.Visible = true;
		this.BackColor = Color.Transparent;
		AddChildControls();
		this.Size = GetSize();
		this.Region = GetRegion(this);
	}
	private void AddChildControls() {
		for (int buttonIndex = 0; buttonIndex < SudokuSize; buttonIndex++) {
			this.Controls.Add(new GameButtonNumber(ButtonType.SudokuNumbers, buttonIndex));
		}
	}
	private Size GetSize() {
		int width = (GameButtonSize + GameButtonMargin) * SudokuSquareSize;
		int height = (GameButtonSize + GameButtonMargin) * SudokuSquareSize;
		return new Size(width, height);
	}
	internal void SetLocation() {
		int xCoordinate = this.Parent.Width - this.Width;
		int yCoordinate = 0;
		this.Location = new(xCoordinate, yCoordinate);
	}
}