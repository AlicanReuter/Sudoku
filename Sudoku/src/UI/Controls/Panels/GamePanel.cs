#region Imports
using System;
using System.Drawing;
using System.Windows.Forms;
using static Shared.Configs.Core.SudokuCreation;
using static Shared.Configs.UI.Controls;
using static UI.Controls.Helpers.Helper;
#endregion

namespace UI.Controls.Panels;
internal class GamePanel : Panel {
	internal PanelType panelType;
	internal GamePanel(PanelType type) {
		this.panelType = type;
		InitializePanel();
	}
	private void InitializePanel() {
		this.Visible = true;
		this.BackColor = Color.Transparent;
		AddChildControls();
		this.Size = new(MainFormWidth, MainFormHeight - TaskPanelHeight - TaskPanelHeight);
		this.Region = GetRegion(this);
	}
	//private Size GetSize() {
		//return panelType switch {
		//	PanelType.Game => new Size(this.Controls[0].Width, this.Controls[0].Height + this.Controls[1].Height + GameButtonSize + GameButtonMargin),
		//	PanelType.GamePanelSudokuPanel => new Size(SudokuSize * (GameButtonSize + GameButtonMargin), SudokuSize * (GameButtonSize + GameButtonMargin)),
		//	PanelType.GamePanelInputPanel => new Size((SudokuSquareSize * (GameButtonSize + GameButtonMargin) * this.Controls.Count) + GameButtonSize + GameButtonMargin, this.Controls[0].Height),
		//	PanelType.GameSubInputSubNumbers => new Size(SudokuSquareSize * (GameButtonSize + GameButtonMargin), SudokuSquareSize * (GameButtonSize + GameButtonMargin)),
		//	PanelType.GameSubInputSubVariants => new Size(SudokuSquareSize * (GameButtonSize + GameButtonMargin), SudokuSquareSize * (GameButtonSize + GameButtonMargin)),
		//	_ => new Size(0, 0)
		//};
	//}
	private void AddChildControls() {
		this.Controls.Add(new GamePanelSudoku(PanelType.GamePanelSudokuPanel));
		//this.Controls.Add(new GamePanelInput(PanelType.GamePanelInputPanel));
	}
}