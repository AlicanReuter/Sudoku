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
		TaskMenuBar,
		FormPanel,
		MainMenu,
		PlayMenu,
		OptionMenu,
		Scoreboard,
		Game,
		GamePanelSudokuPanel,
		GamePanelInputPanel,
		GamePanelInputNumbers,
		GamePanelInputVariants
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
	#endregion

	#region ControlSize
	public static readonly int MainFormWidth = 1280;
	public static readonly int MainFormHeight = 720;
	public static readonly int TaskPanelWidth = MainFormWidth;
	public static readonly int TaskPanelHeight = 30;
	public static readonly int TaskButtonSize = 30;
	public static readonly int GoBackButtonWidth = 100;
	public static readonly int GoBackButtonHeight = 30;
	public static readonly int MainMenuButtonWidth = 300;
	public static readonly int MainMenuButtonHeight = 50;
	public static readonly int PlayMenuButtonWidth = 300;
	public static readonly int PlayMenuButtonHeight = 50;
	public static readonly int GameButtonSize = 30;
	#endregion

	#region ControlMargin
	public static readonly int GoBackButtonMargin = 25;
	public static readonly int MainMenuButtonMargin = 25;
	public static readonly int PlayMenuButtonMargin = 25;
	public static readonly int GameButtonMargin = 2;
	#endregion
}