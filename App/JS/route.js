app.config(['$routeProvider', '$compileProvider', function ($routeProvider, $compileProvider) {
    $compileProvider.aHrefSanitizationWhitelist(/^\s*(https?|ftp|mailto|chrome-extension|app):/);
    $compileProvider.aHrefSanitizationWhitelist(/^\s*(https?|ftp|mailto|file|chrome-extension|app|ftp|blob):|data:image\//);
    $routeProvider
        .when("/", {
            templateUrl: "Partials/default.html"
        })
        .when("/Content/:category_id", {
            templateUrl: "Partials/HaberlerKategori.html"
        })
        .when("/haber_detay/:news_id", {
                    templateUrl: "Partials/haber_detay.html"
        })
        .when("/hata", {
            templateUrl: "Partials/hata.html"
        })
        .when("/login", {
            templateUrl: "Partials/loginpage.html"
        })
        .when("/signup", {
            templateUrl: "Partials/signup.html"
        })
        .when("/xu4q8r9lsx7jry2as1fj", {
            templateUrl: "Partials/adminpage.html"
        })
        .when("/tumhaberler", {
            templateUrl: "Partials/tumhaberler.html"
        })

        .otherwise({ redirectTo: '/' });

    //$mdGestureProvider.skipClickHijack();

}]);

app.config(['$httpProvider', function ($httpProvider) {
    $httpProvider.defaults.useXDomain = true;

    delete $httpProvider.defaults.headers.common['X-Requested-With'];
}
]);