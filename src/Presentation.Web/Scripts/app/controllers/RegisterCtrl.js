'use strict';

app.controller("RegisterCtrl", ["$rootScope", "$scope", "$http", "$location", "RegisterSvc", "LoginSvc", function ($rootScope, $scope, $http, $location, registerSvc, loginSvc) {

    // POST: register new user
    $scope.register = function() {
        registerSvc.register.post({
                Email: $scope.Email,
                Name: $scope.Name,
                Password: $scope.Password
            },
            function () { // success register
                // login automatically
                var loginRsp = loginSvc.login.post({
                    Email: $scope.Email,
                    Password: $scope.Password,
                    Token: btoa($scope.Email + ':' + $scope.Password)
            }, 
                function() { // success login
                    $rootScope.User = loginRsp;
                    $location.path('/dashboard');
                },
                function(result) { // fail login
                    console.log("error: " + result);
                    $rootScope.error = result;
                });
            },
            function(result) { // fail register
                console.log("error: " + result);
                $rootScope.error = result;
            });
    };
    

}]);
