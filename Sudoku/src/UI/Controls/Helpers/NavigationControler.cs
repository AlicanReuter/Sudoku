using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using UI.Controls.Buttons;
using UI.Controls.Panels;
using static Shared.Configs.UI.Controls;

namespace UI.Controls.Helpers;
internal class NavigationControler {
	private static NavigationControler instance = null;
	private static Control rootCntrl;
	private static readonly List<Control> visited = [];
	private NavigationControler() { }
	internal static NavigationControler Instance {
		get {
			instance ??= new NavigationControler();
			return instance;
		}
	}
	internal void InitializeRootControl(Control cntrl) {
		if (rootCntrl == null) {
			rootCntrl = cntrl;
		}
		else {
			throw new InvalidOperationException("rootCntrl is already initialized.");
		}
	}
	internal static void VisitFirstScreen(Control cntrl) {
		if (cntrl == default) { return; }
		cntrl.Visible = true;
		visited.Add(cntrl);
	}
	internal static void VisitNextScreen(Control cntrl) {
		if (cntrl == default) { return; }
		Control nextCntrl = GetControl(GetNextPanelType(GetCurrentButtonType(cntrl)));
		visited.Last().Visible = false;
		nextCntrl.Visible = true;
		visited.Add(nextCntrl);
	}
	internal static void VisitPreviousScreen() {
		visited.Last().Visible = false;
		visited.RemoveAt(visited.Count() - 1);
		visited.Last().Visible = true;
	}
	private static ButtonType GetCurrentButtonType(Control cntrl) { return ((MenuButton) cntrl).buttonType; }
	private static PanelType GetCurrentPanelType(Control cntrl) {
		Control parent = cntrl.Parent;
		if (parent.GetType() == typeof(MenuPanel)) { return ((MenuPanel) parent).panelType; }
		if (parent.GetType() == typeof(GamePanel)) { return ((GamePanel) parent).panelType; }
		return PanelType.None;
	}
	private static PanelType GetNextPanelType(ButtonType type) {
		return type switch {
			ButtonType.MainMenuPlay => PanelType.PlayMenu,
			ButtonType.MainMenuOption => PanelType.OptionMenu,
			ButtonType.MainMenuScoreboard => PanelType.Scoreboard,
			ButtonType.PlayMenuContinue => PanelType.Game,
			ButtonType.PlayMenuEasy => PanelType.Game,
			ButtonType.PlayMenuNormal => PanelType.Game,
			ButtonType.PlayMenuHard => PanelType.Game,
			ButtonType.PlayMenuExpert => PanelType.Game,
			_ => PanelType.MainMenu
		};
	}
	private static Control GetControl(PanelType nextType) {
		Control cntrl = default;
		foreach (Control child in rootCntrl.Controls) {
			if (child.GetType() == typeof(MenuPanel)) {
				if (((MenuPanel) child).panelType == nextType) { cntrl = child; break; }
			}
			if (child.GetType() == typeof(GamePanel)) {
				if (((GamePanel) child).panelType == nextType) { cntrl = child; break; }
			}
		}
		return cntrl;
	}
	internal static void AddToRootControl(Control cntrl) { rootCntrl.Controls.Add(cntrl); }
	internal static void VisitScreen(Control cntrl) {
		visited.Last().Visible = false;
		cntrl.Visible = true;
		visited.Add(cntrl);
	}
}