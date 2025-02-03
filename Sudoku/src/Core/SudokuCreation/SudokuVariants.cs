#region Imports
using System;
using System.Collections.Generic;
using System.Linq;
using static Shared.Configs.Core.SudokuCreation;
#endregion
namespace Core.SudokuCreation;
internal static class SudokuSolver {
	// Erweiterung der Klasse UnsolvedSudoku
	internal static void SolveSudoku(this Sudoku sudoku) {
		CurrentTransformation = Transformation.Row;
		GuessHistory = [];
		CreateVariantSudoku();
		Solve();
		CreateSolvedSudoku();
		GuessHistory.Clear();
	}

	#region SolveFunctions
	//	Versuche das UnsolvedSudoku mittels einer while-Schleife zu lösen indem Varianten zuerst Eliminiert, dann Ausgeschlossen und falls nötig Erraten werden.
	//	Wenn Zahlen Eleminiert wurden, kann es sein, dass weiterhin Zahlen Eleminiert werden können, weswegen die Schleife von vorne beginnt.
	//	Nach jeder Elimination/Aussschließung/Erraten muss geprüft werden, ob das UnsolvedSudoku gelöst wurde.
	//	Falls eine Erratene Variante Falsch ist, muss zuerst der letzte "korrekte" stand des VariantSudoku wiederhergestellt werden.
	private static void Solve() {
		while (true) {
			if (Eliminate()) { continue; }
			if (IsSudokuSolved()) { break; }
			if (Exclusion()) { continue; }
			if (IsSudokuSolved()) { break; }
			if (!IsSudokuValid()) {
				RepairLastGuess();
				continue;
			}
			Guess();
			if (IsSudokuSolved()) { break; }
		}
	}

	//	Prüfe, ob das UnsolvedSudoku gelöst ist. Dies tritt ein, wenn in jedem Feld nur eine Variantenzahl vorhanden ist.
	private static bool IsSudokuSolved() {
		foreach (List<List<int>> row in VariantSudoku) {
			foreach (List<int> variant in row) {
				if (variant.Count > 1) { return false; }
			}
		}
		return true;
	}

	//	Prüfe, ob es sich um ein Korrektes UnsolvedSudoku handelt. Ein UnsolvedSudoku ist korrekt, wenn jedes Feld mind. einen Variantenwert aufweisen kann.
	private static bool IsSudokuValid() {
		bool isValid = IsSudokuValidForState(Transformation.Row);
		if (!isValid) { return isValid; }
		isValid = IsSudokuValidForState(Transformation.Column);
		if (!isValid) { return isValid; }
		isValid = IsSudokuValidForState(Transformation.Square);
		return isValid;
	}

	//	Prüfe ob es sich um ein Korrektes UnsolvedSudoku handelt. Ein UnsolvedSudoku ist korrekt, wenn jedes Feld mind. einen Variantenwert aufweisen kann.
	//	Vorgehensweise:
	//		1.	Transformiere das "Reihen"-UnsolvedSudoku in ein SudokuState-UnsolvedSudoku, je nach dem welcher transformation übergeben wird.
	//		2.	Der bool "isValid" gibt an, ob das UnsolvedSudoku korrekt ist.
	//		3.	Durchlaufe alle Reihen des SudokuState-UnsolvedSudoku.
	//			3.1.	Erstelle eine neue Liste aus Zahlen, welche nicht mehr vorkommen dürfen.
	//		4.	Durchlaufe alle felder jeder Reihe.
	//			4.1.	Wenn ein Feld keine Elemente hat, handelt es sich um kein korrektes UnsolvedSudoku.
	//			4.2.	Wenn ein Feld nur ein Element hat, kann dies Ignoriert werden und das nächste Element kann überprüft werden.
	//			4.3.	Wenn ein Feld aus genau einem Element besteht, muss überprüft werden,
	//					ob dieses Element bereits in der Liste aus 3.1. enthalten ist.
	//					Falls ja, dann ist das UnsolvedSudoku nicht korrekt.
	//					Falls nein, dann füge das Element der Liste aus 3.1 hinzu und mach mit dem nächsten Feld weiter.
	//		5.	Transormiere das SudokuState-UnsolvedSudoku wieder in ein "Reihen"-UnsolvedSudoku zurück und gib den bool isValid zurück.
	private static bool IsSudokuValidForState(Transformation transformation) {
		if (transformation == Transformation.Column) { TransformColumn(false); }
		else if (transformation == Transformation.Square) { TransformSquare(false); }
		bool isValid = true;
		foreach (List<List<int>> rows in VariantSudoku) {
			List<int> numbers = [];
			foreach (List<int> field in rows) {
				if (field.Count == 0) {
					isValid = false;
					break;
				}
				if (field.Count != 1) { continue; }
				if (numbers.Contains(field[0])) {
					isValid = false;
					break;
				}
				numbers.Add(field[0]);
			}
			if (!isValid) { break; }
		}
		if (transformation == Transformation.Column) { TransformColumn(true); }
		else if (transformation == Transformation.Square) { TransformSquare(true); }
		return isValid;
	}
	#endregion

	#region Transformations
	//	Transformiert jede Spalte des Sudokus in eine Reihe des Sudokus.
	//	Vorgehensweise:
	//		1.	Erstelle eine neue Liste "tmp", welches das veränderte Varianten-UnsolvedSudoku beinhalten soll.
	//		2.	Durchlaufe jede Reihe und erstelle eine temporäre liste "tmpRow", welche die Spalte als Reihe darstellen soll.
	//		3.	Durchlaufe jede Spalte und füge das element, welches momentan besucht wird tmpRow hinzu.
	//			Hierbei muss dabei mit umgedrehten indizes (column, row) anstatt (row, column) auf VariantSudoku zugegriffen werden,
	//			da alle elemente der Spalte hinzugefügt werden sollen.
	//		4.	Nachdem alle Elemente "tmpRow" hinzugefügt wurden, füge "tmpRow" in "tmp" ein.
	//		5.	Nachdem alle Reihen durchlaufen wurden, setze "VariantSudoku" gleich "tmp".
	//			(Nun beinhaltet VariantSudoku das selbe sudoku wie davor, nur mit dem unterschied, dass alle Spalten reihen sind.)
	//		6.	Damit verifiziert werden kann welcher status momentan das UnsolvedSudoku besitzt, wird der bool reverse verwendet.
	//			false = VariantSudoku wurde in ein "spalten sudoku" transformiert
	//			true = VariantSudoku wurde in ein "reihen sudoku" transformiert.
	private static void TransformColumn(bool reverse) {
		List<List<List<int>>> tmp = [];
		for (int row = 0; row < SudokuSize; row++) {
			List<List<int>> tmpRow = [];
			for (int column = 0; column < SudokuSize; column++) {
				tmpRow.Add(VariantSudoku[column][row]);
			}
			tmp.Add(tmpRow);
		}
		VariantSudoku = tmp;
		CurrentTransformation = reverse ? Transformation.Row : Transformation.Column;
	}

	//	Transformiert jedes Quadrat des Sudokus in eine Reihe des Sudokus.
	//	Vorgehensweise:
	//		1.	Erstelle eine neue Liste "tmp", welches das veränderte Varianten-UnsolvedSudoku beinhalten soll.
	//		2.	Durchlaufe jede Quadrat (hier wird row als bezeichner verwendet) und erstelle eine temporäre liste "tmpRow",
	//			welche das Quadrat als Reihe darstellen soll.
	//		3.	Durchlaufe jede Spalte und füge das element, welches momentan besucht wird tmpRow hinzu.
	//		4.	Nachdem alle Reihen für eine bestimmte Spalte durchlaufen wurden, füge "tmpRow" in "tmp" ein.
	//		5.	Nachdem alle Spalten durchlaufen wurden, setze "VariantSudoku" gleich "tmp".
	//			(Nun beinhaltet VariantSudoku das selbe sudoku wie davor, nur mit dem unterschied, dass alle Quadrate reihen sind.)
	//		6.	Damit verifiziert werden kann welcher status momentan das UnsolvedSudoku besitzt, wird der bool reverse verwendet.
	//			false = VariantSudoku wurde in ein "quadrat sudoku" transformiert
	//			true = VariantSudoku wurde in ein "reihen sudoku" transformiert.
	//		7.	Damit das UnsolvedSudoku in seine Ursprungsform transformiert werden kann,
	//			wird lediglich in der berechnung von rowIndex/columnIndex row/column miteinander vertauscht.
	private static void TransformSquare(bool reverse) {
		List<List<List<int>>> tmp = [];
		for (int row = 0; row < SudokuSize; row++) {
			List<List<int>> tmpRow = [];
			for (int column = 0; column < SudokuSize; column++) {
				int square = (int) Math.Sqrt(SudokuSize);
				if (reverse) {
					int rowIndex = square * (int) (column / square) + (int) (row / square);
					int columnIndex = row % square + (column * square) % SudokuSize;
					tmpRow.Add(VariantSudoku[rowIndex][columnIndex]);
				}
				else {
					int rowIndex = square * (int) (row / square) + (int) (column / square);
					int columnIndex = column % square + (row * square) % SudokuSize;
					tmpRow.Add(VariantSudoku[columnIndex][rowIndex]);
				}
			}
			tmp.Add(tmpRow);
		}
		VariantSudoku = tmp;
		CurrentTransformation = reverse ? Transformation.Row : Transformation.Square;
	}
	#endregion

	#region EliminateFunctions
	//	Versuche Varianten von Zahlen aus dem UnsolvedSudoku auszuschliesen.
	//	Vorgehensweise:
	//		1.	Versuche in einer while-Schleife so oft wie möglich Varianten des Sudokus in "Reihen"/"Spalten"/"Quadrat"-Form zu löschen.
	//		2.	Wenn keine änderungen mehr möglich sind, wird die while-Schleife abgebrochen.
	private static bool Eliminate() {
		int counter = 0;
		while (true) {
			counter++;
			bool row = EliminateForState(Transformation.Row);
			bool column = EliminateForState(Transformation.Column);
			bool square = EliminateForState(Transformation.Square);
			if (!row && !column && !square) { break; }
		}
		return (counter > 1);
	}
	//	Löscht alle Varianten von Zahlen aus dem UnsolvedSudoku, die ausgeschlossen werden können.
	//	Vorgehensweise:
	//		1.	Transformiere das "Reihen"-UnsolvedSudoku in ein SudokuState-UnsolvedSudoku, je nach dem welcher transformation übergeben wird.
	//		2.	Der bool "isValid" gibt an, ob Varianten ausgeschlossen werden konnten.
	//		3.	Durchlaufe alle reihen vom Transformierten VariantSudoku und erstelle eine Liste von Zahlen, welche in dieser reihe ausgeschlossen werden können.
	//		4.	Durchlaufe alle reihen vom Transformierten VariantSudoku.
	//			4.1	Wenn das Momentane Feld nur eine Zahl besitzt kann diese übersprungen werden. (Diese Zahl muss in diesem Feld stehen)
	//			4.2 Andernfalls erstelle pro Feld in einer Liste die Schnittmenge der Liste "GameScreenSudokuField" und "eliminations".
	//				Diese repräsentiert alle Varianten, welche aus diesem Feld entfernt werden können.
	//			4.3	Falls die in 4.2 erstellte Liste mind. eine Zahl besitzt, bedeutet dies, dass das UnsolvedSudoku geändert werden kann.
	//			4.4 Entferne alle Zahlen aus dem Feld, welche in der Liste aus 4.2 vorhanden sind.
	//		5.	Transformiere das SudokuState-UnsolvedSudoku zurück in ein "Reihen"-UnsolvedSudoku.
	private static bool EliminateForState(Transformation state) {
		if (state == Transformation.Column) { TransformColumn(false); }
		else if (state == Transformation.Square) { TransformSquare(false); }
		bool changed = false;
		foreach (List<List<int>> rows in VariantSudoku) {
			List<int> eliminations = [];
			foreach (List<int> field in rows) {
				if (field.Count == 1) {
					eliminations.Add(field[0]);
				}
			}
			foreach (List<int> field in rows) {
				if (field.Count == 1) { continue; }
				List<int> matches = field.Intersect(eliminations).ToList();
				if (matches.Count > 0) { changed = true; }
				field.RemoveAll(x => matches.Contains(x));
			}
		}
		if (state == Transformation.Column) { TransformColumn(true); }
		else if (state == Transformation.Square) { TransformSquare(true); }
		return changed;
	}
	#endregion

	#region ExclusionFunction
	//	Entferne alle Varianten eines bestimmten Feldes außer eine Zahl i, bei dem die Zahl i ausschließlich in dem Feld vorhanden ist.
	//	Vorgehensweise:
	//		1.	Versuche in einer while-Schleife so oft wie möglich Varianten des Sudokus in "Reihen"/"Spalten"/"Quadrat"-Form zu löschen.
	//		2.	Wenn keine änderungen mehr möglich sind, wird die while-Schleife abgebrochen.
	private static bool Exclusion() {
		int counter = 0;
		while (true) {
			counter++;
			bool row = ExcludeForState(Transformation.Row);
			bool column = ExcludeForState(Transformation.Column);
			bool square = ExcludeForState(Transformation.Square);
			if (!row && !column && !square) { break; }
		}
		return (counter > 1);
	}
	//	Überprüfe ob es ein feld hat, indem eine Zahl i vorkommt,
	//	welche ausschließlich in diesem feld existiert, obwohl noch weitere Varianten in diesem Feld existieren.
	//	Dabei soll in diesem Feld nur noch die Zahl i vorkommen, alle anderen Varianten sind zu löschen.
	//	Vorgehensweise:
	//		1.	Transformiere das "Reihen"-UnsolvedSudoku in ein SudokuState-UnsolvedSudoku, je nach dem welcher transformation übergeben wird.
	//		2.	Der bool "isValid" gibt an, ob Varianten ausgeschlossen werden konnten.
	//		3.	Durchlaufe alle Reihen vom transformierten VariantSudoku.
	//		4.	Durchlaufe alle möglichen Zahlen (1-9, bei einem 9x9 sudoku).
	//			4.1	Der bool skip gibt an, ob die Zahl übersprungen werden kann, da es in einem Feld nur diese Variante gibt.
	//			4.2	Der int counter gibt an, wie viele Zahlen bereits durchlaufen wurden.
	//				Anfangs ist dieser 0, da bisher noch keine Zahl überprüft wurde.
	//			4.3	Wenn die momentane Variantenliste die Zahl enthält wird überprüft ob dies die einzige Variante ist, oder nicht.
	//				4.3.1	Falls diese die einzige ist, kann mit der nächsten zahl weitergemacht werden, da hier keine änderungen mehr möglich sind.
	//				4.3.2	Andernfalls wird counter um eins erhöht.
	//			4.4	Wenn skip = true ist, bedeutet dies, dass es ein Feld gibt, bei dem die Zahl i existiert ohne weitere Varianten.
	//				Wenn counter != 1 ist, bedeutet dies, dass mehrere Felder die Zahl i aufweisen und somit nichts verändert werden kann.
	//			4.5	Wenn skip = false ist und counter = 1 ist, bedeutet dies, dass es GENAU ein Feld gibt, bei in der die Zahl i und weitere Varianten in der Liste sind.
	//				Deshalb können für dieses Feld alle Varianten außer der Zahl i entfernt werden.
	//			4.6	Der bool isValid muss nun auf true gesetzt werden, da eine Änderung stattgefunden hat.
	//		5.	Transformiere das SudokuState-UnsolvedSudoku zurück in ein "Reihen"-UnsolvedSudoku.
	private static bool ExcludeForState(Transformation state) {
		if (state == Transformation.Column) { TransformColumn(false); }
		else if (state == Transformation.Square) { TransformSquare(false); }
		bool changed = false;
		foreach (List<List<int>> row in VariantSudoku) {
			for (int i = 1; i <= SudokuSize; i++) {
				bool skip = false;
				int counter = 0;
				foreach (List<int> variants in row) {
					if (variants.Contains(i)) {
						if (variants.Count == 1) {
							skip = true;
							break;
						}
						counter++;
					}
				}
				if (skip) { continue; }
				if (counter != 1) { continue; }
				foreach (List<int> variants in row) {
					if (variants.Contains(i)) {
						variants.RemoveAll(x => x != i);
						changed = true;
						break;
					}
				}
			}
		}
		if (state == Transformation.Column) { TransformColumn(true); }
		else if (state == Transformation.Square) { TransformSquare(true); }
		return changed;
	}
	#endregion

	#region GuessFunctions
	//	Errate eine bestimmte Zahl aus einem bestimmten feld erraten und diese als eine Neue erratene Zahl der GuessHistory hinzufügen.
	//	Vorgehensweise:
	//		1.	Speicher in einer Liste aus int-Tupeln alle Felder, in der die kleinste Anzahl von Varianten existeren.
	//		2.	Nimm ein beliebiges Feld aus der Liste und nimm eine der Varianten die "erraten" werden soll.
	//		3.	Speicher den stand des Sudokus vor dem erraten, und welche Zahl an welcher stelle erraten wird.
	//		4.	Setze die "erratene" Zahl in das jeweilige Feld.
	private static void Guess() {
		int min = MinVariantCount();
		List<(int, int)> minIndices = GetAllFieldsWithVariantCount(min);
		Random random = new();
		int index = random.Next(minIndices.Count());
		(int, int) field = minIndices[index];
		Dictionary<string, object> guess = [];
		int nextGuess = VariantSudoku[field.Item1][field.Item2][random.Next(VariantSudoku[field.Item1][field.Item2].Count())];
		guess.Add("currentSudoku", CopyVariant(VariantSudoku));
		guess.Add("row", field.Item1);
		guess.Add("column", field.Item2);
		guess.Add("GameScreenSudokuNumber", nextGuess);
		GuessHistory.Add(guess);
		VariantSudoku[field.Item1][field.Item2] = [nextGuess];
	}

	//	Erhalte die minimale Anzahl von Varianten aller Variantenlisten
	private static int MinVariantCount() {
		int min = VariantSudoku.Count();
		foreach (List<List<int>> rows in VariantSudoku) {
			foreach (List<int> field in rows) {
				if (field.Count() == 1) { continue; };
				if (field.Count() < min) { min = field.Count(); };
				// Kleinstmögliche anzahl der Varianten 2.
				if (min == 2) { return min; };
			}
		}
		return min;
	}

	//	Erhalte alle Felder, welche eine Anzahl von Varianten beinhalten.
	private static List<(int, int)> GetAllFieldsWithVariantCount(int count) {
		List<(int, int)> fields = [];
		for (int row = 0; row < VariantSudoku.Count(); row++) {
			List<List<int>> rows = VariantSudoku[row];
			for (int column = 0; column < rows.Count(); column++) {
				List<int> field = rows[column];
				if (field.Count() != count) { continue; };
				fields.Add((row, column));
			}
		}
		return fields;
	}

	//	Setze das UnsolvedSudoku auf den Zeitpunkt vor dem Raten zurück.
	//	Dies wird benötigt, wenn die erratene Zahl falsch ist.
	//	Hierbei kann die zuvor erratene Zahl auch als Variante entfernt werden, da dadurch das UnsolvedSudoku nicht lösbar ist.
	//	Da die erratenen Zahlen in einem Dictionary gespeichert werden, kann hier auf das letzte element zugegriffen werden und wiederhergestellt werden.
	private static void RepairLastGuess() {
		Dictionary<string, object> lastGuess = GuessHistory.Last();
		VariantSudoku = (List<List<List<int>>>) lastGuess["currentSudoku"];
		int row = (int) lastGuess["row"];
		int column = (int) lastGuess["column"];
		int number = (int) lastGuess["GameScreenSudokuNumber"];
		VariantSudoku[row][column].RemoveAll(x => x == number);
		GuessHistory.Remove(lastGuess);
	}
	#endregion

	#region HelperFunctions
	//	Kopiert die übergebene Liste und gibt die Kopie zurück.
	//	Wird beim erraten benötigt, da die Liste über Pointer geändert wird.
	private static List<List<List<int>>> CopyVariant(List<List<List<int>>> variant) {
		List<List<List<int>>> copy = [];
		for (int row = 0; row < variant.Count; row++) {
			List<List<int>> copyRow = [];
			for (int column = 0; column < variant.Count; column++) {
				copyRow.Add(new List<int>(variant[row][column]));
			}
			copy.Add(copyRow);
		}
		return copy;
	}
	public static List<List<int>> CopySudoku(List<List<int>> sudoku) {
		List<List<int>> copy = [];
		for (int row = 0; row < sudoku.Count; row++) {
			List<int> copyRow = [];
			for (int column = 0; column < sudoku.Count; column++) {
				copyRow.Add(sudoku[row][column]);
			}
			copy.Add(copyRow);
		}
		return copy;
	}
	#endregion

	#region DebuggFunctions
	//	Print funktion für Debugging Zwecke.
	private static void PrintVariantSudoku() {
		for (int row = 0; row < VariantSudoku.Count; row++) {
			if (row % (int) Math.Sqrt(SudokuSize) == 0) {
				Console.WriteLine("|-------|-------|-------|");
			}
			for (int column = 0; column < VariantSudoku.Count; column++) {
				if (column % (int) Math.Sqrt(SudokuSize) == 0) {
					Console.Write("| ");
				}
				if (VariantSudoku[row][column].Count > 1 || VariantSudoku[row][column].Count == 0) {
					Console.Write("0 ");
				}
				else {
					Console.Write(VariantSudoku[row][column][0] + " ");
				}
			}
			Console.WriteLine("|");
		}
		Console.WriteLine("|-------|-------|-------|");
	}
	#endregion
}