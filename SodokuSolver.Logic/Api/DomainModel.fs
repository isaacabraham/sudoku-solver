module SudokuSolver.DomainModel

open SudokuSolver.Soduku
open System

let toCell (request : CellRequest) = 
    { Cell.X = request.X
      Y = request.Y
      Value = 
          match request.Value |> Int32.TryParse with
          | true, value when value < 10 && value > 0 -> Some value
          | _ -> None }
    
let toRequest (cell : Cell) = 
    { CellRequest.X = cell.X
      Y = cell.Y
      Value = 
          match cell.Value with
          | Some value -> value.ToString()
          | None -> String.Empty }