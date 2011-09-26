﻿function CheckChanged() {
    var chk = $('#chkBillSame');
    if (chk.attr('checked')) {
        $('#billingwrapper').hide();
    }
    else {
        $('#billingwrapper').show();
    }
}

function ClearTotals() {
    $('#totalsastable').html(' -- ');
}

function CleanCC() {
    var notclean = $('#cccardnumber').val();
    $.post('~/checkout/CleanCreditCard',
   { "CardNumber": notclean },
    function (data) {
        $('#cccardnumber').val(data.CardNumber);
    },
   "json");
}

function IsEmailKnown() {
    var emailfield = $('#customeremail').val();
    $.post('~/checkout/IsEmailKnown',
            { "email": emailfield },
            function (data) {
                if (data.success == "1") {
                    $('#loginform').show();
                    $('.logincontrolemailfield').val(emailfield);
                }
                else {
                    $('#loginform').hide();
                }
            },
            "json");
}
function IsEmpty(input) { if (input.length > 0) { return false; } return true; }

function IsShippingValid() {
    if (IsEmpty($('#shippingfirstname').val())) { return false; }
    if (IsEmpty($('#shippinglastname').val())) { return false; }
    if (IsEmpty($('#shippingaddress').val())) { return false; }
    if (IsEmpty($('#shippingcity').val())) { return false; }

    var ziptest = $('#shippingzip').val();
    //if (ziptest.length < 5) { return false; }
    if (ziptest.length < 1) { return false; }
    return true;
}

function RefreshShipping() {

    $('#lstShipping').html('');
    $('.shipping-changing').show();
    $('#shipping-needs-refresh').hide();
    $('#shipping-not-valid').hide();

    $.ajax(
                        { type: "POST",
                            url: '~/estimateshipping/GetRatesAsRadioButtons',
                            data: {
                                country: $('#shippingcountryname :selected').val(),
                                firstname: $('#shippingfirstname').val(),
                                lastname: $('#shippinglastname').val(),
                                address: $('#shippingaddress').val(),
                                city: $('#shippingcity').val(),
                                zip: $('#shippingzip').val(),
                                state: $('#shippingstate :selected').val(),
                                orderid: $('#orderbvin').val()
                            },
                            dataType: "json",
                            success: function (data) {
                                $('.shipping-changing').hide();
                                $('#lstShipping').html(data.rates);
                                $('#lstShipping').show();
                                $('#totalsastable').html(data.totalsastable);
                                BindShippingRadioButtons();
                            },
                            error: function (data) {                                
                                $('.shipping-changing').hide();
                                $('#shipping-not-valid').show();
                                ClearTotals();
                            }
                        });
}

function ShippingAddressChanged() {

    $('#lstShipping').html('');
    ClearTotals();
    
    if (IsShippingValid()) {
        RefreshShipping();
        $('#shipping-not-valid').hide();
        }
    else {
        $('#shipping-not-valid').show();
    }
    return true;
}

function ApplyShippingMethod(methodid) {
    ClearTotals();
    $('.shipping-changing').show();

    var orderid = $('#orderbvin').val();

    $.ajax({ type: 'POST',
        url: '~/checkout/applyshipping',
        data: { MethodId: methodid,
            OrderId: orderid
        },
        dataType: "json",
        success: function (data) {
            $('.shipping-changing').hide();
            $('#totalsastable').html(data.totalsastable);
        },
        error: function (data) {
            ClearTotals();
        }
    });
}

function ApplyCurrentShippingRate() {
    var rateKey = $("input[name='shippingrate']:checked").val();
    //alert(' current unique key = ' + rateKey);
    ApplyShippingMethod(rateKey);
}

function BindStateDropDownLists() {    
    $('#shippingstate').change(
    function () { ShippingAddressChanged(); }
    );
}

function BindShippingRadioButtons() {
    $("input[name = 'shippingrate']").change(
    function () { ApplyCurrentShippingRate(); }
    );
}

      function LoadRegionsWithSelection(regionlist, countryid, selectedregion) {
            $.post('~/estimateshipping/getregions/' + countryid,
          { "regionid": selectedregion },
          function (data) {
              regionlist.html(data.Regions);
              $('#TempShippingRegion').val('');
              $('#TempBillingRegion').val('');
              BindStateDropDownLists();
          },
         "json"
         );
      } 
      


function CloseDialog() {
    $('.overlay').remove();
    $('.modal').hide();
    }

function OpenDialog(lnk) {
    $('<div />').addClass('overlay').appendTo('body').show();
    $('.modal').show();
    var loadid = $(lnk).attr('href');
    //LoadDialog(loadid.replace('edit', ''));
    $('#popoverpage').attr('src', loadid);
    }

    

// Document Ready Function
    $(document).ready(function () {

        BindStateDropDownLists();

        $('#chkBillSame').click(function () { CheckChanged(); return true; });
        $('#cccardnumber').change(function () { CleanCC(); });
        $('#billingcountryname').change(function () {
            LoadRegionsWithSelection($('#billingstate'), $('#billingcountryname option:selected').val(), $('#TempBillingRegion').val());
        });
        $('#shippingcountryname').change(function () {
            LoadRegionsWithSelection($('#shippingstate'), $('#shippingcountryname option:selected').val(), $('#TempShippingRegion').val());
            ShippingAddressChanged();
        });
        // Trigger First Change
        LoadRegionsWithSelection($('#shippingstate'), $('#shippingcountryname option:selected').val(), $('#TempShippingRegion').val());

        $('#shippingregionname').change(function () { ShippingAddressChanged(); });
        $('#shippingfirstname').change(function () { ShippingAddressChanged(); });
        $('#shippinglastname').change(function () { ShippingAddressChanged(); });
        $('#shippingaddress').change(function () { ShippingAddressChanged(); });
        $('#shippingcity').change(function () { ShippingAddressChanged(); });
        $('#shippingzip').change(function () { ShippingAddressChanged(); });
        //BindShippingRadioButtons();    
        CheckChanged();

        ShippingAddressChanged();

        // Email Field
        $('#customeremail').change(function () { IsEmailKnown(); });
        IsEmailKnown();

        // Popup Close
        $("#dialogclose").click(function () {
            CloseDialog();
            return false;
        });

        // Popup Open
        $(".popover").click(function () {
            OpenDialog($(this));
            return false;
        });

        CloseDialog();
    });          // End Document Ready
        

