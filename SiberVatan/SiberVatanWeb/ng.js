var app = angular.module("blogApp", []);
var API_BASE = "https://localhost:44364/api";

function isManagementPath() {
    return window.location.pathname.toLowerCase().indexOf("/yonetim-771/") !== -1;
}

function authConfig() {
    return {
        headers: {
            "X-Admin-Token": localStorage.getItem("adminToken") || ""
        }
    };
}

app.controller("blogController", function($scope, $http) {
    $scope.bloglar = [];
    $scope.yukleniyor = true;

    $http.get(API_BASE + "/blog/BlogGetir").then(function(response) {
        $scope.bloglar = response.data;
    }).catch(function() {
        $scope.bloglar = [];
    }).finally(function() {
        $scope.yukleniyor = false;
    });
});

app.controller("adminController", function($scope, $http, $window) {
    if (localStorage.getItem("girisAnahtari") !== "aktif" || !localStorage.getItem("adminToken")) {
        $window.location.href = "giris.html";
        return;
    }

    $scope.bloglar = [];
    $scope.blogs = {};
    $scope.panelMesaj = "";
    $scope.duzenlemeModu = false;

    function bloglariYukle() {
        $http.get(API_BASE + "/blog/BlogGetir").then(function(response) {
            $scope.bloglar = response.data;
        }).catch(function() {
            $scope.panelMesaj = "Blog listesi alınamadı. API veya veritabanı bağlantısını kontrol et.";
        });
    }

    function formGecerliMi(blogVerisi) {
        return blogVerisi &&
            blogVerisi.baslik &&
            blogVerisi.ozet &&
            blogVerisi.etiket &&
            blogVerisi.icerik;
    }

    bloglariYukle();

    $scope.blogKaydet = function(blogVerisi) {
        if (!formGecerliMi(blogVerisi)) {
            $scope.panelMesaj = "Başlık, özet, etiket ve içerik alanları zorunludur.";
            return;
        }

        var istek = $scope.duzenlemeModu
            ? $http.post(API_BASE + "/blog/BlogGuncelle", blogVerisi, authConfig())
            : $http.post(API_BASE + "/blog/BlogGetir2", blogVerisi, authConfig());

        var duzenleniyordu = $scope.duzenlemeModu;

        istek.then(function(response) {
            $scope.bloglar = response.data;
            $scope.blogs = {};
            $scope.duzenlemeModu = false;
            $scope.panelMesaj = duzenleniyordu ? "Yazı güncellendi." : "Yazı başarıyla yayınlandı.";
        }).catch(function(error) {
            if (error.status === 401) {
                localStorage.removeItem("girisAnahtari");
                localStorage.removeItem("adminToken");
                $window.location.href = "giris.html";
                return;
            }

            $scope.panelMesaj = "İşlem tamamlanamadı. Alanları ve bağlantıyı kontrol et.";
        });
    };

    $scope.blogekle = function(blogVerisi) {
        $scope.blogKaydet(blogVerisi);
    };

    $scope.duzenle = function(blog) {
        $scope.duzenlemeModu = true;
        $scope.panelMesaj = "Düzenleme modu aktif.";
        $scope.blogs = angular.copy(blog);
        window.scrollTo({ top: 0, behavior: "smooth" });
    };

    $scope.duzenlemeyiIptalEt = function() {
        $scope.duzenlemeModu = false;
        $scope.blogs = {};
        $scope.panelMesaj = "";
    };

    $scope.blogsil = function(id) {
        $http.post(API_BASE + "/blog/blogsil2?id=" + id, null, authConfig()).then(function(response) {
            $scope.bloglar = response.data;
            $scope.panelMesaj = "Yazı silindi.";
            if ($scope.blogs.id === id) {
                $scope.duzenlemeyiIptalEt();
            }
        }).catch(function(error) {
            if (error.status === 401) {
                localStorage.removeItem("girisAnahtari");
                localStorage.removeItem("adminToken");
                $window.location.href = "giris.html";
                return;
            }

            $scope.panelMesaj = "Yazı silinemedi.";
        });
    };

    $scope.guvenliCikis = function() {
        localStorage.removeItem("girisAnahtari");
        localStorage.removeItem("adminToken");
        $window.location.href = "../blog.html";
    };
});

app.controller("LoginController", function($scope, $http, $window) {
    $scope.user = {};
    $scope.loginMesaj = "";

    $scope.girisYap = function() {
        if (!$scope.user.kullaniciadi || !$scope.user.sifre) {
            $scope.loginMesaj = "Kullanıcı adı ve şifre gerekli.";
            return;
        }

        $http.post(API_BASE + "/login/kontrol", $scope.user)
            .then(function(response) {
                localStorage.setItem("girisAnahtari", "aktif");
                localStorage.setItem("adminToken", response.data.token);
                $window.location.href = isManagementPath() ? "panel.html" : "yonetim-771/panel.html";
            })
            .catch(function() {
                $scope.loginMesaj = "Giriş bilgileri hatalı veya API erişilemiyor.";
            });
    };
});

app.controller("detayController", function($scope, $http) {
    $scope.yazi = {};
    var urlParams = new URLSearchParams(window.location.search);
    var blogId = urlParams.get("id");

    if (blogId) {
        $http.get(API_BASE + "/blog/BlogDetayGetir?id=" + blogId).then(function(response) {
            $scope.yazi = response.data;
        });
    }
});
