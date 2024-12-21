#region Imports
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using UI.Controls.Panels;
using static Shared.Configs.Core.SudokuCreation;
using static Shared.Configs.UI.Controls;
using static UI.Controls.Helpers.NavigationControler;
#endregion
namespace UI.Controls.Helpers;
internal static class Helper {
	#region ControlRegion
	internal static Region GetRegion(Control control) {
		Rectangle rect = new(0, 0, control.Width, control.Height);
		float radius = 25;
		float bottomRight = 0f;
		float bottomLeft = 90f;
		float topLeft = 180f;
		float topRight = 270f;
		float angle = 90f;
		using GraphicsPath path = new();
		path.StartFigure();
		path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, bottomRight, angle);
		path.AddArc(rect.Left, rect.Bottom - radius, radius, radius, bottomLeft, angle);
		path.AddArc(rect.Left, rect.Top, radius, radius, topLeft, angle);
		path.AddArc(rect.Right - radius, rect.Top, radius, radius, topRight, angle);
		path.CloseFigure();
		return new Region(path);
	}
	#endregion

	#region ControlSelected
	internal static List<Control> ControlsSelected = [];
	internal static void UnselectControls() {
		foreach (Control cntrl in ControlsSelected) { cntrl.BackColor = Color.White; }
		ControlsSelected.Clear();
	}
	internal static void SelectEqual(Control cntrl) {
		ControlsSelected.Add(cntrl);
		cntrl.BackColor = Color.LemonChiffon;
		if (cntrl.Text == string.Empty) { return; }
		foreach (Control child in cntrl.Parent.Controls) {
			if (child.Text == "") { continue; }
			if (child.Text != cntrl.Text) { continue; }
			if (child == cntrl) { continue; }
			child.BackColor = Color.LemonChiffon;
			ControlsSelected.Add(child);
		}
	}
	internal static void PlaceNumberInControl(string number) {
		if (ControlsSelected.Count <= 0) { return; }
		if (ControlsSelected[0].Text == number) { ControlsSelected[0].Text = string.Empty; }
		else { ControlsSelected[0].Text = number; }
	}
	internal static void PlaceVariantInControl(string number) {
		if (ControlsSelected.Count <= 0) { return; }
		ControlsSelected[0].Text = number;
	}
	#endregion

	#region ControlGamePanel
	internal static void CreateSudoku(Difficult difficulty) { Core.Program.Main(["3", difficulty.ToString()]); }
	internal static void LoadSudoku() {
		//TODO: Load saved Sudoku Function
		Core.Program.Main(["3", Difficult.Expert.ToString()]);
	}
	internal static void CreateGamePanel() {
		Control child = new GamePanel(PanelType.Game);
		RootCntrl.Controls[2].Controls.Add(child);
	}
	internal static void SetLocation(Control cntrl) {
		if (cntrl.GetType() != typeof(GamePanel)) { return; }
		cntrl.Location = CalcPoint(cntrl, (cntrl as GamePanel).panelType);
		if (cntrl.Controls.Count == 0) { return; }
		foreach (Control child in cntrl.Controls) {
			SetLocation(child);
		}
	}
	internal static Point CalcPoint(Control cntrl, PanelType type) {
		int xCoordinate;
		int yCoordinate;
		switch (type) {
			case PanelType.Game:
				xCoordinate = (cntrl.Parent.Width - cntrl.Width) / 2;
				yCoordinate = (cntrl.Parent.Height - cntrl.Height) / 2;
				break;
			case PanelType.GamePanelSudokuPanel:
				xCoordinate = 0;
				yCoordinate = 0;
				break;
			case PanelType.GamePanelInputPanel:
				xCoordinate = (cntrl.Parent.Width - cntrl.Width) / 2;
				yCoordinate = cntrl.Parent.Controls[0].Height + GameButtonSize + GameButtonMargin;
				break;
			case PanelType.GamePanelInputNumbers:
				xCoordinate = cntrl.Parent.Width - cntrl.Width;
				yCoordinate = 0;
				break;
			case PanelType.GamePanelInputVariants:
				xCoordinate = 0;
				yCoordinate = 0;
				break;
			default:
				xCoordinate = 0;
				yCoordinate = 0;
				break;
		}
		return new Point(xCoordinate, yCoordinate);
	}
	#endregion
}