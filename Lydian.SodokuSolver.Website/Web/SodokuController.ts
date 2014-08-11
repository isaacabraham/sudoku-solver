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
    $scope.solve = x =>
        $http.get("/api/sodoku")
             .success(x => alert(x));
    $scope.name = "isaac";
}]);