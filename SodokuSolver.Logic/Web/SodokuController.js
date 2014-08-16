/// <reference path="../scripts/typings/angularjs/angular.d.ts" />
var x = 1;

var sodokuApp = angular.module('sodokuApp', []);

var Cell = (function () {
    function Cell(x, y, value) {
        this.Value = value;
        this.X = x;
        this.Y = y;
    }
    return Cell;
})();

var Grid = (function () {
    function Grid(data) {
        this.Data = data;
    }
    return Grid;
})();

sodokuApp.controller("sodokuCtrl", [
    '$scope', '$http', function ($scope, $http) {
        var counter = [0, 1, 2];
        var grid = new Grid(counter.map(function (verticalBand) {
            return counter.map(function (horizontalBand) {
                return counter.map(function (line) {
                    return counter.map(function (cell) {
                        var y = verticalBand * 3 + line + 1;
                        var x = horizontalBand * 3 + cell + 1;
                        return new Cell(x, y, "");
                    });
                });
            });
        }));

        $scope.grid = grid;
        $scope.solve = function (_) {
            $scope.name = "serializing...";
            $scope.name = "posting...";
            $http.post("/api/sodoku", JSON.stringify(grid)).error(function (x) {
                return $scope.name = "ERROR!";
            }).success(function (results) {
                $scope.name = "SUCCESS!";
                results.forEach(function (cell) {
                    var verticalBand = Math.floor((cell.Y - 1) / 3) + 1;
                    var horizontalBand = Math.floor((cell.X - 1) / 3) + 1;
                    var line = ((cell.Y - 1) % 3) + 1;
                    var cellPos = ((cell.X - 1) % 3) + 1;
                    grid.Data[verticalBand - 1][horizontalBand - 1][line - 1][cellPos - 1].Value = cell.Value;
                });
            });
        };

        $scope.clear = function (_) {
            grid.Data.forEach(function (verticalBand, a, b) {
                return verticalBand.forEach(function (horizontalBand, c, d) {
                    return horizontalBand.forEach(function (line, e, f) {
                        return line.forEach(function (cell, g, h) {
                            return cell.Value = "";
                        });
                    });
                });
            });
        };

        $scope.name = "Enter puzzle.";
    }]);
