#region Imports
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using UI.Controls.Buttons;
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
		GameButtonField field = (ControlsSelected[0] as GameButtonField);
		if (field.isLocked) { return; }
		int buttonIndex = field.buttonIndex;
		int row = buttonIndex / SudokuSize;
		int col = buttonIndex % SudokuSize;
		RemoveError();
		if (ControlsSelected[0].Text == number) {
			ControlsSelected[0].Text = string.Empty;
			UnsolvedSudoku[row][col] = 0;
			field.isSolved = false;
		}
		else {
			if (int.TryParse(number, out int result)) {
				if (!IsValid(buttonIndex, result)) { return; }
			}
			else { return; }
			ControlsSelected[0].Text = number;
			ControlsSelected[0].ForeColor = Color.Blue;
			ControlsSelected[0].Font = FontPlacedNumber;
			UnsolvedSudoku[row][col] = int.Parse(number);
			field.isSolved = true;
			PlaceHistory.Add(field.buttonIndex);
		}
	}
	internal static void PlaceVariantInControl(string number) {
		if (ControlsSelected.Count <= 0) { return; }
		GameButtonField field = (ControlsSelected[0] as GameButtonField);
		if (field.isLocked) { return; }
		if (field.isSolved) { return; }
		if (ControlsSelected[0].Text.Contains(number)) { ControlsSelected[0].Text = ControlsSelected[0].Text.Replace(number, ""); }
		else { ControlsSelected[0].Text += number; }
		char[] variants = ControlsSelected[0].Text.ToCharArray();
		Array.Sort(variants);
		ControlsSelected[0].Text = new string(variants);
		ControlsSelected[0].ForeColor = Color.Gray;
		ControlsSelected[0].Font = FontVariantNumber;
		PlaceHistory.Add(field.buttonIndex);
	}
	private static void RemoveError() {
		GamePanelSudoku pnl = RootCntrl.Controls[2].Controls[4].Controls[0] as GamePanelSudoku;
		for (int i = 0; i < SudokuSize * SudokuSize; i++) {
			pnl.SetErrorAt(i, false);
		}
	}
	private static bool IsValid(int buttonIndex, int number) {
		int row = buttonIndex / SudokuSize;
		int col = buttonIndex % SudokuSize;
		GamePanelSudoku pnl = RootCntrl.Controls[2].Controls[4].Controls[0] as GamePanelSudoku;
		//Row
		for (int i = 0; i < SudokuSize; i++) {
			if (UnsolvedSudoku[row][i] == number) {
				pnl.SetErrorAt(row * SudokuSize + i, true);
				return false;
			}
		}
		//Column
		for (int i = 0; i < SudokuSize; i++) {
			if (UnsolvedSudoku[i][col] == number) {
				pnl.SetErrorAt(i * SudokuSize + col, true);
				return false;
			}
		}
		//Square
		int x = SudokuSquareSize * (row / SudokuSquareSize);
		int y = SudokuSquareSize * (col / SudokuSquareSize);
		for (int i = 0; i < SudokuSquareSize; i++) {
			for (int j = 0; j < SudokuSquareSize; j++) {
				if (UnsolvedSudoku[x + i][y + j] == number) {
					pnl.SetErrorAt((x + i) * SudokuSize + (y + j), true);
					return false;
				}
			}
		}
		return true;
	}
	internal static void UndoLastPlacedNumber(Control cntrl) {
		int lastButtonIndex = PlaceHistory.Last();
		foreach (Control child in cntrl.Controls) {
			if ((child as GameButtonField).buttonIndex != lastButtonIndex) { continue; }
			child.Text = string.Empty;
			break;
		}
	}
	#endregion

	#region ControlGamePanel
	internal static void CreateSudoku(Difficult difficulty, string sudokuJSON = default) {
		if (sudokuJSON == default) { Core.Program.Main(["init", "3", difficulty.ToString()]); }
		else {
			Core.Program.Main(["load", sudokuJSON]);
		}
	}
	[STAThread]
	internal static string LoadSudoku() {
		OpenFileDialog openFileDialog = new() {
			Title = "Load Game",
			Filter = "JSON-Dateien (*.json)|*.json|Alle Dateien (*.*)|*.*",
			InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
		};
		if (openFileDialog.ShowDialog() != DialogResult.OK) { return string.Empty; }
		else {
			string filePath = openFileDialog.FileName;
			string fileContent = File.ReadAllText(filePath);
			return fileContent;
		}
	}
	internal static void SaveSudoku() {
		if (UnsolvedSudoku == default) { return; }
		Dictionary<string, object> dicJson = new() {
			{ "square_size", SudokuSquareSize},
			{ "difficult", Difficulty.ToString()},
			{ "original", OriginalSudoku},
			{ "unsolved", UnsolvedSudoku},
			{ "solved", SolvedSudoku},
			{ "variants", SaveVariants()}
		};
		string json = JsonConvert.SerializeObject(dicJson);
		SaveFileDialog saveFileDialog = new SaveFileDialog {
			Title = "Save Game",
			Filter = "JSON-Dateien (*.json)|*.json|Alle Dateien (*.*)|*.*",
			DefaultExt = "json"
		};

		if (saveFileDialog.ShowDialog() == DialogResult.OK) {
			string filePath = saveFileDialog.FileName;
			File.WriteAllText(filePath, json);
		}
		else {
			Console.WriteLine("Speichern abgebrochen.");
		}
	}
	private static List<List<List<int>>> SaveVariants() {
		List<List<List<int>>> variants = [];
		Control cntrl = RootCntrl.Controls[2].Controls[4].Controls[0];
		int row = 0;
		int col = 0;
		List<List<int>> rowVariant = [];
		foreach (Control child in cntrl.Controls) {
			List<int> variant = [];
			if (col >= SudokuSize) {
				col = 0;
				row++;
				variants.Add(rowVariant);
				rowVariant = [];
			}
			if (child.Text.Length == 0) {
				rowVariant.Add(variant);
				col++;
				continue;
			}
			foreach (char c in child.Text.ToCharArray()) {
				if (!int.TryParse(c.ToString(), out int intVar)) { continue; }
				variant.Add(intVar);
			}
			rowVariant.Add(variant);
			col++;
		}
		variants.Add(rowVariant);
		return variants;
	}
	internal static string ConvertList(List<int> list) {
		string result = string.Empty;
		foreach (int elem in list) {
			result += elem.ToString();
		}
		return result;
	}
	internal static List<string> GetGameButtonFieldContent() {
		Control cntrl = RootCntrl.Controls[2].Controls[4].Controls[0];
		List<string> buttonContent = new();
		foreach (Control child in cntrl.Controls) {
			buttonContent.Add(child.Text);
		}
		return buttonContent;
	}

	internal static void CreateGamePanel() {
		Control child = new GamePanel(PanelType.Game);
		child.Tag = 10;
		List<Control> remove = [];
		foreach (Control cntrl in RootCntrl.Controls[2].Controls) {
			if (cntrl.Tag == null) { continue; }
			if (cntrl.Tag.Equals(10)) { remove.Add(cntrl); }
		}
		foreach (Control cntrl in remove) {
			RootCntrl.Controls[2].Controls.Remove(cntrl);
		}
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