#region Imports
using System.Drawing;
using System.Windows.Forms;
using static Shared.Configs.UI.Controls;
using static UI.Controls.Helpers.Helper;
#endregion

namespace UI.Controls.Panels;
internal class GamePanel : Panel {
	internal PanelType panelType;
	internal GamePanel(PanelType type) {
		this.panelType = type;
		InitializePanel();
	}
	private void InitializePanel() {
		this.Visible = true;
		this.BackColor = Color.Transparent;
		this.Size = new(MainFormWidth, MainFormHeight - TaskPanelHeight - TaskPanelHeight);
		this.Region = GetRegion(this);
		AddChildControls();
	}
	private void AddChildControls() {
		GamePanelSudoku pnlSudoku = new(PanelType.GamePanelSudokuPanel);
		GamePanelInput pnlInput = new(PanelType.GamePanelInputPanel);
		this.Controls.Add(pnlSudoku);
		this.Controls.Add(pnlInput);
		pnlSudoku.SetLocation();
		pnlInput.SetLocation();
	}
}