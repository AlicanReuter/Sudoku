#region Imports
using System;
using System.Collections.Generic;
#endregion
namespace Shared.Configs.UI;
public static class Controls {
	#region ControlTypes
	public enum FormType {
		None,
		MainForm,
		SubForm
	}
	public enum PanelType {
		None,
		TaskBar,
		FormPanel,
		MainMenu,
		PlayMenu,
		OptionMenu,
		Scoreboard,
		Game,
		GameSubSudoku,
		GameSubInput,
		GameSubInputSubNumbers,
		GameSubInputSubVariants
	}
	public enum ButtonType {
		None,
		TaskBarClose,
		TaskBarMaximize,
		TaskBarMinimize,
		MainMenuPlay,
		MainMenuOption,
		MainMenuScoreboard,
		PlayMenuContinue,
		PlayMenuEasy,
		PlayMenuNormal,
		PlayMenuHard,
		PlayMenuExpert,
		SudokuField,
		SudokuNumbers,
		SudokuVariants,
		GoBack
	}
	public static bool IsCorrectType(Enum type) {
		return type switch {
			FormType => true,
			PanelType => true,
			ButtonType => true,
			_ => false,
		};
	}
	#endregion

	#region ControlSize
	public static readonly int MainFormWidth = 1280;
	public static readonly int MainFormHeight = 720;
	public static readonly int TaskPanelWidth = MainFormWidth;
	public static readonly int TaskPanelHeight = 30;
	public static readonly int TaskButtonSize = 30;
	public static readonly int MainMenuButtonWidth = 300;
	public static readonly int MainMenuButtonHeight = 50;
	public static readonly int GameButtonSize = 25;
	#endregion

	#region ControlMargin
	public static readonly int MainMenuButtonMargin = 25;
	public static readonly int GameButtonMargin = 5;
	#endregion

	#region ControlChilds
	public static readonly Dictionary<PanelType, List<(Enum, string)>> MenuPanelChilds = new() {
		[PanelType.MainMenu] = [
			(ButtonType.MainMenuPlay, "Play"),
			(ButtonType.MainMenuOption, "Options"),
			(ButtonType.MainMenuScoreboard, "Scoreboard")
			],
		[PanelType.PlayMenu] = [
			(ButtonType.PlayMenuContinue, "Continue"),
			(ButtonType.PlayMenuEasy, "Easy"),
			(ButtonType.PlayMenuNormal, "Normal"),
			(ButtonType.PlayMenuHard, "Hard"),
			(ButtonType.PlayMenuExpert, "Expert"),
			],
		[PanelType.OptionMenu] = [],
		[PanelType.Scoreboard] = []
	};
	public static readonly Dictionary<PanelType, List<(Enum, string)>> GamePanelChilds = new() {
		[PanelType.Game] = [
			(PanelType.GameSubSudoku, ""),
			(PanelType.GameSubInput, "")
			],
		[PanelType.GameSubSudoku] = [(ButtonType.SudokuField, ""),],
		[PanelType.GameSubInput] = [
			(PanelType.GameSubInputSubNumbers, ""),
			(PanelType.GameSubInputSubVariants, "")
			],
		[PanelType.GameSubInputSubNumbers] = [
			(ButtonType.SudokuNumbers, "")
			],
		[PanelType.GameSubInputSubVariants] = [
			(ButtonType.SudokuVariants, "")
			]
	};
	#endregion

	#region ControlRadius
	public static readonly float Radius = 25f;
	#endregion
}