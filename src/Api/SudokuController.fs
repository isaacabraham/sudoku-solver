namespace SudokuSolver.Api

open Nessos.Streams.Core
open SudokuSolver
open SudokuSolver.DomainModel
open SudokuSolver.Soduku
open System.Web.Http

type SudokuController() = 
    inherit ApiController()
    member __.Post(request : SolutionRequest) = 
        async {
            return request.Data
                   |> Stream.ofArray
                   |> Stream.collect (Seq.collect (Seq.collect id) >> Stream.ofSeq)
                   |> Stream.map toCell
                   |> Stream.toArray
                   |> Solve
                   |> fun (grid, succeeded) -> 
                       { Grid = 
                             grid
                             |> Option.map (Stream.ofSeq >> Stream.map toRequest >> Stream.toArray)
                             |> function 
                             | Some solution -> solution
                             | None -> Array.empty
                         Result = succeeded }
        } |> Async.StartAsTask