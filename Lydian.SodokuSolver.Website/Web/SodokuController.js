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

sodokuApp.controller("sodokuCtrl", [
    '$scope', '$http', function ($scope, $http) {
        var counter = [0, 1, 2];
        var foreach = function (fun) {
            return counter.map(fun);
        };

        var grid = foreach(function (verticalBand) {
            return foreach(function (horizontalBand) {
                return foreach(function (line) {
                    return foreach(function (cell) {
                        var y = verticalBand * 3 + line + 1;
                        var x = horizontalBand * 3 + cell + 1;
                        return new Cell(x, y, "");
                    });
                });
            });
        });

        $scope.grid = grid;
        $scope.solve = function (y) {
            $scope.name = "serializing...";
            var json = JSON.stringify(grid);
            $scope.name = "posting...";
            $http.post("/api/sodoku", json).error(function (x) {
                return $scope.name = "ERROR!";
            }).success(function (results) {
                results.forEach(function (cell) {
                    var verticalBand = Math.floor((cell.Y - 1) / 3) + 1;
                    var horizontalBand = Math.floor((cell.X - 1) / 3) + 1;
                    var line = ((cell.Y - 1) % 3) + 1;
                    var cellPos = ((cell.X - 1) % 3) + 1;
                    grid[verticalBand - 1][horizontalBand - 1][line - 1][cellPos - 1].Value = cell.Value;
                    $scope.name = cell.X;
                });
            });
        };
        $scope.name = "isaac";
    }]);
//# sourceMappingURL=SodokuController.js.map
