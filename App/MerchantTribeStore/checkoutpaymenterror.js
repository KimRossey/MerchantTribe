function CleanCC() {
    var notclean = $('#cccardnumber').val();
    $.post('~/checkout/CleanCreditCard',
   { "CardNumber": notclean },
    function (data) {
        $('#cccardnumber').val(data.CardNumber);
    },
   "json");
}

function LoadRegions(countrylist, regionlist, countryname) {
    $.post('~/estimateshipping/getregions/' + countryid,
          { "regionid": '' },
          function (data) {
              regionlist.html(data.Regions);
          },
         "json"
         );
}

// Document Ready Function
$(document).ready(function () {

    $('#cccardnumber').change(function () { CleanCC(); });
    $('#billingcountryname').change(function () {
        LoadRegions($('#billingcountryname'), $('#billingstate'), $('#billingcountryname option:selected').val());
    });

});        // End Document Ready
        

