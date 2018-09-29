﻿'use strict';
app.controller('PageController', ['$scope', '$rootScope', '$routeParams', 'ngAppSettings', 'PageService', '$routeParams',
    function ($scope, $rootScope, $routeParams, ngAppSettings, service, $routeParams) {
        service.init('page');
        BaseCtrl.call(this, $scope, $rootScope, $routeParams, ngAppSettings, service);
        $scope.request.query = 'level=0';        
        $scope.loadPageDatas = async function () {
            $rootScope.isBusy = true;
            var id = $routeParams.id;
            var response = await pageServices.getPage(id, 'fe');
            if (response.isSucceed) {
                $scope.activedPage = response.data;
                $rootScope.initEditor();
                $rootScope.isBusy = false;
                $scope.$apply();
            }
            else {
                $rootScope.showErrors(response.errors);
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };

        $scope.updateInfos = async function (items) {
            $rootScope.isBusy = true;
            var resp = await pageServices.updateInfos(items);
            if (resp && resp.isSucceed) {
                $scope.activedPage = resp.data;
                $rootScope.showMessage('success', 'success');
                $rootScope.isBusy = false;
                $scope.$apply();
            }
            else {
                if (resp) { $rootScope.showErrors(resp.errors); }
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };

    }]);
