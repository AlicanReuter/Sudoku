namespace Core;
public class Program {
	/// <summary>
	/// Main entry of Project Core
	/// </summary>
	public static void Main(string[] args) {
		Core.SudokuCreation.Sudoku a = new(int.Parse(args[0]), args[1]);
		a.PrintSudoku();
	}
}