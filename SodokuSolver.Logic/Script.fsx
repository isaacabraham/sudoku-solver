#I @"..\packages"
#r @"Streams.0.1.5\lib\Streams.Core.dll"
#load @"Deedle.1.0.2\Deedle.fsx"
#load "Solver.fs"

open SodokuSolver.Logic.Solver
open Deedle

let grid = query { for x in { 1 .. 9 } do
                   for y in { 1 .. 9 } do
                   select { X = x; Y = y; Value = None } }
           |> Seq.toArray

grid.[0].Value <- Some 5

let solution, succeeded = Solve grid

solution
|> Option.map Frame.ofRecords