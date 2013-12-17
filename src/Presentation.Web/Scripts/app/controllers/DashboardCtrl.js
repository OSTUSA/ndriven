'use strict';

app.controller("DashboardCtrl", ["$rootScope", "$scope", "$http", "$location", "UserSvc", function ($rootScope, $scope, $http, $location, userSvc) {

    // GET: user data
    $scope.getUser = function () {
        var userResp = userSvc.user.get({
            Id: $rootScope.User.Id,
            Token: $rootScope.User.Token
        },
        function () { // success login
            $rootScope.User = userResp;
        },
        function (result) { // fail login
            console.log("error: " + result);
            $rootScope.error = result;
        });
    };

    // load dashboard data if we are logged in
    if ($rootScope.User != null && $rootScope.User.Token) {
        $scope.getUser();
    }
    
}]);