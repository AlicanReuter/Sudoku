namespace Core;
public class Program {
	/// <summary>
	/// Main entry of Project Core
	/// </summary>
	public static void Main(string[] args) {
		if (args[0] == "init") {
			Core.SudokuCreation.Sudoku a = new(int.Parse(args[1]), args[2]);
			a.PrintSudoku();
		}
		else if (args[0] == "load") {
			//int size = Int32.Parse(loadedSudoku["square_size"].ToString());
			Core.SudokuCreation.Sudoku a = new(args[1]);
		}
	}
}