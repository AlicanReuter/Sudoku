#region Imports
using System.Drawing;
using System.Windows.Forms;
using UI.Controls.Buttons;
using static Shared.Configs.Core.SudokuCreation;
using static Shared.Configs.UI.Controls;
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
		AddChilds();
		this.Size = GetSize();
	}
	private void AddChilds() {
		int buttonCount = SudokuSize * SudokuSize;
		for (int buttonIndex = 0; buttonIndex < buttonCount; buttonIndex++) {
			this.Controls.Add(new GameButtonField(ButtonType.SudokuField, buttonIndex));
		}
	}
	private Size GetSize() {
		Control lastChild = this.Controls[this.Controls.Count - 1];
		int width = lastChild.Location.X + lastChild.Width + GameButtonMargin;
		int height = lastChild.Location.Y + lastChild.Height + GameButtonMargin;
		return new Size(width, height);
	}
	internal void SetLocation() {
		int xCoordinate = (this.Parent.Width - this.Width) / 2;
		int yCoordinate = (this.Parent.Height - (this.Parent.Height / 3) - this.Height) / 2;
		this.Location = new Point(xCoordinate, yCoordinate);
	}
	internal void SetErrorAt(int buttonIndex, bool error) {
		foreach (Control child in this.Controls) {
			GameButtonField button = child as GameButtonField;
			if (button.buttonIndex == buttonIndex) {
				button.ShowError(error);
				return;
			}
		}
	}
}