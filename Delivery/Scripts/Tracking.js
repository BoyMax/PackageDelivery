
var Tracking = (function () {

    var trackGoogleImageUpload = function () {
        goog_report_conversion();
    };

    var trackFacebookLead = function () {
        fbq('track', 'Lead');
    };

    var trackFacebookPurchase = function (value) {
        fbq('track', 'Purchase', { value: value, currency: 'GBP' });
    };

    return {
        trackGoogleImageUpload: trackGoogleImageUpload,
        trackFacebookLead: trackFacebookLead,
        trackFacebookPurchase: trackFacebookPurchase,
    };

})();