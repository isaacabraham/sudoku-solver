/// <reference path="../scripts/typings/angularjs/angular.d.ts" />
var x = 1

var sodokuApp = angular.module('sodokuApp', []);

class Cell {
    Value: string;
    X: number;
    Y: number;
    constructor(x: number, y: number, value: string) {
        this.Value = value;
        this.X = x;
        this.Y = y;
    }
}

class Grid {
    Data: Cell[][][][];
    constructor(data: Cell[][][][]) {
        this.Data = data;
    }
}


sodokuApp.controller("sodokuCtrl", ['$scope', '$http', ($scope, $http: ng.IHttpService) => {
    var counter = [0, 1, 2];
    var grid = new Grid(counter.map(verticalBand =>
        counter.map(horizontalBand =>
            counter.map(line =>
                counter.map(cell => {
                    var y = verticalBand * 3 + line + 1;
                    var x = horizontalBand * 3 + cell + 1;
                    return new Cell(x, y, "");
                })))));

    $scope.grid = grid;
    $scope.solve = _ => {
        $scope.name = "serializing...";
        $scope.name = "posting...";
        $http.post("/api/sodoku", JSON.stringify(grid))
            .error(x => $scope.name = "ERROR!")
            .success((results: Cell[]) => {
                $scope.name = "SUCCESS!"
                results.forEach(cell => {
                    var verticalBand = Math.floor((cell.Y - 1) / 3) + 1
                    var horizontalBand = Math.floor((cell.X - 1) / 3) + 1
                    var line = ((cell.Y - 1) % 3) + 1
                    var cellPos = ((cell.X - 1) % 3) + 1
                    grid.Data[verticalBand - 1][horizontalBand - 1][line - 1][cellPos - 1].Value = cell.Value;
                });
            });
    }

    $scope.clear = _ => {
        grid.Data.forEach((verticalBand, a, b) =>
            verticalBand.forEach((horizontalBand, c, d) =>
                horizontalBand.forEach((line, e, f) =>
                    line.forEach((cell, g, h) =>
                        cell.Value = ""))))
        };

    $scope.name = "Enter puzzle.";
}]);