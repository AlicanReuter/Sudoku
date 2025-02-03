#region Imports
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace Shared.Configs.Core;
public static class SudokuCreation {
	#region SudokuTypes
	public enum Difficult {
		None,
		Easy,
		Normal,
		Hard,
		Expert
	}
	public enum Transformation {
		Row,
		Column,
		Square
	}
	#endregion

	#region SudokuVariables
	public static int SudokuSize { get; set; }
	public static int SudokuSquareSize { get; set; }
	public static List<List<int>> OriginalSudoku { get; set; }
	public static List<List<int>> UnsolvedSudoku { get; set; }
	public static List<List<int>> SolvedSudoku { get; set; }
	public static List<List<List<int>>> VariantSudoku { get; set; }
	public static Difficult Difficulty { get; set; }
	public static Transformation CurrentTransformation { get; set; }
	public static List<Dictionary<string, object>> GuessHistory { get; set; }
	public static List<int> PlaceHistory { get; set; } = [];
	#endregion

	#region SudokuHelperFunctions
	public static void Unload() {
		SudokuSize = SudokuSquareSize = default;
		OriginalSudoku = UnsolvedSudoku = SolvedSudoku = null;
		VariantSudoku = null;
		Difficulty = default;
		CurrentTransformation = default;
		GuessHistory = default;
		PlaceHistory = [];
	}
	public static void CreateEmptySudoku() {
		List<List<int>> sudoku = [];
		for (int row = 0; row < SudokuSize; row++) {
			List<int> newRow = [];
			for (int column = 0; column < SudokuSize; column++) {
				newRow.Add(0);
			}
			sudoku.Add(newRow);
		}
		UnsolvedSudoku = sudoku;
	}
	public static List<int> GetSudokuNumbers() {
		List<int> numbers = [];
		for (int number = 1; number <= SudokuSize; number++) {
			numbers.Add(number);
		}
		return numbers;
	}
	public static void CreateVariantSudoku() {
		List<List<List<int>>> variantSudoku = [];
		for (int row = 0; row < SudokuSize; row++) {
			List<List<int>> nextRow = [];
			for (int column = 0; column < SudokuSize; column++) {
				nextRow.Add(GetSudokuNumbers());
			}
			variantSudoku.Add(nextRow);
		}
		VariantSudoku = variantSudoku;
	}
	public static void CreateSolvedSudoku() {
		List<List<int>> solvedSudoku = [];
		for (int row = 0; row < SudokuSize; row++) {
			List<int> nextRow = [];
			for (int column = 0; column < SudokuSize; column++) {
				nextRow.Add(VariantSudoku[row][column].First());
			}
			solvedSudoku.Add(nextRow);
		}
		SolvedSudoku = solvedSudoku;
	}
	public static void CreateUnsolvedSudoku() {
		List<List<int>> sudoku = [];
		for (int row = 0; row < SolvedSudoku.Count; row++) {
			List<int> sudokuRow = [];
			for (int column = 0; column < SolvedSudoku.Count; column++) {
				sudokuRow.Add(SolvedSudoku[row][column]);
			}
			sudoku.Add(sudokuRow);
		}
		UnsolvedSudoku = sudoku;
	}
	public static void LoadVariantSudoku(object variantSudoku) {
		VariantSudoku = ((JArray) variantSudoku).ToObject<List<List<List<int>>>>();
	}
	public static void LoadOriginalSudoku(object originalSudoku) {
		OriginalSudoku = ((JArray) originalSudoku).ToObject<List<List<int>>>();
	}
	public static void LoadUnsolvedSudoku(object unsolvedSudoku) {
		UnsolvedSudoku = ((JArray) unsolvedSudoku).ToObject<List<List<int>>>();
	}
	public static void LoadSolvedSudoku(object solvedSudoku) {
		SolvedSudoku = ((JArray) solvedSudoku).ToObject<List<List<int>>>();
	}
	#endregion

	#region DebugFunctions
	public static void Print(List<List<int>> sudoku) {
		for (int row = 0; row < SudokuSize; row++) {
			if (row % SudokuSquareSize == 0) {
				Console.WriteLine("|-------|-------|-------|");
			}
			for (int column = 0; column < SudokuSize; column++) {
				if (column % SudokuSquareSize == 0) {
					Console.Write("| ");
				}
				Console.Write(sudoku[row][column] + " ");
			}
			Console.WriteLine("|");
		}
		Console.WriteLine("|-------|-------|-------|");
	}
	#endregion
}