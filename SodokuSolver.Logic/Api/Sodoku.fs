namespace SodokuSolver.Api

open SodokuSolver.Logic.Solver
open System.Web.Http
open System
open Nessos.Streams.Core

type CellRequest = 
    { X : int
      Y : int
      mutable Value : string }

type SolveResponse =
    { Grid : CellRequest[]
      Result : bool } 

type SolveRequest =
    { Data : CellRequest[][][][] }

type SodokuController() =
    inherit ApiController()

    let toCell request =
        { Cell.X = request.X
          Y = request.Y
          Value = match request.Value |> Int32.TryParse with
                  | true, value -> Some value
                  | false, _ -> None }

    let toRequest (cell:Cell) =
        { X = cell.X
          Y = cell.Y
          Value = match cell.Value with
                  | Some value -> value.ToString()
                  | None -> String.Empty }

    member __.Post(request:SolveRequest) =
        request.Data
        |> ParStream.ofSeq
        |> ParStream.collect(fun c -> c |> Stream.ofArray |> Stream.collect(fun d -> d |> Stream.ofArray |> Stream.collect(fun e -> e |> Stream.ofArray |> Stream.map id)))
        |> ParStream.map toCell
        |> ParStream.toArray
        |> Solve
        |> fun (grid, succeeded) ->
            { Grid = grid
                      |> Option.map (ParStream.ofSeq
                                     >> ParStream.map toRequest
                                     >> ParStream.toArray)
                      |> function | Some solution -> solution | None -> Array.empty
              Result = succeeded }