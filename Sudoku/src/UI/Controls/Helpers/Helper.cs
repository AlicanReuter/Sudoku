#region Imports
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using UI.Controls.Buttons;
using UI.Controls.Panels;
using static Shared.Configs.Core.SudokuCreation;
using static Shared.Configs.UI.Controls;
#endregion
namespace UI.Controls.Helpers;
internal static class Helper {
	#region ControlRegion
	internal static Region GetRegion(Control control) {
		Rectangle rect = new(0, 0, control.Width, control.Height);
		float bottomRight = 0f;
		float bottomLeft = 90f;
		float topLeft = 180f;
		float topRight = 270f;
		float angle = 90f;
		using GraphicsPath path = new();
		path.StartFigure();
		path.AddArc(rect.Right - Radius, rect.Bottom - Radius, Radius, Radius, bottomRight, angle);
		path.AddArc(rect.Left, rect.Bottom - Radius, Radius, Radius, bottomLeft, angle);
		path.AddArc(rect.Left, rect.Top, Radius, Radius, topLeft, angle);
		path.AddArc(rect.Right - Radius, rect.Top, Radius, Radius, topRight, angle);
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

	#region ControlCreateChild
	internal static Control CreateChildControl((Enum, string) childData, int buttonIndex = default) {
		if (childData.Item1.GetType() == typeof(ButtonType)) {
			return childData.Item1 switch {
				//ButtonType.TaskBarClose => new TaskButton(ButtonType.TaskBarClose, buttonIndex, childData.Item2),
				//ButtonType.TaskBarMaximize => new TaskButton(ButtonType.TaskBarMaximize, buttonIndex, childData.Item2),
				//ButtonType.TaskBarMinimize => new TaskButton(ButtonType.TaskBarMinimize, buttonIndex, childData.Item2),
				ButtonType.MainMenuPlay => new MenuButton(ButtonType.MainMenuPlay, buttonIndex, childData.Item2),
				ButtonType.MainMenuOption => new MenuButton(ButtonType.MainMenuOption, buttonIndex, childData.Item2),
				ButtonType.MainMenuScoreboard => new MenuButton(ButtonType.MainMenuScoreboard, buttonIndex, childData.Item2),
				ButtonType.PlayMenuContinue => new MenuButton(ButtonType.PlayMenuContinue, buttonIndex, childData.Item2),
				ButtonType.PlayMenuEasy => new MenuButton(ButtonType.PlayMenuEasy, buttonIndex, childData.Item2),
				ButtonType.PlayMenuNormal => new MenuButton(ButtonType.PlayMenuNormal, buttonIndex, childData.Item2),
				ButtonType.PlayMenuHard => new MenuButton(ButtonType.PlayMenuHard, buttonIndex, childData.Item2),
				ButtonType.PlayMenuExpert => new MenuButton(ButtonType.PlayMenuExpert, buttonIndex, childData.Item2),
				ButtonType.SudokuField => new GameButtonField(ButtonType.SudokuField, buttonIndex),
				ButtonType.SudokuNumbers => new GameButtonNumber(ButtonType.SudokuNumbers, buttonIndex),
				ButtonType.SudokuVariants => new GameButtonVariant(ButtonType.SudokuVariants, buttonIndex),
				_ => new Control()
			};
		}
		else if (childData.Item1.GetType() == typeof(PanelType)) {
			return childData.Item1 switch {
				//PanelType.TaskBar => new TaskPanel(PanelType.TaskBar),
				PanelType.MainMenu => new MenuPanel(PanelType.MainMenu),
				PanelType.PlayMenu => new MenuPanel(PanelType.PlayMenu),
				PanelType.OptionMenu => new MenuPanel(PanelType.OptionMenu),
				PanelType.Scoreboard => new MenuPanel(PanelType.Scoreboard),
				PanelType.Game => new GamePanel(PanelType.Game),
				PanelType.GameSubSudoku => new GamePanel(PanelType.GameSubSudoku),
				PanelType.GameSubInput => new GamePanel(PanelType.GameSubInput),
				PanelType.GameSubInputSubNumbers => new GamePanel(PanelType.GameSubInputSubNumbers),
				PanelType.GameSubInputSubVariants => new GamePanel(PanelType.GameSubInputSubVariants),
				_ => new Control()
			};
		}
		return default;
	}
	#endregion

	#region ControlGamePanel
	internal static Difficult GetDifficult(ButtonType type) {
		return type switch {
			ButtonType.PlayMenuContinue => Difficulty,
			ButtonType.PlayMenuEasy => Difficult.Easy,
			ButtonType.PlayMenuNormal => Difficult.Normal,
			ButtonType.PlayMenuHard => Difficult.Hard,
			ButtonType.PlayMenuExpert => Difficult.Expert,
			_ => Difficult.None,
		};
	}
	internal static void CreateSudoku(Difficult difficulty) { Core.Program.Main(["3", difficulty.ToString()]); }
	internal static Control CreateGamePanel(Control cntrl) {
		Control parent = cntrl.Parent.Parent;
		Control child = (new GamePanel(PanelType.Game)) as Control;
		parent.Controls.Add(child);
		return child;
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
			case PanelType.GameSubSudoku:
				xCoordinate = 0;
				yCoordinate = 0;
				break;
			case PanelType.GameSubInput:
				xCoordinate = (cntrl.Parent.Width - cntrl.Width) / 2;
				yCoordinate = cntrl.Parent.Controls[0].Height + GameButtonSize + GameButtonMargin;
				break;
			case PanelType.GameSubInputSubNumbers:
				xCoordinate = cntrl.Parent.Width - cntrl.Width;
				yCoordinate = 0;
				break;
			case PanelType.GameSubInputSubVariants:
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