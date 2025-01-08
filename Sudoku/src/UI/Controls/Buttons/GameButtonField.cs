#region Imports
using System;
using System.Drawing;
using System.Windows.Forms;
using static Shared.Configs.Core.SudokuCreation;
using static Shared.Configs.UI.Controls;
using static UI.Controls.Helpers.Helper;
#endregion
namespace UI.Controls.Buttons;
internal class GameButtonField : Button {
	internal readonly ButtonType buttonType;
	internal readonly int buttonIndex;
	internal GameButtonField(ButtonType type, int buttonIndex) {
		this.buttonType = type;
		this.buttonIndex = buttonIndex;
		InitializeButton();
	}
	private void InitializeButton() {
		this.Visible = true;
		this.BackColor = Color.White;
		this.FlatAppearance.BorderSize = 0;
		this.FlatStyle = FlatStyle.Flat;
		this.Size = new(GameButtonSize, GameButtonSize);
		this.Location = GetLocation();
		this.MouseClick += new MouseEventHandler(ButtonClick);
		this.Text = GetText();
	}
	private Point GetLocation() {
		//TODO:
		int row = buttonIndex % SudokuSize;
		int column = buttonIndex / SudokuSize;
		int squareRow = (row / SudokuSquareSize) + 1;
		int squareColumn = (column / SudokuSquareSize) + 1;

		int marginFieldRow = (GameButtonMargin / 2) * row;
		int marginFieldColumn = (GameButtonMargin / 2) * column;

		int marginSquareRow = GameButtonMargin * squareRow;
		int marginSquareColumn = GameButtonMargin * squareColumn;

		int xCoordinate = (this.Width * row) + marginFieldRow + marginSquareRow;
		int yCoordinate = (this.Height * column) + marginFieldColumn + marginSquareColumn;
		if (buttonIndex == 5) {
			Console.WriteLine();
		}
		return new Point(xCoordinate, yCoordinate);
	}
	private string GetText() {
		int row = buttonIndex / SudokuSize;
		int column = buttonIndex % SudokuSize;
		if (UnsolvedSudoku[row][column].ToString() == "0") {
			return string.Empty;
		}
		else {
			Console.Write(UnsolvedSudoku[row][column]);
			return UnsolvedSudoku[row][column].ToString();
		}
	}
	private void ButtonClick(object sender, MouseEventArgs mouseEventArgs) {
		UnselectControls();
		SelectEqual(sender as Control);
	}
}