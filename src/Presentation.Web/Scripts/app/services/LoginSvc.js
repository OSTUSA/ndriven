'use strict';

app.factory('LoginSvc', function ($resource, $http) {
    return {

        login: $resource("api/login", {}, {

            post: {
                method: 'POST',
                headers: {
                    Authorization: '@Authorization'
                },
                data: {
                    Email: '@Email',
                    Password: '@Password',
                },
                isArray: false
            },
        })
    };
});


