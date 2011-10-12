﻿
function SubmitEditor() {
    
    var target = $('#editorform');
    var senddata = target.serialize();

    $.ajax({ type: "POST",
        url: target.attr('action'),
        data: senddata,
        dataType: "json",
        success: function (data) {
            var newHtml = data.ResultHtml;
            var outputdiv = 'part' + $('#flexpageediting').html();

            if (data.IsFinishedEditing) {
                $('#' + outputdiv).replaceWith(newHtml);
                CloseDialog();
                BindEverything();
            }
            else {
                target.html(data.ResultHtml);
                if (data.ScriptFunction.length > 0) {
                    eval(data.ScriptFunction);
                }                
                BindEverything();
            }
        },
        error: function () {
            alert('failed');
            BindEverything();
        }
    }
    );
    return false;
}

function BindJsonForms() {
    $('#editorform').unbind();

    $('#editorform').submit(function () {       
        SubmitEditor();
        return false;
    });
}

function BindEditable() {
    $('.editable').unbind();
    $('.coltools').unbind();
    $('.edithandle').unbind();

    $('.coltools').hover(
        function () { $(this).addClass('formhover'); $(this).find('.coledittools').show(); },
        function () { $(this).removeClass('formhover'); $(this).find('.coledittools').hide(); }
        );
    $('.editable').hover(
                    function () { $(this).addClass('formhover'); $(this).find('.edittools').show(); },
                    function () { $(this).removeClass('formhover'); $(this).find('.edittools').hide(); }
                    );

    $('.coltools').click(
        function () {
            var div = $(this).parent();
            ShowEditor(div);
        }
    );
    $('.edithandle').click(
        function () {
            var div = $(this).parent().parent();
            ShowEditor(div);            
        }
    );
}

function ShowEditor(edittarget) {
    var targetid = edittarget.attr('id').replace('part', '');
    $('<div />').addClass('editoroverlay').appendTo('body').show();
    $("#flexpageediting").html(targetid);
    $('#editorform').html('<input type="hidden" name="partaction" value="showeditor" />');
    var destination = $('#flexjsonurl').html() + '/' + targetid;
    $('#editorform').attr('action', destination);    
    SubmitEditor();
    $('.editormodal').show();    
}


// Modal Popup Code
function CloseDialog() {
    $('#flexpageediting').html('');
    $('.editoroverlay').remove();
    $('.editormodal').hide();
    $('#editorform').unbind();
    return true;
}

$(document).keyup(function(e) {
 if (e.keyCode == 27) { CloseDialog(); }
});


function BindDeleteClicks() {
    $('.deletepart').unbind();
    $('.deletecols').unbind();

    $('.deletepart').click(function () {
        var lnk = $(this);
        var slider = lnk.parent().parent();
        var delid = lnk.attr('id');
        delid = delid.replace('dp', '')

        var url = $('#flexjsonurl').html();
        url += "/" + delid;
        $.post(url,
              { "partaction": "deletepart" },
              function (data) {
                  slider.slideUp('slow', function () {
                      slider.remove();
                      DisplayPlaceholder();
                  });
              },
              "json"
              );

        return false;
    });
    $('.deletecols').click(function () {
        var lnk = $(this);
        var slider = lnk.parent().parent().parent();
        var delid = lnk.attr('id');
        delid = delid.replace('dp', '')

        var url = $('#flexjsonurl').html();
        url += "/" + delid;
        $.post(url,
              { "partaction": "deletepart" },
              function (data) {
                  slider.slideUp('slow', function () {
                      slider.remove();
                      DisplayPlaceholder();
                  });
              },
              "json"
              );

        return false;
    });

}


function BindDroppable() {

    $('.droppable').droppable({
        accept: ".dragpart",
        greedy: true,
        hoverClass: 'droppart-hover',
        activeClass: 'droppart-dragging',
        tollerance: 'pointer',
        drop: function (event, ui) {
            var droptargetid = $(this).attr('id');
            var dragid = ui.draggable.attr('id');

            var h = $(this).html();
            var output = $(this);

            var url = $('#flexjsonurl').html();
            url += "/" + droptargetid.replace('part', '');
            $.post(
                url,
                { "partaction": "addpart", "parttype": dragid },
                function (data) {
                    $('.editplaceholder').remove();
                    output.append(data.ResultHtml);                   
                    BindEverything();
                },
                "json"
             );
        }
    });

}

function BindDraggable() {
    $(".dragpart").draggable({
        helper: "clone",
        revert: "invalid"
    });    
}

function BindCancelFormButtons() {
    $('[name="canceleditbutton"]').unbind();

    $('[name="canceleditbutton"]').click(function () {
        $('.editactionhidden').val('canceledit');
    });
}


function DisplayPlaceholder() {
    if ($('.editable').size() < 1) {
        $('.grid_12 > .droppable').append('<div class="editplaceholder">Drag and Drop Parts to add Content to this Page</div>');        
    }
}

function BindEverything() {
    BindJsonForms();
    BindDeleteClicks();
    BindDraggable();
    BindDroppable();
    BindCancelFormButtons();
    BindEditable();
    BindSortable();
    $('.edittools').hide();
    $('.coledittools').hide();
}

function BindSortable() {
    $(".droppable").sortable({
        items: '.issortable',
        handle: '.sorthandle',
        revert: true,
        connectWith: '.droppable'
    });  
}

function ImageWasUploaded(resultMessage) {
    var data = jQuery.parseJSON(resultMessage);
    if (data.success == "1") {
        $('#uploadimagepreview').attr('src', data.imageurl);
        $('#uploadedfilename').val(data.filename);
    }
}


    function onSilverlightError(sender, args) {
      var appSource = "";
      if (sender != null && sender != 0) {
        appSource = sender.getHost().Source;
      }

      var errorType = args.ErrorType;
      var iErrorCode = args.ErrorCode;

      if (errorType == "ImageError" || errorType == "MediaError")
      {
        return;
      }

      var errMsg = "Unhandled Error in Silverlight Application "
      + appSource + "\n";

      errMsg += "Code: " + iErrorCode + "  \n";
      errMsg += "Category: " + errorType + "    \n";
      errMsg += "Message: " + args.ErrorMessage + "   \n";

      if (errorType == "ParserError") {
        errMsg += "File: " + args.xamlFile + "   \n";
        errMsg += "Line: " + args.lineNumber + "   \n";
        errMsg += "Position: " + args.charPosition + "   \n";
      }
      else if (errorType == "RuntimeError") {

if (args.lineNumber != 0) {
          errMsg += "Line: " + args.lineNumber + "   \n";
          errMsg += "Position: " + args.charPosition + "   \n";
        }
        errMsg += "MethodName: " + args.methodName + "   \n";
      }

      throw new Error(errMsg);
    }  


$(document).ready(function () {

      

    // Popup Close
    $('#editorclose').click(function () {
        CloseDialog();
        return false;
    });

    $('#editorclose2').click(function () {
        CloseDialog();
        return false;
    });

    BindEverything();
    CloseDialog();
    DisplayPlaceholder();

});