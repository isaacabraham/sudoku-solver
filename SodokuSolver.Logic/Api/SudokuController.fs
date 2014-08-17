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
                   |> ParStream.ofSeq
                   |> ParStream.collect (Seq.collect (Seq.collect id) >> Stream.ofSeq)
                   |> ParStream.map toCell
                   |> ParStream.toArray
                   |> Solve
                   |> fun (grid, succeeded) -> 
                       { Grid = 
                             grid
                             |> Option.map (ParStream.ofSeq
                                            >> ParStream.map toRequest
                                            >> ParStream.toArray)
                             |> function 
                             | Some solution -> solution
                             | None -> Array.empty
                         Result = succeeded }
        } |> Async.StartAsTask