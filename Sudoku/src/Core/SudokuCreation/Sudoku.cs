#region Imports
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using static Shared.Configs.Core.SudokuCreation;
#endregion
namespace Core.SudokuCreation;
internal class Sudoku {
	internal Sudoku(int size, string difficulty, Dictionary<string, object> loadedSudoku = default) {
		SudokuSize = size * size;
		SudokuSquareSize = size;
		Difficulty = (Difficult) Enum.Parse(typeof(Difficult), difficulty);
		InitializeSudoku(loadedSudoku, true);
	}
	internal Sudoku(string jsonString) {
		Dictionary<string, object> json = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);
		int size = Int32.Parse(json["square_size"].ToString());
		SudokuSize = size * size;
		SudokuSquareSize = size;
		string diff = json["difficult"].ToString();
		Enum.TryParse(diff, out Difficult difficulty);
		Difficulty = difficulty;
		InitializeSudoku(json, false);
	}
	private void InitializeSudoku(Dictionary<string, object> loadedSudoku, bool isNew) {
		if (isNew) {
			CreateEmptySudoku();
			this.SolveSudoku();
			CreateUnsolvedSudoku();
			SetDifficulty();
			OriginalSudoku = SudokuSolver.CopySudoku(UnsolvedSudoku);
			VariantSudoku = default;
		}
		else {
			LoadOriginalSudoku(loadedSudoku["original"]);
			LoadUnsolvedSudoku(loadedSudoku["unsolved"]);
			LoadSolvedSudoku(loadedSudoku["solved"]);
			LoadVariantSudoku(loadedSudoku["variants"]);
		}
	}
	#region DifficultFunctions
	private void SetDifficulty() {
		switch (Difficulty) {
			case Difficult.Easy:
				RemoveNumbers(3);
				break;
			case Difficult.Normal:
				RemoveNumbers(4);
				break;
			case Difficult.Hard:
				RemoveNumbers(5);
				break;
			case Difficult.Expert:
				RemoveNumbers(7);
				break;
			default:
				RemoveNumbers(0);
				break;
		}
	}

	private void RemoveNumbers(int removableNumberCount) {
		Random rng = new();
		for (int squareIndex = 0; squareIndex < SudokuSize; squareIndex++) {
			DeleteRandomNumber(rng, GetFieldIndizesInSquare(squareIndex), removableNumberCount);
		}
	}

	private List<(int, int)> GetFieldIndizesInSquare(int squareIndex) {
		List<(int, int)> fields = [];
		var startIndex = GetStartIndizes(squareIndex);
		for (int row = startIndex.Item1; row < startIndex.Item1 + SudokuSquareSize; row++) {
			for (int column = startIndex.Item2; column < startIndex.Item2 + SudokuSquareSize; column++) {
				fields.Add((row, column));
			}
		}
		return fields;
	}

	private void DeleteRandomNumber(Random rng, List<(int, int)> fieldIndizes, int removableNumberCount) {
		fieldIndizes = [.. fieldIndizes.OrderBy(x => rng.Next())];
		for (int i = 0; i < removableNumberCount; i++) {
			var rndFieldIndex = fieldIndizes[rng.Next(0, fieldIndizes.Count)];
			UnsolvedSudoku[rndFieldIndex.Item1][rndFieldIndex.Item2] = 0;
			fieldIndizes.Remove(rndFieldIndex);
		}
	}
	#endregion

	#region PlaceFunctions
	//	Platziere eine Number in einem bestimmten Feld.
	internal void PlaceNumber(int number, int row, int column) { UnsolvedSudoku[row][column] = number; }

	//	Lösche eine Nummer in einem bestimmten Feld.
	internal void DeleteNumber(int row, int column) { UnsolvedSudoku[row][column] = 0; }
	#endregion

	#region VerifyFunctions
	//	Prüfe ob eine zu setzende Zahl platziert werden darf.
	internal bool IsPlaced(int numberToPlace, int row, int column) {
		if (ContainsNumber(GetNumbersOf(Transformation.Row, row), numberToPlace)) { return true; }
		if (ContainsNumber(GetNumbersOf(Transformation.Column, column), numberToPlace)) { return true; }
		if (ContainsNumber(GetNumbersOf(Transformation.Square, row, column), numberToPlace)) { return true; }
		return false;
	}

	//	Prüfe ob die übergebene Liste die übergebene Zahl beinhaltet-
	private bool ContainsNumber(List<int> line, int numberToCheck) {
		foreach (int number in line) {
			if (number == numberToCheck) { return true; }
		}
		return false;
	}

	//	Erhalte eine Liste des Typs Row, Column, Square
	private List<int> GetNumbersOf(Transformation transformation, int index1, int index2 = 0) {
		List<int> numbers = [];
		if (transformation == Transformation.Row) {
			numbers = UnsolvedSudoku[index1];
		}
		else if (transformation == Transformation.Column) {
			for (int i = 0; i < SudokuSize; i++) {
				numbers.Add(UnsolvedSudoku[i][index1]);
			}
		}
		else if (transformation == Transformation.Square) {
			int rowStart = GetStartIndex(index1);
			int rowEnd = rowStart + SudokuSquareSize;
			int columnStart = GetStartIndex(index2);
			int columnEnd = columnStart + SudokuSquareSize;
			for (int currentRow = rowStart; currentRow < rowEnd; currentRow++) {
				for (int currentColumn = columnStart; currentColumn < columnEnd; currentColumn++) {
					numbers.Add(UnsolvedSudoku[currentRow][currentColumn]);
				}
			}
		}
		return numbers;
	}

	//	Erhalte den Startindex des Squares, aus dem übergebenen Indexes.
	//	Wenn der Startindex des feldes (x, y) = (8, 5) berechnet werden soll, muss für 8 bzw 5 die Funktion aufgerufen werden.
	//	Somit erhält man für 8 den Index 6 und für 5 den Index 3
	private int GetStartIndex(int index) { return SudokuSquareSize * (index / SudokuSquareSize); }
	private (int, int) GetStartIndizes(int squareIndex) {
		(int row, int column) startIndex;
		startIndex.row = (squareIndex * SudokuSquareSize) % SudokuSize;
		startIndex.column = (squareIndex / SudokuSquareSize) * SudokuSquareSize;
		return startIndex;
	}
	#endregion

	#region DebugFunctions
	internal void PrintSudoku() {
		Print(UnsolvedSudoku);
		Print(SolvedSudoku);
	}
	#endregion
}
