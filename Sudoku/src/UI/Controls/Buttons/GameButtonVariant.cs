#region Imports
using System.Drawing;
using System.Windows.Forms;
using static Shared.Configs.Core.SudokuCreation;
using static Shared.Configs.UI.Controls;
using static UI.Controls.Helpers.Helper;
#endregion
namespace UI.Controls.Buttons;
internal class GameButtonVariant : Button {
	internal readonly ButtonType buttonType;
	internal readonly int buttonIndex;
	internal GameButtonVariant(ButtonType type, int buttonIndex) {
		this.buttonType = type;
		this.buttonIndex = buttonIndex;
		InitializeButton();
	}
	private void InitializeButton() {
		this.Visible = true;
		this.BackColor = Color.AliceBlue;
		this.FlatAppearance.BorderSize = 0;
		this.FlatStyle = FlatStyle.Flat;
		this.Size = new(GameButtonSize, GameButtonSize);
		this.Location = GetLocation();
		this.MouseClick += new MouseEventHandler(ButtonClick);
		this.Region = GetRegion(this);
		this.Text = (buttonIndex + 1).ToString();
	}
	private Point GetLocation() {
		int xCoordinate = (this.Width + GameButtonMargin) * (buttonIndex % SudokuSquareSize);
		int yCoordinate = (this.Height + GameButtonMargin) * (buttonIndex / SudokuSquareSize);
		return new Point(xCoordinate, yCoordinate);
	}
	private void ButtonClick(object sender, MouseEventArgs mouseEventArgs) {
		PlaceVariantInControl((sender as Control).Text);
	}
}