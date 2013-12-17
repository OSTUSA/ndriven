'use strict';

angular.module('BDOPortal.directives', [])
    .directive('partnerinfo', function () {
        return {
            restrict: 'E',
            templateUrl: 'http://localhost:50522/templates/partnerinfo'
        };
    })
    .directive('retirementhighestearning', function () {
        return {
            restrict: 'E',
            templateUrl: 'http://localhost:50522/templates/retirementHighestEarnings'
        };
    })
     .directive('retirementbenefitfactors', function () {
         return {
             restrict: 'E',
             templateUrl: 'http://localhost:50522/templates/retirementBenefitFactors'
         };
     })
      .directive('retirementaccountbalance', function () {
          return {
              restrict: 'E',
              templateUrl: 'http://localhost:50522/templates/retirementAccountBalance'
          };
      })
      .directive('retirementpaymentschedule', function () {
          return {
              restrict: 'E',
              templateUrl: 'http://localhost:50522/templates/retirementPaymentSchedule'
          };
      });


//app.directive('a', function () {
//    return {
//        restrict: 'E',
//        link: function (scope, elem, attrs) {
//            if (attrs.ngClick || attrs.href === '' || attrs.href === '#') {
//                elem.on('click', function (e) {
//                    e.preventDefault();
//                });
//            }
//        }
//    };
//});
