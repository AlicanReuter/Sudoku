#region Imports
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static Shared.Configs.UI.Controls;
#endregion
namespace UI.Controls.Helpers;
internal class NavigationControler {
	internal static Control RootCntrl { get; set; }
	private static readonly List<Control> visited = [];

	#region NavigationFunctions
	internal static void VisitFirstScreen(Control cntrl) {
		foreach (Control child in cntrl.Controls) {
			if ((child as dynamic).panelType != PanelType.MainMenu) { continue; }
			child.Visible = true;
			visited.Add(child);
			break;
		}
	}
	internal static void VisitNextScreen(Control cntrl) {
		Control nextCntrl = FindNextControl(cntrl);
		visited.Last().Visible = false;
		nextCntrl.Visible = true;
		visited.Add(nextCntrl);
		EnableGoBackButton();
	}
	internal static void VisitPreviousScreen() {
		visited.Last().Visible = false;
		visited.RemoveAt(visited.Count() - 1);
		visited.Last().Visible = true;
		DisableGoBackButton();
	}
	private static Control FindNextControl(Control cntrl) {
		PanelType nextControlType = GetNextPanelType(cntrl);
		Control parent = cntrl.Parent.Parent;
		foreach (Control child in parent.Controls) {
			if (child.GetType() == typeof(Button)) { continue; }
			if ((child as dynamic).panelType != nextControlType) { continue; }
			return child;
		}
		return default;
	}
	private static PanelType GetNextPanelType(Control cntrl) {
		ButtonType type = (cntrl as dynamic).buttonType;
		return type switch {
			ButtonType.MainMenuPlay => PanelType.PlayMenu,
			ButtonType.MainMenuOption => PanelType.OptionMenu,
			ButtonType.MainMenuScoreboard => PanelType.Scoreboard,
			ButtonType.PlayMenuContinue => PanelType.Game,
			ButtonType.PlayMenuEasy => PanelType.Game,
			ButtonType.PlayMenuNormal => PanelType.Game,
			ButtonType.PlayMenuHard => PanelType.Game,
			ButtonType.PlayMenuExpert => PanelType.Game,
			_ => PanelType.None
		};
	}
	#endregion

	#region NavigationHelperFunctions
	private static void EnableGoBackButton() { RootCntrl.Controls[1].Controls[0].Enabled = true; }
	private static void DisableGoBackButton() { if (visited.Count <= 1) { RootCntrl.Controls[1].Controls[0].Enabled = false; } }
	#endregion
}