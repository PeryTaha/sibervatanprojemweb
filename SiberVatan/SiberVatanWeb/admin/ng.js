var app = angular.module('blogAdmin', []);

app.controller('AdminController', function($scope) {
    // 1. Çekmeceden verileri al (En başta duruyor, süper)
    $scope.yazilar = JSON.parse(localStorage.getItem('blogVerileri')) || [];

    // 2. BLOG EKLEME FONKSİYONU
    $scope.blogEkle = function() {
        if (!$scope.yeniBlog || !$scope.yeniBlog.baslik || !$scope.yeniBlog.icerik) {
            alert("Kanka başlık ve içeriği boş bırakma!");
            return;
        }

        let yeni = {
            id: Date.now(),
            baslik: $scope.yeniBlog.baslik,
            ozet: $scope.yeniBlog.ozet,
            etiket: $scope.yeniBlog.etiket,
            icerik: $scope.yeniBlog.icerik,
        };

        $scope.yazilar.push(yeni);
        localStorage.setItem('blogVerileri', JSON.stringify($scope.yazilar));

        $scope.yeniBlog = {}; // Formu temizle
        alert("Yazı başarıyla yayınlandı kanka!");
    }; // <--- blogEkle burada BİTMELİ!

    // 3. BLOG SİLME FONKSİYONU (Ekleme bittikten sonra, tek başına)
    $scope.blogSil = function(id) {
        if(confirm("Bu yazıyı siliyorum, emin misin?")) {
            $scope.yazilar = $scope.yazilar.filter(function(item) {
                return item.id !== id;
            });
            localStorage.setItem('blogVerileri', JSON.stringify($scope.yazilar));
        }
    };

}); // <--- Controller burada biter.