
function RotateImages(id)
{
    var current = ($('#rotator' + id + ' li.show')?  $('#rotator' + id + ' li.show') : $('#rotator' + id + ' li:first'));
    var next = ((current.next().length) ? ((current.next().hasClass('show')) ? $('#rotator' + id + ' li:first') :current.next()) : $('#rotator' + id + ' li:first'));

    next.css({opacity: 0.0})
        .addClass('show')
        .animate({opacity: 1.0}, 1000);

        current.animate({opacity: 0.0}, 1000)
        .removeClass('show');
}
            
function StartRotator(id, pause)
{
    $('#rotator' + id + ' li').css({opacity: 0.0});
    $('#rotator' + id + ' li:first').css({opacity: 1.0});

    if (pause < 0)
    {
        pause = 3;
    }

    setInterval(function () {
        RotateImages(id);
    },
    pause * 1000);
}               

