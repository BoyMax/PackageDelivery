/* global $ */
$(function () {
	$('input[type="number"]').blur(function (e) {
		var that = $(this),
			val = parseInt(that.val()),
			min = parseInt(that.attr('min')),
			max = parseInt(that.attr('max'));
		if (min && val < min) {
			that.val(min);
			that.change();
		} else if (max && val > max) {
			that.val(max);
			that.change();
		}
	});
	$('input[type="number"], input.number').keydown(function (e) {
		if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
		  (e.keyCode == 65 && (e.ctrlKey === true || e.metaKey === true)) ||
		  (e.keyCode >= 35 && e.keyCode <= 40)) {
			return;
		}
		if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
			e.preventDefault();
			return false;
		}
	});
	$('input.number').keyup(function () {
		if (!parseInt($(this).val())) {
			$(this).val(0).change();
		}
	});
	$('.color-select').each(function () {
		var $this = $(this), numberOfOptions = $(this).children('option').length;

		$this.addClass('select-hidden');
		$this.wrap('<div class="select"></div>');
		$this.after('<div class="select-styled"></div>');

		var $styledSelect = $this.next('div.select-styled');
		$styledSelect.text($this.children('option:selected').text());

		var $list = $('<ul />', {
			'class': 'select-options'
		}).insertAfter($styledSelect);

		for (var i = 0; i < numberOfOptions; i++) {
			$('<li />', {
				text: $this.children('option').eq(i).text(),
				rel: $this.children('option').eq(i).val()
			}).appendTo($list);
		}

		var $listItems = $list.children('li');
		$styledSelect.click(function (e) {
			e.stopPropagation();
			$('div.select-styled.active').not(this).each(function () {
				$(this).removeClass('active').next('ul.select-options').hide();
			});
			$(this).toggleClass('active').next('ul.select-options').toggle();
		});
		$listItems.click(function (e) {
			e.stopPropagation();
			$styledSelect.text($(this).text()).removeClass('active');
			$this.val($(this).attr('rel'));
			$this.change();
			$list.hide();
		});
		$(document).click(function () {
			$styledSelect.removeClass('active');
			$list.hide();
		});
	});

	$('.open-popup-link').magnificPopup({
		type: 'inline',
		midClick: true // Allow opening popup on middle mouse click. Always set it to true if you don't provide alternative source in href.
	});

	$('.gallery').each(function () {
		$(this).magnificPopup({
			delegate: 'a',
			type: 'image',
			titleSrc: 'title',
			gallery: {
				enabled: true,
				navigateByImgClick: true,
				arrowMarkup: '<button title="%title%" type="button" class="mfp-arrow mfp-arrow-%dir%"></button>',
				tPrev: 'Previous (Left arrow key)',
				tNext: 'Next (Right arrow key)',
				tCounter: '<span class="mfp-counter">%curr% of %total%</span>'
			}
		});
	});

	var getEmail = function (data) {
		return atob(data).split("").reverse().join("");
	};
	$('a[data-mailto]').each(function () {
		$(this).attr('href', 'mailto:' + getEmail($(this).data('mailto')));
	});
	$('a[data-mailto-text]').each(function () {
		$(this).text(getEmail($(this).data('mailto')));
	});

	// Init all tooltips
	$('[data-toggle="tooltip"]').tooltip();

});

function isEmail(email) {
	var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
	return re.test(email);
}

var Router = (function () {

	var params = window.location.search;

	var pathname = window.location.pathname.split('/');
	var orderId = pathname.length === 3 ? '/' + window.location.pathname.split('/')[2] : '';

	var quotePage = function () {
		location.href = "/quote" + params;
	};
	var orderPage = function () {
		location.href = "/order" + orderId;
	};
	var checkoutPage = function () {
		location.href = "/checkout" + orderId;
	};

	var redirect = function (route) {
		location.href = route + params;
	};

	return {
		goToQuotePage: quotePage,
		goToOrderPage: orderPage,
		goToCheckoutPage: checkoutPage,
		goTo: redirect
	};

})();