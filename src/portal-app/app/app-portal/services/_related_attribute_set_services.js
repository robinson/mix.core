﻿
'use strict';
app.factory('RelatedAttributeSetService', ['$rootScope', 'CommonService', 'BaseODataService',
    function ($rootScope, commonService, baseService) {
        var serviceFactory = angular.copy(baseService);
        serviceFactory.init('related-attribute-set');
        var _getList = async function (viewType, objData, parentType, parentId) {
            objData.filter  = '';            
            if(parentType){
                if(objData.filter){
                    objData.filter += ' and ';
                }
                objData.filter += 'parentType eq ' + parentType;
            }
            if(parentId){
                if(objData.filter){
                    objData.filter += ' and ';
                }
                objData.filter += "parentId eq " + parentId;
            }        
            var data = serviceFactory.parseODataQuery(objData);           
            var url = this.prefixUrl + '/' + viewType;
            if(data){
                url = url.concat(data);
            }
            var req = {
                method: 'GET',
                url: url
            };
            return await commonService.getApiResult(req);
        };
        var _getOthers = async function (viewType, objData, parentType, parentId) {
            objData.filter  = '';            
            if(parentType){
                if(objData.filter){
                    objData.filter += ' and ';
                }
                objData.filter += 'parentType eq ' + parentType;
            }
            if(parentId){
                if(objData.filter){
                    objData.filter += ' and ';
                }
                objData.filter += "parentId ne " + parentId;
            }        
            var data = serviceFactory.parseODataQuery(objData);           
            var url = this.prefixUrl + '/' + viewType;
            if(data){
                url = url.concat(data);
            }
            var req = {
                method: 'GET',
                url: url
            };
            return await commonService.getApiResult(req);
        };
        
        serviceFactory.getOthers = _getOthers;
        serviceFactory.getList = _getList;
        return serviceFactory;

    }]);
