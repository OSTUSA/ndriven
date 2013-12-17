'use strict';

app.factory('RegisterSvc', function ($resource, $http) {
    return {

        register: $resource("api/register", {}, {

            post: {
                method: 'POST',
                params: {},
                data: {
                    Email: '@Email',
                    Name: '@Name',
                    Password: '@Password',
                },
                isArray: false
            },
        })
    };
});


