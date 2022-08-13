var app = angular.module('myApp', ['ngRoute', 'angularjsToast', 'ngCookies'] );


app.controller('MainController', function ($scope, $rootScope, $http, $cookies, $location) {
    $scope.logOut = function () {
        $location.path("/login");
    }
    if ($cookies.get('habertoken') == null) {
        $scope.logOut();
    }
    else {
        var accesstoken = $cookies.get('habertoken');
        $scope.authHeaders = {};
        $scope.authHeaders.Authorization = 'Bearer ' + accesstoken;
    }

    $scope.tokenKontrol = function () {
        if ($cookies.get('habertoken') == null) {
            $scope.logOut();
        }
    }
   



    $scope.Menu = [];
    $rootScope.arama = "Search";
    $http.get("/api/v1/Haberler/getkategs").then(function (resp) {
        $scope.kategs = resp.data;
    });
    
    
   
    //$scope.getNewById = function (id) {
    //    $http.get("/api/v1/Haberler/GetNewById/"+id).then(function (response) {
    //        $scope.habers = response.data;
    //    });
    //}
    //$scope.getNewById(11);
    //$scope.getAllLastDokuz = function () {
    //    $http.get("/api/v1/Haberler/GetAllLastDokuz").then(function (response) {


    //        $scope.newsLastNine = response.data;
    //    });
      


    //}
    //$scope.getAllLastDokuz();
    //$scope.getAll = function () {
    //    $http.get("/api/v1/Haberler/GetAll").then(function (response) {
    //        $scope.getAll = response.data;
    //    });
    //}
    /*$scope.getAll();*/
    //$scope.getAllLastThree = function () {
    //    $http.get("/api/v1/Haberler/GetAllLastThree").then(function (response) {
    //        $scope.lastThreeElement = response.data;
    //    });
    //}
    //$scope.getAllLastThree();



   

});

app.controller('ListController', function ($scope, $http,toast) {
    //$scope.getAll = function () {
    //    $http.get("/api/v1/Haberler/GetAll").then(function (response) {
    //        $scope.habers = response.data;
    //        $scope.setMaxPage($scope.habers.length);
    //    });
    //}
    //$scope.getAll();
    $scope.tokenKontrol();
    $scope.getAllLastThree = function () {
        $http.get("/api/v1/Haberler/GetAllLastThree").then(function (response) {
            $scope.lastThreeElement = response.data;
        });
    }
    $scope.getAllLastThree();

    //pager//
    $scope.setMaxPage = function (count) {
        $scope.maxPage = [];
        var say = parseInt(count);
        var kalanvarmi = say % $scope.itemsPerPage;
        var maxPage = 0;
        if (kalanvarmi > 0) {
            maxPage = 1 + parseInt(say / $scope.itemsPerPage);
        }
        else  {
            maxPage = parseInt(say / $scope.itemsPerPage);
        }
        for (var i = 0; i < maxPage; i++) {
            $scope.maxPage.push(i+1);
        }
    }
    
    $scope.pagedNews = [];
    $scope.goToPage = function (newPage, itemsPerPage) {
        $http.get("/api/v1/Haberler/GoToPage/" + newPage+ "/" +itemsPerPage).then(function (resp) {
            $scope.pagedNews = resp.data.data;
            if (resp.data.success) {
                $scope.setMaxPage(resp.data.mesaj);
            }
            else {
                $scope.hataGoster(resp.data.mesaj);
            }
        })
    }
    $scope.curPage = 1;
    $scope.itemsPerPage = 9;
    //$scope.setMaxPage(99);
    $scope.hataGoster = (mesaj) => {
        //TODO Toast işlemlerini yap
        toast.create({
            timeout: 5 * 1000,
            message: mesaj,
            className: 'alert-danger',
            dismissible: true
            
        });
    }
    $scope.mesajGoster = (mesaj) => {
        //TODO Toast işlemlerini yap
        toast.create({
            timeout: 3 * 1000,
            message: mesaj,
            className: 'alert-success',
            dismissible: true
            
        });
    }
    $scope.goToPage($scope.curPage, $scope.itemsPerPage);

   ////pager//
    $scope.getkategs = function () {
        $http.get("/api/v1/Haberler/getkategs").then(function (response) {
            $scope.getkategs = response.data;
        });
    }
    /*$scope.getByCategory();*/
    $scope.getkategs();
    
   
});

app.controller('GetController', function ($scope, $http, $routeParams) {

    var kateg_id = $routeParams.category_id;
    $scope.getByCategory = function (kateg_id) {
        $http.get("/api/v1/Haberler/GetByCategory/" + kateg_id).then(function (response) {
            $scope.haber1 = response.data;
            /*$scope.setMaxPage(resp.data);*/
        });
    }
    $scope.getkategs = function () {
        $http.get("/api/v1/Haberler/getkategs").then(function (response) {
            $scope.habers = response.data;
        });
    }
    $scope.getByCategory(kateg_id);
    $scope.getkategs();
});
app.controller('GetAllController', function ($scope, $http) { 
    $scope.getAll = function () {
        $http.get("/api/v1/Haberler/GetAll").then(function (response) {
            $scope.allNews = response.data;

            for (let i = 0; i < allNews.length; i++) {
                var kategori = response.data[i].category_id;
            switch (kategori) {
                case 1: kategori = console.log("Spor");
                    break;
                case 2: kategori = console.log("Gündem");
                    break;
                case 3: kategori = console.log("Teknoloji");
                    break;
                case 4: kategori = console.log("Magazin");
                    break;
            }

            
               console.log(kategori)

            }
           
            
        });


    }
    app.config(['$qProvider', function ($qProvider) {
        $qProvider.errorOnUnhandledRejections(false);
    }]);
    $scope.getUser = function () {
            $http.get("/api/v1/Haberler/GetUser").then(function (response) {
                $scope.allUsers = response.data;
                
            });
        }
        $scope.getUser();
        $scope.getAll();
        
});
app.controller('HaberDetayController', function ($scope, $http) {
    $scope.GetNewById = function () {
        $http.get("/api/v1/Haberler/GetNewById").then(function (response) {
            $scope.GetNewById = response.data;
        });
    }
    $scope.GetNewById();
});
app.controller('SignUpController', function ($scope, $http) {
    $scope.SignUp = function () {
        $http.post("/api/v1/Haberler/UserEkleme", JSON.stringify($scope.UserEkleme)).then(function (response) {
            $scope.UserEkleme = response.data;
            

        });
    }
    
   




});
app.controller('LoginController', function ($scope, $http, $location, $cookies) {
    $cookies.remove('habertoken');
    $scope.hata = false;
    $scope.reset = function () {
        $scope.LoginInfo = { fullname: null, userType: 0, userId: 0, username: null, password: null };
    }
    $scope.reset();
    $scope.Login = function () {
        $http.post("/api/v1/Login", JSON.stringify($scope.LoginInfo)).then(function (response) {
            /*console.log(response.data.mesaj);*/
            if (response.data.success) {
                if (response.data.variables == 1 || response.data.variables == 2) {
                    var userToken = response.data.mesaj;
                    console.log("YAZAR VEYA KULLANICI")
                    console.log(userToken);
                    $cookies.put('habertoken', userToken, { 'expires': new Date(2030, 12, 01) });
                    /*var path = $location.path("/Partials/default.html");*/
                    $scope.hata = false;
                    var path = $location.path("/");
                    var Token = $cookies.tokenCookies;
                    $cookies.tokenCookies = userToken;
                    /*console.log(tokenCookies)*/

                    /*  $cookie.put(response.data.userId, userToken);*/
                }
                else {
                    var userToken = response.data.mesaj;
                    console.log(userToken);
                    /*  $cookie.put(response.data.userId, userToken);*/
                    /*var path = $location.path("/Partials/default.html");*/
                    $scope.hata = false;
                    var path = $location.path("/xu4q8r9lsx7jry2as1fj");
                    /*  $cookie.put(response.data.userId, userToken);*/

                }
                
            }
            else {
                $scope.hata = true;
                console.log("böyle bir kullanıcı yok.")
            }
            //localStorage.setItem('userToken', response.token);
            

        });
        

        /*location.pathname*/


        //http.get().then().catch().finally();
    }
    






});
app.controller('HaberController', function ($scope, $http) {
    $scope.haberEkleme = function () {
        $http.post("/api/v1/Haberler/HaberEkleme", JSON.stringify($scope.haberInfo)).then(function (response) {
            console.log(haber1);
            $scope.haber1 = response.data;

        });
    }
    function myFunction() {
        var x = document.getElementById("myDatetime").value;
        document.getElementById("HaberInfo.DatePosted").innerHTML = x;
    }
    $scope.haberEkleme();
    $scope.haberGuncelleme = function () {
        $http.get("/api/v1/Haberler/HaberGuncelleme").then(function (response) {
            $scope.haber1 = response.data;
        });
    }

});
    



    
    








   
