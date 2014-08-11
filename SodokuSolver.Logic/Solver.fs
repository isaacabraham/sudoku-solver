module Lydian.SodokuSolver.Solver

open System

module private Seq = 
    /// Evaluate a predicate over every item. If the predicate passes, return Some x, otherwise return None.
    let toOptional predicate = 
        Seq.map (fun item -> 
            if predicate item then Some item
            else None)

type Cell = 
    { Index : int
      X : int
      Y : int
      Block : int
      IsLastCell : bool
      mutable Value : int option }

let private toCell index (x, y, value)=
    let getBlock x y = (((x - 1) / 3) + 1) + (((y - 1) / 3) * 3)
    {  Index = index
       X = x
       Y = y
       Value = value
       Block = getBlock x y
       IsLastCell = (x = 9 && y = 9) }

let convertToCell(index, x, y, value:Nullable<_>) =
    let value = if value.HasValue then Some value.Value else None
    toCell index (x, y, value)

// converts a simple array of numbers into a sudoku grid
let private toGrid sourceData = 
    sourceData
    |> Seq.toOptional ((<>) 0)
    |> Seq.mapi (fun index cell -> 
           let div, rem = Math.DivRem(index, 9)
           rem + 1, div + 1, cell)
    |> Seq.mapi toCell
    |> Seq.toArray

/// tests whether a suggestion for a cell is valid
let private validateSuggestion cell grid suggestion = 
    let isConnected = (fun otherCell -> otherCell.X = cell.X || otherCell.Y = cell.Y || otherCell.Block = cell.Block)
    grid
    |> Seq.filter isConnected
    |> Seq.map (fun cell -> cell.Value)
    |> Seq.choose id
    |> Seq.forall ((<>) suggestion)

/// solves the puzzle starting at a given cell
let rec private solveRec cell (grid : array<Cell>) = 
    let checkNextCell() = 
        if cell.IsLastCell then true
        else grid |> solveRec (grid.[cell.Index + 1])
    match cell.Value with
    | Some _ -> checkNextCell()
    | None -> 
        seq { 1..9 }
        |> Seq.filter (validateSuggestion cell grid)
        |> Seq.fold (fun passed suggestion -> 
               if passed then true
               else 
                   cell.Value <- Some suggestion
                   let nextNodeWasSuccessful = checkNextCell()
                   if not nextNodeWasSuccessful then cell.Value <- None
                   nextNodeWasSuccessful) false

let private toData gridData = 
    gridData
    |> Seq.groupBy (fun cell -> cell.Y)
    |> Seq.map (fun (key, row) -> 
           row
           |> Seq.sortBy (fun cell -> cell.Y, cell.X)
           |> Seq.map (fun cell -> cell.Value.Value)
           |> Seq.toArray)
    |> Seq.toArray

/// solves the grid
let private solve (grid : array<Cell>) = 
    solveRec grid.[0] grid |> ignore
    grid

// parsing input
type Command = 
    | Number of value : int
    | SetOfBlanks of count : int

let parse data = 
    let isNumber, value = Int32.TryParse data
    if isNumber then Some(Number value)
    elif data.StartsWith("0x") then Some(SetOfBlanks(Int32.Parse(data.Substring 2)))
    else None

let SolvePuzzle(input : string) = 
    let items = 
        input.Split(',')
        |> Seq.collect (fun x -> 
               match parse x with
               | Some(Number value) -> [ value ]
               | Some(SetOfBlanks count) -> 
                   [ for i in 1..count -> 0 ]
               | None -> failwith "at least some items in the list are not valid")
        |> Seq.toArray
    match items.Length with
    | 81 -> 
        items
        |> toGrid
        |> solve
        |> toData
    | len -> failwith (sprintf "expected 81 items, but got %d" len)

let SolvePuzzleFromCells cells = cells |> solve