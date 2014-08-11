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


sodokuApp.controller("sodokuCtrl", ['$scope', '$http', ($scope, $http:ng.IHttpService) => {
    var counter = [0, 1, 2];
    var foreach = fun => counter.map(fun);

    var grid = foreach(verticalBand =>
                    foreach(horizontalBand =>
                        foreach(line =>
                            foreach(cell => {
                                var y = verticalBand * 3 + line + 1;
                                var x = horizontalBand * 3 + cell + 1;
                                return new Cell(x, y, "");
                            }))));
    
    $scope.grid = grid;
    $scope.solve = y => {
        $scope.name = "serializing...";
        var json = JSON.stringify(grid)
        $scope.name = "posting...";
        $http.post("/api/sodoku", json)
            .error(x => $scope.name = "ERROR!")
            .success((results: Cell[]) => {
                results.forEach(cell => {
                    var verticalBand = Math.floor((cell.Y - 1) / 3) + 1
                    var horizontalBand = Math.floor((cell.X - 1) / 3) + 1
                    var line = ((cell.Y - 1) % 3) + 1
                    var cellPos = ((cell.X - 1) % 3) + 1
                    grid[verticalBand - 1][horizontalBand - 1][line - 1][cellPos - 1].Value = cell.Value;
                    $scope.name = cell.X;
                });
            });
    }
    $scope.name = "isaac";
}]);