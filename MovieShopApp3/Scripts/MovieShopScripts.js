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

var app = angular.module("myApp", []);

app.controller("myCtrl", function ($scope) {
    $scope.text = "Success";
});