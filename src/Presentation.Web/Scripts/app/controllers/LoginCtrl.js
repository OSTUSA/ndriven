﻿'use strict';

app.controller("LoginCtrl", ["$rootScope", "$scope", "$http", "$location", "LoginSvc", function ($rootScope, $scope, $http, $location, loginSvc) {

    // POST: login as user
    $scope.login = function () {
        var loginRsp = loginSvc.login.post({
            Email: $scope.Email,
            Password: $scope.Password,
            Authorization: "Basic" + btoa($scope.Email + ':' + $scope.Password)
        },
        function () { // success login
            $rootScope.User = loginRsp;
            $location.path('/dashboard');
        },
        function (result) { // fail login
            console.log("error: " + result);
            $rootScope.error = result;
        });
    };
    
}]);