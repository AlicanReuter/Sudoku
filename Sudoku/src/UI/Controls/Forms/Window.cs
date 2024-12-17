using System.Drawing;
using System.Windows.Forms;
using UI.Controls.Helpers;
using UI.Controls.Panels;
using static Shared.Configs.UI.Controls;
using static UI.Controls.Helpers.Helper;
using static UI.Controls.Helpers.NavigationControler;

namespace UI.Controls.Forms;
internal class Window : Form {
	private NavigationControler instance;
	internal FormType formType;
	internal Window() {
		this.formType = FormType.MainForm;
		InitializeWindow();
		InitializeNavigationControler();
		NavigateToFirstScreen();
	}
	private void InitializeWindow() {
		this.Size = new(MainFormWidth, MainFormHeight);
		this.FormBorderStyle = FormBorderStyle.None;
		this.StartPosition = FormStartPosition.CenterScreen;
		this.Region = GetRegion(this);
		this.BackColor = Color.Gray;
		AddChildControl();
	}
	private void AddChildControl() {
		//this.Controls.Add(new TaskBar(PanelType.TaskBar));
		this.Controls.Add(new MenuPanel(PanelType.MainMenu));
		this.Controls.Add(new MenuPanel(PanelType.PlayMenu));
		this.Controls.Add(new MenuPanel(PanelType.OptionMenu));
		this.Controls.Add(new MenuPanel(PanelType.Scoreboard));
	}
	private void InitializeNavigationControler() {
		instance = NavigationControler.Instance;
		instance.InitializeRootControl(this);
	}
	private void NavigateToFirstScreen() {
		VisitFirstScreen(this.Controls[0]);
	}
}
