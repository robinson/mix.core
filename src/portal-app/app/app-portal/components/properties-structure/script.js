modules.component('propertiesStructure', {
    templateUrl: '/app/app-portal/components/properties-structure/view.html',
    controller: ['$rootScope', '$scope',
        function ($rootScope, $scope) {
            var ctrl = this;
            ctrl.selectedCol = null;
            ctrl.settings = $rootScope.globalSettings;
            ctrl.defaultAttr = {
                title: '',
                name: '',
                defaultValue: null,
                options: [],
                priority: 0,
                dataType: 7,
                isGroupBy: false,
                isSelect: false,
                isDisplay: true,
                width: 3
            };
            
            ctrl.addAttr = function () {
                if (ctrl.columns) {
                    var t = angular.copy(ctrl.defaultAttr);
                    ctrl.columns.push(t);
                }
            };
    
            ctrl.addOption = function (col, index) {
                var val = $('#option_' + index).val();
                col.options.push(val);
                $('#option_' + index).val('');
            };
            ctrl.generateForm = function(){
                var formHtml = document.createElement('module-form');
                formHtml.setAttribute('class','row');
                angular.forEach(ctrl.activedData.columns, function(e,i){
                    var el;
                    var label = document.createElement('label');
                    label.setAttribute('class', 'control-label');
                    label.setAttribute('ng-bind', '{{data.title}}');
                    
                    switch(e.dataType){
                        case 1:
                        el = document.createElement('input');
                        el.setAttribute('type', 'datetime-local');                                 
                        break;
                        
                        case 2:
                        el = document.createElement('input');
                        el.setAttribute('type', 'date');                                 
                        break;
                        
                        case 3:
                        el = document.createElement('input');
                        el.setAttribute('type', 'time');                                 
                        break;
    
                        case 5:
                        el = document.createElement('input');
                        el.setAttribute('type', 'tel');                                 
                        break;
                       
                        case 6:
                        el = document.createElement('input');
                        el.setAttribute('type', 'number');                                 
                        break;
                       
                        case 8:
                        el = document.createElement('trumbowyg');
                        el.setAttribute('options', '{}');                                 
                        el.setAttribute('type', 'number');                                 
                        break;
                        
                        case 9:
                        el = document.createElement('textarea');
                        break;
    
                        default:
                        el = document.createElement('input');
                        el.setAttribute('type', 'text');
                        formHtml.appendChild(el);
                        break;
                    }
                    el.setAttribute('ng-model', 'data.jItem[' + e.name + '].value');
                    el.setAttribute( 'placeholder', '{{$ctrl.title}}');
                    formHtml.appendChild(label);      
                    formHtml.appendChild(el);      
                    
                });
                ctrl.activedData.formView.content = formHtml.innerHTML;
            };
    
            ctrl.generateName = function (col) {
                col.name = $rootScope.generateKeyword(col.title, '_');
            }
            ctrl.removeAttr = function (index) {
                if (ctrl.columns) {
                    ctrl.columns.splice(index, 1);
                }
            }
        }],
    bindings: {
        header: '=',
        columns: '='
    }
});