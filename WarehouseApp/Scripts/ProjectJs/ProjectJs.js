$(function () {
    $('.navbar-toggle-sidebar').click(function () {
        $('.navbar-nav').toggleClass('slide-in');
        $('.side-body').toggleClass('body-slide-in');
        $('#search').removeClass('in').addClass('collapse').slideUp(200);
    });

    $('#search-trigger').click(function () {
        $('.navbar-nav').removeClass('slide-in');
        $('.side-body').removeClass('body-slide-in');
        $('.search-input').focus();
    });
});

function nextVersion() {
    $.alert("This option will be available in next version.", '<i class="fa fa-exclamation-circle" aria-hidden="true"> Alert</i>');

}
//calculating discount====================================================
function DiscountCalculator(discountAmount, discountType, amount)
        {
    
    var discountAmountWhenPercent = parseFloat(amount * (discountAmount / 100)).toFixed(2);
    var discountValue = discountType == "Flat" ? discountAmount : discountAmountWhenPercent;
    var discountValueShow = discountType == "Flat" ? discountAmount + " " : discountAmountWhenPercent + " (" + discountAmount + "%)";
    return {
        discountValue: parseFloat(discountValue).toFixed(2),
        discountValueShow: discountValueShow
    };
}

function discoutAmountValidation(discountType, discount, price) {
    if (discountType == "Percent") {
        if (parseFloat(discount) > 100) {
            return false;
        } else {
            return true;
        }
    }
    else if (discountType == "Flat") {
        if (parseFloat(discount) > price) {
            return false;
        } else {
            return true;
        }
    }

}

//deleting row============================================
function deleteRow($obj, url, paramData, method) {
    //if (confirm('Do you really want to delete this item?')) {
        method = method == null ? "POST" : method;
        $.confirm({
            title: '<i class="fa fa-exclamation-circle"></i> Confirmation',
            content: 'Are you sure want to Delete this item? ',
            confirmButton: "Yes",
            cancelButton: "No",
            confirm: function() {
                $.ajax({
                    url: url,
                    type: method,
                    data: { id: paramData }
                }).done(function (data) {
                    //alert(data.Result);
                    if (data.Result == "OK") {
                        $obj.closest('tr').remove();
                    } else if (data.Result.Message) {
                        alert(data.Result.Message);
                    }
                }).fail(function() {
                    $.alert("There is something wrong! Please try again.", '<i class="fa fa-exclamation-circle" aria-hidden="true"> Alert</i>');
                });

            },
            cancel: function() {

            }

        });
    
}

// DatePicker setting--------------------------------------------------------------
// Jquery Date Picker------------------------

//$(".rfwDatepicker").datepicker({
//    dateFormat: "dd-mm-yy",
//    changeMonth: true,
//    changeYear: true,
   
//    //todayHighlight: true,
//    //language: "en-GB",
//    //weekStart: 7,
//    //daysOfWeekHighlighted: "5",
//    //autoclose: true,
//    //orientation: "bottom auto",
//});

// Bootstrap DateTime Picker---------------------
$('.rfwDatepicker').datetimepicker({
    format: 'DD-MM-YYYY',

});
$(document).on("click", ".rfwDatepickerIcon", function () {
    //console.log($(this).parent().find('.rfwDatepicker'));
    $(this).parent().find('.rfwDatepicker').focus();
});


//required asteric sign for input field=============================================================================
$('input[type=text]').each(function () {
    var req = $(this).attr('data-val-required');
    if (undefined != req) {
        var label = $('label[for="' + $(this).attr('id') + '"]');
        var text = label.text();
        if (text.length > 0) {
            label.append('<span style="color:red"> *</span>');
        }
    }
});


//comma Separate Number=============================================================================
function commaSeparateNumber(val) {
    while (/(\d+)(\d{3})/.test(val.toString())) {
        val = val.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
    }
    return val;
}
function isNonNegativeNumber(number) {
    if (number < 0 || isNaN(number) || number == '') {
        return false;
    }
    return true;
}



/*Function for product autocomplete****Shop*******************************************************************************/
function AutoCompleteProductName(selector, url, paramData, method) {

    method = (method == undefined || method == '') ? "GET" : method;
    //paramData = (paramData == undefined || paramData == '') ? { term: $(selector).val() } : { term: $(selector).val(), id: paramData };

    $(selector).autocomplete({
        source: function (request, response) {
            $.ajax({
                url: url,
                type: method,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: (paramData != undefined || paramData != '' || paramData>0) ? { term: $(selector).val(), id: paramData } : { term: $(selector).val() },
                success: function (data) {
                    response($.map(data, function (value, index) {
             
                        return {
                            //for showing label and value
                            label: value.ProductFullName,
                            value: value.ProductFullName,

                            //for passing value to ui after select
                            ProductId:value.ProductId,
                            ProductFullName: value.ProductFullName,
                            ProductName: value.ProductName,
                            Unit: value.Unit,
                            UnitPrice: value.UnitPrice,
                            stock: value.stock,
                            DiscountAmount: value.DiscountAmount,
                            DiscountType: value.DiscountType,
                            Vat: value.Vat,
                            Flag: value.Flag,
                            ExpiryDate: value.ExpiryDate,
                            Barcode: value.Barcode,
                            DefaultWarehouse: value.DefaultWarehouse
                        };
                    })
                    );
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    $.alert(textStatus + "! please try again", '<i class="fa fa-exclamation-circle" aria-hidden="true"> Alert</i>');
                }
            });
        },
        minLength: 2,
        select: function (event, ui) {
            if (ui.item.Flag == "Purchase") {
                $(this).val('');
               
                addProductToPurchase(ui.item.ProductId, ui.item.ProductName, ui.item.UnitPrice, ui.item.Unit,ui.item.Barcode);

                //$('#SelectedProductId').val(ui.item.ProductId);
                //$('#UnitPrice').val(ui.item.UnitPrice);
                //$('#TotalPrice').val(ui.item.UnitPrice);
                //$('#TotalQuantity').val('');
                //getExpiryDate(ui.item.ProductId, ui.item.ExpiryDate);
                return false;
            }if (ui.item.Flag == "StockIn") {
                $(this).val('');
                addProductToStockIn(ui.item.ProductId, ui.item.ProductName, ui.item.Unit, ui.item.Barcode);
                return false;
            }
           
            if (ui.item.Flag == "Sell") {
                $(this).val('');
                addProductToSell(ui.item.ProductId, ui.item.ProductFullName, ui.item.UnitPrice, ui.item.Unit, ui.item.DiscountAmount, ui.item.DiscountType, ui.item.Vat, ui.item.stock, ui.item.DefaultWarehouse, ui.item.Barcode);
               // $('#SelectedProductId').val(ui.item.ProductId);
               //// $('#UnitPrice').val(ui.item.UnitPrice);
               // $('#TotalPrice').val(ui.item.UnitPrice);
               // $('#FinalPrice').val(0);
               // $('#Stock').val(ui.item.stock);
               // $('#productVatPercent').val(ui.item.Vat);
               // $('#DiscountAmount').val(ui.item.DiscountAmount);               
               // $('input[name=DiscountType]').filter('[value=' + ui.item.DiscountType + ']').attr('checked', true);

               // $('#beforeDisc').text(ui.item.UnitPrice);
               // $('#totalDisc').text(DiscountCalculator(ui.item.DiscountAmount, ui.item.DiscountType, ui.item.UnitPrice).discountValueShow);
               // $('#ProductDiscountedPrice').val(ui.item.UnitPrice-DiscountCalculator(ui.item.DiscountAmount, ui.item.DiscountType, ui.item.UnitPrice).discountValue);
               // $('#UnitPrice').val(ui.item.UnitPrice - DiscountCalculator(ui.item.DiscountAmount, ui.item.DiscountType, ui.item.UnitPrice).discountValue);
               // $('#TotalQuantity').val('');
               // (ui.item.DiscountAmount > 0) ? $('#discountSpan').removeClass('hidden') : $('#discountSpan').addClass('hidden');
                return false;
            }
            if (ui.item.Flag == "Barcode") {
                $(this).val('');
                addProductToGenearateBarcode(ui.item.ProductId, ui.item.ProductFullName);
                return false;
            } if (ui.item.Flag == "PrintBarcode") {
              
                $(this).val('');
                addBarcodeToPrint(ui.item.ProductId, ui.item.ProductName, ui.item.UnitPrice,  ui.item.DiscountAmount, ui.item.DiscountType,ui.item.Barcode);
                return false;
            }if (ui.item.Flag == "Damage") {
               
                $(this).val('');
                addProductToDamage(ui.item.ProductId, ui.item.ProductName, ui.item.DefaultWarehouse);
                return false;
            } if (ui.item.Flag == "ArticleTransfer") {
               
                $(this).val('');
                addProductToArticleTransfer(ui.item.ProductId, ui.item.ProductName, ui.item.DefaultWarehouse);
                return false;
            } if (ui.item.Flag == "ProductTransfer") {
               
                $(this).val('');
                addProductToProductTransfer(ui.item.ProductId, ui.item.ProductName, ui.item.DefaultWarehouse);
                return false;
            }

            if (ui.item.Flag == "Others") {

                $(this).val(ui.item.ProductName);
                $("#SelectedProductId").val(ui.item.ProductId);
                return false;
            }
            
        }
    });

}


/*Function for menuItem autocomplete****Restaurant*******************************************************************************/
function AutoCompleteItemName(selector, url, method) {
    method = method == null ? "GET" : method;
    $(selector).autocomplete({
        source: function (request, response) {
            $.ajax({
                url: url,
                type: method,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: { term: $(selector).val() },
                success: function (data) {
                    response($.map(data, function (value, index) {

                        return {
                            //for showing label and value
                            label: value.ItemName,
                            value: value.ItemName,

                            //for passing value to ui after select
                            MenuItemId: value.MenuItemId,
                            Price: value.Price,
                            ItemCode: value.ItemCode,
                            ItemType: value.ItemType,
                            Flag: value.Flag
                        
                        };
                    })
                    );
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    $.alert(textStatus + "! please try again", '<i class="fa fa-exclamation-circle" aria-hidden="true"> Alert</i>');
                }
            });
        },
        minLength: 2,
        select: function (event, ui) {
            if (ui.item.Flag == "Including") {
           
                $(selector).parent().find('.selectedItemId').val(ui.item.MenuItemId);
                
            }

        }
    });

}


//display and hide progress popup message in submit======================================================
function DisplayProgressMessage(ctl, msg) {
    $(ctl).prop("disabled", true).html('<i class="fa fa-spinner fa-lg fa-spin"></i> '+msg);
    $("body").addClass("submit-progress-bg");
    setTimeout(function () {
        $(".submit-progress").removeClass("hidden");
    }, 1);
    return true;
}

function HideProgressMessage(ctl, msg) {
    $(ctl).prop("disabled", false).html(msg);
    $(".submit-progress").addClass("hidden");
    $("body").removeClass("submit-progress-bg");

    return true;
}

function getExpiryDate(productId, expiryDateString) {
    if (expiryDateString != null) {
        $('#ExpiryDate').val(formatDateString(expiryDateString, "-"));
    } else {
        $('#ExpiryDate').val('');
    }
}


function rearrangeNameSuffix(selector) {

    var count = 0;
    $(selector).find('tr').each(function () {
       
        var suffix = $(this).find(':input:first').attr('name').match(/\d+/);
        $.each($(this).find(':input'), function (i, val) {
            // Replaced Name
            var oldN = $(this).attr('name');
            var newN = oldN.replace('[' + suffix + ']', '[' + count + ']');
            $(this).attr('name', newN);

        });

        count++;
    });
}
//==================== function for converting form serializedArray data to object==================
function objectifyForm(formArray) {//serialize data function

    var returnArray = {};
    for (var i = 0; i < formArray.length; i++) {
        returnArray[formArray[i]['name']] = formArray[i]['value'];
    }
    return returnArray;
}
// function for js date formate from json date string--------------------------------------
function formatDateString(date, symbol) {
    var monthShortNames = new Array();
    monthShortNames[0] = "Jan";
    monthShortNames[1] = "Feb";
    monthShortNames[2] = "Mar";
    monthShortNames[3] = "Apr";
    monthShortNames[4] = "May";
    monthShortNames[5] = "Jun";
    monthShortNames[6] = "Jul";
    monthShortNames[7] = "Aug";
    monthShortNames[8] = "Sep";
    monthShortNames[9] = "Oct";
    monthShortNames[10] = "Nov";
    monthShortNames[11] = "Dec";
    var monthNames = new Array();
    monthNames[0] = "January";
    monthNames[1] = "February";
    monthNames[2] = "March";
    monthNames[3] = "April";
    monthNames[4] = "May";
    monthNames[5] = "June";
    monthNames[6] = "July";
    monthNames[7] = "August";
    monthNames[8] = "September";
    monthNames[9] = "October";
    monthNames[10] = "November";
    monthNames[11] = "December";

    var dateString = date.substr(6);
    var currentTime = new Date(parseInt(dateString));

    var month = '' + (currentTime.getMonth() + 1),
    //var month = monthShortNames[currentTime.getMonth()],
     day = '' + currentTime.getDate(),
     year = currentTime.getFullYear();
    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;
    return [day, month, year].join(symbol);
}

// Jquery confirm and alert setting =========================================================================
jconfirm.defaults = {
    title: ' ',
    content: 'Are you sure to continue?',
    icon: '',
    confirmButton: 'Ok',
    cancelButton: 'Cancel',
    confirmButtonClass: 'btn-default',
    cancelButtonClass: 'btn-primary',
    theme: 'white',
    animation: 'none',
    animationSpeed: 400,
    animationBounce: 1.5,
    keyboardEnabled: true,
    container: 'body',
    confirm: function () {
    },
    cancel: function () {
    },
    backgroundDismiss: true,
    autoClose: false,
    closeIcon: true,
    closeIconClass: 'fa fa-close'
};


//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : December 2016
//=======================================================================================//