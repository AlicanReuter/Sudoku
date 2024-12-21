#region Imports
using System;
using System.Drawing;
using System.Windows.Forms;
using UI.Controls.Buttons;
using static Shared.Configs.Core.SudokuCreation;
using static Shared.Configs.UI.Controls;
using static UI.Controls.Helpers.Helper;
#endregion

namespace UI.Controls.Panels;
internal class GamePanelSudoku : Panel {
	internal PanelType panelType;
	internal GamePanelSudoku(PanelType type) {
		this.panelType = type;
		InitializePanel();
	}
	private void InitializePanel() {
		this.Visible = true;
		this.BackColor = Color.Black;
		AddChildControls();
		this.Size = new Size(SudokuSize * (GameButtonSize + GameButtonMargin), SudokuSize * (GameButtonSize + GameButtonMargin));
		this.Region = GetRegion(this);
	}
	private void AddChildControls() {
		int buttonCount = SudokuSize * SudokuSize;
		for (int buttonIndex = 0; buttonIndex < buttonCount; buttonIndex++) {
			this.Controls.Add(new GameButtonField(ButtonType.SudokuField, buttonIndex));
		}
	}
}