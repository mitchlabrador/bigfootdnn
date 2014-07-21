//*********************************************
// jquery.centerXY plugin
//*********************************************
// jQuery centerXY 1.0 Plugin by John Terenzio (http://terenz.io)
(function ($) {
    $.fn.centerXY = function (offsetX, offsetY) {
        offsetX = offsetX || 0; offsetY = offsetY || 0;
        return this.each(function () {
            var elem = $(this);

            // Make sure the parent element is not empty
            if (elem.parent()[0] == undefined) return;

            var parent = (elem.parent()[0].tagName.toLowerCase() == ('body' || 'html')) ? $(window) : elem.parent();
            var left = Math.floor((parent.width() - elem.width()) / 2 + offsetX);
            var top = Math.floor((parent.height() - elem.height()) / 2 + offsetY);
            elem.css('position', 'absolute'); 
            elem.css('top', (top > 0) ? top + 'px' : top + 'px'/*'0px'*/);
            elem.css('left', (left > 0) ? left + 'px' : left + 'px' /*'0px'*/); 
            $(window).one('resize', function () { elem.centerXY(offsetX, offsetY); });
        });
    };

    $.fn.centerX = function (offsetX) {
        offsetX = offsetX || 0;
        return this.each(function () {
            var elem = $(this);

            // Make sure the parent element is not empty
            if (elem.parent()[0] == undefined) return;

            var parent = (elem.parent()[0].tagName.toLowerCase() == ('body' || 'html')) ? $(window) : elem.parent();
            var left = Math.floor((parent.width() - elem.width()) / 2 + offsetX);
            elem.css('position', 'absolute');
            elem.css('left', (left > 0) ? left + 'px' : left + 'px' /*'0px'*/); 
            $(window).one('resize', function () { elem.centerX(offsetX); });
        });
    };

    $.fn.centerY = function (offsetY) {
        offsetY = offsetY || 0;
        return this.each(function () {
            var elem = $(this);

            // Make sure the parent element is not empty
            if (elem.parent()[0] == undefined) return;

            var parent = (elem.parent()[0].tagName.toLowerCase() == ('body' || 'html')) ? $(window) : elem.parent();
            var top = Math.floor((parent.height() - elem.height()) / 2 + offsetY);
            elem.css('position', 'absolute');
            elem.css('top', (top > 0) ? top + 'px' : top + 'px'/*'0px'*/);
            $(window).one('resize', function () { elem.centerY(offsetY); });
        });
    };


})(jQuery);
