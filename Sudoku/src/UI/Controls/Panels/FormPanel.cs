#region Imports
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static Shared.Configs.UI.Controls;
using static UI.Controls.Helpers.NavigationControler;
using static UI.Controls.Helpers.Helper;
using UI.Controls.Buttons;
#endregion
namespace UI.Controls.Panels;
internal class FormPanel : Panel {
	internal PanelType panelType;
	internal FormPanel(PanelType type) {
		this.panelType = type;
		InitializeControl();
		VisitFirstScreen(this);
	}
	private void InitializeControl() {
		this.Visible = true;
		this.BackColor = Color.Transparent;
		this.Size = new Size(MainFormWidth, MainFormHeight - TaskPanelHeight);
		this.Region = GetRegion(this);
		this.Location = new Point(0, TaskPanelHeight + TaskPanelHeight);
		AddChilds();
	}
	private void AddChilds() {
		MainMenuPanel mainMenu = new(PanelType.MainMenu);
		PlayMenuPanel playMenu = new(PanelType.PlayMenu);
		OptionMenuPanel optionMenu = new(PanelType.OptionMenu);
		ScoreboardPanel scoreboard = new(PanelType.Scoreboard);
		this.Controls.Add(mainMenu);
		this.Controls.Add(playMenu);
		this.Controls.Add(optionMenu);
		this.Controls.Add(scoreboard);
		mainMenu.SetLocation();
		playMenu.SetLocation();
		optionMenu.SetLocation();
		scoreboard.SetLocation();
	}
}