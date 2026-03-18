var app = angular.module("blogApp", []);

// ==========================================
// 1. blog.html (Ana Sayfa Listeleme)
// ==========================================
app.controller("blogController", function($scope, $http) {
    $http.get("https://localhost:44364/api/blog/BlogGetir").then(function(response) {
        // HTML'de {{blog.baslik}} yaptığın için artık veriler buraya dolacak
        $scope.bloglar = response.data;
    });
});

// ==========================================
// 2. admin.html (Ekleme ve Silme Sayfası)
// ==========================================
app.controller("adminController", function($scope, $http, $window) {
    
    // Sayfa yüklenince listeyi çek
    $http.get("https://localhost:44364/api/blog/BlogGetir").then(function(response) {
        $scope.bloglar = response.data;
    });

    // BLOG EKLEME (Senin HTML'deki ng-model="blogs.baslik" yapısına göre)
    $scope.blogs = {}; 
    $scope.blogekle = function(blogVerisi) {
        $http.post("https://localhost:44364/api/blog/BlogGetir2", blogVerisi).then(function(response) {
            $scope.bloglar = response.data; // Listeyi yenile
            $scope.blogs = {}; // Formu temizle
        });
    };

    // BLOG SİLME (ng-click="blogsil(x.id)")
    $scope.blogsil = function(id) {
        $http.get("https://localhost:44364/api/blog/blogsil2?id=" + id).then(function(response) {
            $scope.bloglar = response.data;
        });
    };

    // Geri dön butonu (ng-click="guvenliCikis()")
    $scope.guvenliCikis = function() {
        $window.location.href = 'blog.html';
    };
});

// ==========================================
// 3. indexx.html (Giriş Sayfası)
// ==========================================
app.controller('LoginController', function($scope, $http, $window) {
    $scope.user = {}; 
    $scope.girisYap = function() {
        // Senin HTML'deki user.kullaniciadi ve user.sifre buraya geliyor
        $http.post('https://localhost:44364/api/login/kontrol', $scope.user)
            .then(function(response) {
                // Giriş başarılıysa anahtarı ver ve SENİN admin sayfana yolla
                localStorage.setItem("girisAnahtari", "aktif");
                
                // DİKKAT: Ekleme yaptığın sayfanın adı 'admin.html' mi 'Index.html' mi? 
                // Eğer hata alırsan buradaki ismi dosya adınla değiştir kanka.
                $window.location.href = 'admin.html'; 
            })
            .catch(function() {
                alert("Giriş bilgileri hatalı!");
            });
    };
});

// ==========================================
// 4. devam.html (Detay Sayfası)
// ==========================================
app.controller("detayController", function($scope, $http) {
    var urlParams = new URLSearchParams(window.location.search);
    var blogId = urlParams.get('id');

    if (blogId) {
        $http.get("https://localhost:44364/api/blog/BlogDetayGetir?id=" + blogId).then(function(response) {
            // Senin HTML'deki {{yazi.baslik}} yapısına göre 'yazi' ismini kullandık
            $scope.yazi = response.data; 
        });
    }
});