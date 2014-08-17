namespace SudokuSolver

type CellRequest = 
    { X : int
      Y : int
      mutable Value : string }

type SolutionRequest =
    { Data : CellRequest[][][][] }

type SolutionResponse =
    { Grid : CellRequest[]
      Result : bool } 