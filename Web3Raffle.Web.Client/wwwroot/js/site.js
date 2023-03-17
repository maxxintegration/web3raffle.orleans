web3raffle = (function () {

  "use strict";

  // FLAG MOBILE
  if (/android|webos|iphone|ipad|ipod|blackberry|iemobile|opera mini|mobile/i.test(navigator.userAgent)) {
    document.querySelector("body").classList.add("mobile-device");
  }

  var triggerFileDownload = function (fileName, url) {
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = fileName ?? '';
    anchorElement.click();
    anchorElement.remove();
  };

  return {
    triggerFileDownload: triggerFileDownload

  };

})();