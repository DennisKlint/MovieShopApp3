$(document).ready(function () {
    $("#ProductName").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/TEST/Index",
                type: "POST",
                dataType: "json",
                data: { Prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {

                        return { label: item.ProductName, value: item.ProductName };
                    }))
                }
            })
        },
        messages: {
            noResults: function () { }, results: function () { }
        }
    });
});
sadfs



angular.module('tabsDemoDynamicHeight', ['ngMaterial']);


var app = angular.module("myApp", []);
app.controller("myCtrl", function ($scope) {
    $scope.text = "Success";

});

angular.module('tabApp', [])
  .controller('TabController', ['$scope', function ($scope) {
      $scope.tab = 1;

      $scope.setTab = function (newTab) {
          $scope.tab = newTab;
      };

      $scope.isSet = function (tabNum) {
          return $scope.tab === tabNum;
      };
  }]);