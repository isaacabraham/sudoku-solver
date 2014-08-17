#I @"..\packages"
#r @"Streams.0.1.5\lib\Streams.Core.dll"
#load @"Deedle.1.0.2\Deedle.fsx"
#load @"Api\Solver.fs"

open SudokuSolver.Soduku
open Deedle

// Create an example grid
let grid = query { for x in { 1 .. 9 } do
                   for y in { 1 .. 9 } do
                   select { X = x; Y = y; Value = None } }
           |> Seq.toArray

// Set some values
grid.[0].Value <- Some 5

// Solve it
let solution, succeeded = Solve grid

// Put it into a data frame!
solution
|> Option.map Frame.ofRecords