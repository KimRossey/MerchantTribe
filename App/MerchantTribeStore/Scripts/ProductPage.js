// Product Page Javascript

var currentmediumimage;

// Modal Popup Code
function CloseDialog() {
    $('.overlay').remove();
    $('.modal2').hide();
}

function OpenDialog(lnk) {
    $('<div />').addClass('overlay').appendTo('body').show();
    $('.modal2').show();
    var loadid = $(lnk).attr('href');
    $('#popoverpage2').attr('src', loadid);
}

$(document).keyup(function(e) {
    if (e.keyCode == 27) { CloseDialog(); }
});

$(document).ready(function () {

        // AdditionalProductImage
        $('.additionalimages a').mouseover( function() {
            var newurl = $(this).attr('alt');
            var currentimg = $('#imgMain').attr('src');
            $('#imgMainLast').val(currentimg);
            $('#imgMain').attr('src',newurl);
        });

        $('.additionalimages a').mouseout( function() {
            var originalimg = $('#imgMainLast').val();
            $('#imgMain').attr('src',originalimg);
        });


        // Popup Close
        $('#dialogclose').click(function () {
           CloseDialog();
           return false;
        });
        $('#dialogclose2').click(function () {
           CloseDialog();
           return false;
        });

    // Popup Open
    $('.popover').click(function () {    
        OpenDialog($(this));
        return false;
    });

    CloseDialog();
});
            