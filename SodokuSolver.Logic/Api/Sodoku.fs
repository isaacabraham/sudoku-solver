namespace SodokuSolver.Website.Api

open Lydian.SodokuSolver.Solver
open System.Web.Http
open System
open Newtonsoft.Json

type ResultCell =
    { Value : Nullable<int>
      X : int
      Y : int }

type SolveRequest =
    { Data : ResultCell[][][][] }

type SodokuController() =
    inherit ApiController()
    member __.Post(request:SolveRequest) =
        let cells = request.Data
                    |> Seq.collect(fun c -> c |> Seq.collect(fun d -> d |> Seq.collect(fun e -> e |> Seq.map id)))
                    |> Seq.mapi(fun i (cell:ResultCell) -> convertToCell(i, cell.X, cell.Y, cell.Value))
                    |> Seq.toArray

        let result = SolvePuzzleFromCells cells
                     |> Seq.map(fun cell ->
                        let value = match cell.Value with
                                    | Some value -> Nullable(value)
                                    | None -> Nullable()
                        { ResultCell.Value = value; X = cell.X; Y = cell.Y })
                     |> Seq.toArray
        result
