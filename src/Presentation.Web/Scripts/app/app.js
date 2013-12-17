var app = angular.module('app', ["ngResource", "BDOPortal.directives", "ui.bootstrap", "ui.directives", "ngSanitize", "ui"]);

app.config(['$routeProvider', function ($routeProvider) {
    $routeProvider.
        when('/register', { templateUrl: 'templates/register', controller: 'RegisterCtrl' }).
        when('/login', { templateUrl: 'templates/login', controller: 'LoginCtrl' }).
        when('/dashboard', { templateUrl: 'templates/dashboard', controller: 'DashboardCtrl' }).
        otherwise({ redirectTo: '/dashboard' });

}]);

app.run(['$rootScope', function ($rootScope) {
    
   

    $rootScope.closeErrorModal = function () {
        $('#errorModal').modal('hide');
    };

    // watch for errors - when there are open modal else keep it closed
    $rootScope.$watch('error', function () {
        if ($rootScope.error) {
            $rootScope.busy = false;
            $('#errorModal').modal('show');
            $('#busyModal').modal('hide');
        } else {
            $('#errorModal').modal('hide');
        }
    }, true);
    
    $rootScope.clearError = function () {
        $rootScope.error = "";
        $rootScope.busy = false;
        $scope.closeErrorModal();
    };

    $rootScope.convertToUTC = function (dt) {
        var localDate = new Date(dt);
        var localTime = localDate.getTime();
        var localOffset = localDate.getTimezoneOffset() * 60000;
        return new Date(localTime + localOffset);
    };

}]);
