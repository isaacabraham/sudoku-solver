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
        $scope.solve = function (x) {
            return $http.get("/api/sodoku").success(function (x) {
                return alert(x);
            });
        };
        $scope.name = "isaac";
    }]);
//# sourceMappingURL=SodokuController.js.map
