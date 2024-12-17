#region Imports
using System;
using System.Collections.Generic;
using System.Linq;
using static Shared.Configs.Core.SudokuCreation;
#endregion
namespace Core.SudokuCreation;
internal class Sudoku {
	internal Sudoku(int size, string difficulty) {
		SudokuSize = size * size;
		SudokuSquareSize = size;
		Difficulty = (Difficult) Enum.Parse(typeof(Difficult), difficulty);
		InitializeSudoku();
	}
	private void InitializeSudoku() {
		CreateEmptySudoku();
		this.SolveSudoku();
		SetDifficulty();
	}
	#region DifficultFunctions
	private void SetDifficulty() {
		switch (Difficulty) {
			case Difficult.Easy:
				RemoveNumbers(12);
				break;
			case Difficult.Normal:
				RemoveNumbers(29);
				break;
			case Difficult.Hard:
				RemoveNumbers(47);
				break;
			case Difficult.Expert:
				RemoveNumbers(64);
				break;
			default:
				RemoveNumbers(0);
				break;
		}
	}

	private void RemoveNumbers(int count) {
		Random rng = new();
		List<int> availableButtonIndexes = [];
		int lastIndex = SudokuSize * SudokuSize;
		int currentButtonIndex, nextRNG, row, col;
		for (int i = 0; i < lastIndex; i++) { availableButtonIndexes.Add(i); }
		availableButtonIndexes = [.. availableButtonIndexes.OrderBy(x => rng.Next())];
		for (int i = 0; i < count; i++) {
			nextRNG = rng.Next(0, availableButtonIndexes.Count);
			currentButtonIndex = availableButtonIndexes[nextRNG];
			availableButtonIndexes.RemoveAt(nextRNG);
			row = currentButtonIndex / SudokuSize;
			col = currentButtonIndex % SudokuSize;
			UnsolvedSudoku[row][col] = 0;
			i += RemoveUnnecessaryFields(ref availableButtonIndexes, currentButtonIndex);
		}
	}

	private int RemoveUnnecessaryFields(ref List<int> availableButtonIndexes, int currentButtonIndex) {
		int rowStart, rowCurrent, rowEnd, colStart, colCurrent, colEnd, emptyFieldCount, removableIndex, removeCount;
		rowCurrent = currentButtonIndex / SudokuSize;
		colCurrent = currentButtonIndex % SudokuSize;
		rowStart = GetStartIndex(rowCurrent);
		rowEnd = rowStart + SudokuSquareSize;
		colStart = GetStartIndex(colCurrent);
		colEnd = colStart + SudokuSquareSize;
		emptyFieldCount = 0;
		for (int row = rowStart; row < rowEnd; row++) {
			for (int col = colStart; col < colEnd; col++) {
				if (UnsolvedSudoku[row][col] == 0) { emptyFieldCount++; }
			}
		}
		if (emptyFieldCount <= SudokuSize - SudokuSquareSize) { return 0; }
		removeCount = 0;
		for (int row = rowStart; row < rowEnd; row++) {
			for (int col = colStart; col < colEnd; col++) {
				removableIndex = (row * SudokuSize) + col;
				if (!availableButtonIndexes.Contains(removableIndex)) { continue; }
				availableButtonIndexes.Remove(removableIndex);
				removeCount++;
			}
		}
		return removeCount;
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
	#endregion

	#region DebugFunctions
	internal void PrintSudoku() {
		Print(UnsolvedSudoku);
		Print(SolvedSudoku);
	}
	#endregion
}
