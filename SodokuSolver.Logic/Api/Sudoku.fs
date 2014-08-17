module SudokuSolver.Soduku

open Nessos.Streams.Core

type Cell = 
    { X : int
      Y : int
      mutable Value : int option }

/// tests whether a suggestion for a cell is valid
let validateSuggestion targetCell grid suggestion = 
    let getBlock cell = (((cell.X - 1) / 3) + 1) + (((cell.Y - 1) / 3) * 3)

    let isConnectedTo destination other =
        if destination = other then false
        else
            other.X = destination.X ||
            other.Y = destination.Y ||
            (getBlock other = getBlock destination)

    grid
    |> Stream.ofSeq
    |> Stream.filter (isConnectedTo targetCell)
    |> Stream.map (fun cell -> cell.Value)
    |> Stream.toArray
    |> Seq.choose id
    |> Seq.forall ((<>) suggestion)

/// solves the puzzle starting at a given cell
let rec private solveRec (cell, index) (grid : Cell array) = 
    let checkNextCell() = 
        let isLastCell = (cell.X = 9 && cell.Y = 9)
        if isLastCell then true
        else grid |> solveRec (grid.[index + 1], index + 1)
    match cell.Value with
    | Some _ -> checkNextCell()
    | None -> 
        seq { 1..9 }
        |> Stream.ofSeq
        |> Stream.filter (validateSuggestion cell grid)
        |> Stream.fold (fun passed suggestion -> 
               if passed then true
               else 
                   cell.Value <- Some suggestion
                   let nextNodeWasSuccessful = checkNextCell()
                   if not nextNodeWasSuccessful then cell.Value <- None
                   nextNodeWasSuccessful) false

let private validate (grid:Cell array) =          
    let isValid = grid
                  |> Seq.choose(fun cell ->
                      match cell.Value with
                      | Some value -> Some (cell, value)
                      | None -> None)
                  |> Seq.forall (fun (cell, value) -> validateSuggestion cell grid value)
    if isValid then Some grid else None

/// solves the puzzle
let Solve (grid:Cell array) = 
    grid
    |> validate
    |> Option.map (solveRec (grid.[0], 0))
    |> function
       | None -> None, false
       | Some result -> Some grid, result