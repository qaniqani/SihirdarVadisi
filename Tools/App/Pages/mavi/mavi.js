app.controller("MaviController", ["$scope", function ($scope) {
	$scope.script = function () {
		$(function () {
			var sonuc = $('.sonuc'),
				toplam = 0;

			$('input').each(function () {
				$(this).val('0');
			}).keydown(function(e) {
				if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
					(e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) ||
					(e.keyCode >= 35 && e.keyCode <= 40)) {
					return;
				}
				if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
					e.preventDefault();
				}
			}).keyup(function () {
				var self = $(this),
					val = parseInt(self.val()),
					hedef = self.data('hedef'),
					carpan = self.data('carpan');
				if($.isNumeric(val))
					$(hedef).text(val * carpan);
				else
					self.val('0');

				var sum = 0;
				$(".ara-sonuc").each(function(){
					sum += +$(this).text();
				});
				sonuc.text(sum);
			});

		});
	};
	$scope.script();
}]);