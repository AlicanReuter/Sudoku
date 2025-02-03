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
	internal bool isLocked;
	internal bool isSolved;
	internal GameButtonField(ButtonType type, int buttonIndex) {
		this.buttonType = type;
		this.buttonIndex = buttonIndex;
		this.isLocked = false;
		this.isSolved = false;
		InitializeButton();
	}
	private void InitializeButton() {
		this.Visible = true;
		this.BackColor = Color.White;
		this.FlatAppearance.BorderSize = 0;
		this.FlatStyle = FlatStyle.Flat;
		this.Font = FontKnownNumber;
		this.ForeColor = Color.Black;
		this.Size = new(GameButtonSize, GameButtonSize);
		this.Location = GetLocation();
		this.MouseClick += new MouseEventHandler(ButtonClick);
		this.Text = GetOriginal();
		this.Text = GetUnsolved();
		this.Text = GetVariants();
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
	private string GetOriginal() {
		int row = buttonIndex / SudokuSize;
		int column = buttonIndex % SudokuSize;
		if (OriginalSudoku[row][column].ToString() == "0") {
			return string.Empty;
		}
		else {
			Console.Write(OriginalSudoku[row][column]);
			this.isLocked = true;
			return OriginalSudoku[row][column].ToString();
		}
	}
	private string GetUnsolved() {
		if (this.Text.Length > 0) { return this.Text; }
		int row = buttonIndex / SudokuSize;
		int column = buttonIndex % SudokuSize;
		if (UnsolvedSudoku[row][column].ToString() == "0") {
			return string.Empty;
		}
		else {
			this.ForeColor = Color.Blue;
			this.isSolved = true;
			return UnsolvedSudoku[row][column].ToString();
		}
	}
	private string GetVariants() {
		if (this.Text.Length > 0) { return this.Text; }
		if (VariantSudoku == default) { return this.Text; }
		int row = buttonIndex / SudokuSize;
		int column = buttonIndex % SudokuSize;
		if (VariantSudoku[row][column].Count < 1) {
			return string.Empty;
		}
		else {
			this.ForeColor = Color.Gray;
			this.Font = FontVariantNumber;
			return ConvertList(VariantSudoku[row][column]);
		}
	}
	public void ShowError(bool isError) {
		if (isError) { this.ForeColor = Color.Red; }
		else if (this.isLocked) { this.ForeColor = Color.Black; }
		else if (this.isSolved) { this.ForeColor = Color.Blue; }
		else { this.ForeColor = Color.Gray; }
	}
	private void ButtonClick(object sender, MouseEventArgs mouseEventArgs) {
		UnselectControls();
		SelectEqual(sender as Control);
	}
}