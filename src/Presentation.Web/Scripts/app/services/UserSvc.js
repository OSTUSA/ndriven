'use strict';

app.factory('UserSvc', function ($resource) {
    return {

        user: $resource('/api/users/:Id/', {}, {

            get: {
                method: 'GET',
                headers: {
                    "X-AUTH-TOKEN": '@Token'
                },
                params: { Id: '@Id' },
                isArray: false
            },
        })
    };
});