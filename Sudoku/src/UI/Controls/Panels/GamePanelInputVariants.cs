#region Imports
using System.Drawing;
using System.Windows.Forms;
using UI.Controls.Buttons;
using static Shared.Configs.Core.SudokuCreation;
using static Shared.Configs.UI.Controls;
using static UI.Controls.Helpers.Helper;
#endregion

namespace UI.Controls.Panels;
internal class GamePanelInputVariants : Panel {
	internal PanelType panelType;
	internal GamePanelInputVariants(PanelType type) {
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
			this.Controls.Add(new GameButtonVariant(ButtonType.SudokuVariants, buttonIndex));
		}
	}
	private Size GetSize() {
		int width = (GameButtonSize + GameButtonMargin) * SudokuSquareSize;
		int height = (GameButtonSize + GameButtonMargin) * SudokuSquareSize;
		return new Size(width, height);
	}
	internal void SetLocation() {
		int xCoordinate = 0;
		int yCoordinate = 0;
		this.Location = new(xCoordinate, yCoordinate);
	}
}