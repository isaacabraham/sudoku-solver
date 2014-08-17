/// <reference path="../scripts/typings/angularjs/angular.d.ts" />
var sodokuApp = angular.module('sodokuApp', []);

sodokuApp.directive('numbersOnly', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, modelCtrl) {
            modelCtrl.$parsers.push(function (inputValue) {
                if (inputValue == undefined)
                    return '';
                var transformedInput = inputValue.replace(/[^0-9]/g, '');
                if (transformedInput != inputValue) {
                    modelCtrl.$setViewValue(transformedInput);
                    modelCtrl.$render();
                }

                return transformedInput;
            });
        }
    };
});

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
        this.data = data;
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
                        return new Cell(x, y, '');
                    });
                });
            });
        }));
        $scope.grid = grid;
        $scope.solve = function () {
            $scope.processing = true;
            $scope.status = "Solving...";
            $http.post("/api/sodoku", JSON.stringify(grid)).error(function (x) {
                $scope.processing = false;
                $scope.status = "Error!";
            }).success(function (solution) {
                $scope.processing = false;
                if (solution.Result) {
                    $scope.status = "Success!";
                    solution.Grid.forEach(function (cell) {
                        var verticalBand = Math.floor((cell.Y - 1) / 3) + 1;
                        var horizontalBand = Math.floor((cell.X - 1) / 3) + 1;
                        var line = ((cell.Y - 1) % 3) + 1;
                        var cellPos = ((cell.X - 1) % 3) + 1;
                        grid.data[verticalBand - 1][horizontalBand - 1][line - 1][cellPos - 1].Value = cell.Value;
                    });
                } else
                    $scope.status = "Failed to solve this puzzle :(";
            });
        };

        $scope.clear = function () {
            grid.data.forEach(function (verticalBand, a, b) {
                return verticalBand.forEach(function (horizontalBand, c, d) {
                    return horizontalBand.forEach(function (line, e, f) {
                        return line.forEach(function (cell, g, h) {
                            return cell.Value = "";
                        });
                    });
                });
            });
            $scope.status = "Enter puzzle.";
        };

        $scope.clear();
    }]);
