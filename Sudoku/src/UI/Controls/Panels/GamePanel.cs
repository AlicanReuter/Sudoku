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
		AddChildControls();
		this.BackColor = GetColor();
		this.Size = GetSize();
		this.Region = GetRegion(this);
	}
	private Color GetColor() {
		if (this.panelType == PanelType.GameSubSudoku) { return Color.Black; }
		return Color.Transparent;
		//return panelType switch {
		//	PanelType.Game => Color.Red,
		//	PanelType.GameSubSudoku => Color.Black,
		//	PanelType.GameSubInput => Color.Blue,
		//	PanelType.GameSubInputSubNumbers => Color.Green,
		//	PanelType.GameSubInputSubVariants => Color.Yellow,
		//	_ => Color.Transparent
		//};
	}
	private Size GetSize() {
		return panelType switch {
			PanelType.Game => new Size(this.Controls[0].Width, this.Controls[0].Height + this.Controls[1].Height + GameButtonSize + GameButtonMargin),
			PanelType.GameSubSudoku => new Size(SudokuSize * (GameButtonSize + GameButtonMargin), SudokuSize * (GameButtonSize + GameButtonMargin)),
			PanelType.GameSubInput => new Size((SudokuSquareSize * (GameButtonSize + GameButtonMargin) * this.Controls.Count) + GameButtonSize + GameButtonMargin, this.Controls[0].Height),
			PanelType.GameSubInputSubNumbers => new Size(SudokuSquareSize * (GameButtonSize + GameButtonMargin), SudokuSquareSize * (GameButtonSize + GameButtonMargin)),
			PanelType.GameSubInputSubVariants => new Size(SudokuSquareSize * (GameButtonSize + GameButtonMargin), SudokuSquareSize * (GameButtonSize + GameButtonMargin)),
			_ => new Size(0, 0)
		};
	}
	private void AddChildControls() {
		foreach ((Enum, string) childData in GamePanelChilds[panelType]) {
			if (childData.Item1.GetType() == typeof(PanelType)) {
				this.Controls.Add(CreateChildControl(childData));
			}
			if (childData.Item1.GetType() == typeof(ButtonType)) {
				int buttonCount = SudokuSize;
				if ((ButtonType) childData.Item1 == ButtonType.SudokuField) { buttonCount *= SudokuSize; }
				for (int buttonIndex = 0; buttonIndex < buttonCount; buttonIndex++) {
					this.Controls.Add(CreateChildControl(childData, buttonIndex));
				}
			}
		}
	}
}