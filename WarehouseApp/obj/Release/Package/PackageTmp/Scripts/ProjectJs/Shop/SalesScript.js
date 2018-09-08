function calculateTotalPrice() {
    var unitPrice = isNonNegativeNumber($('#UnitPrice').val().trim()) == false ? 0 : parseFloat($('#UnitPrice').val().trim());
    var productDiscountedPrice = parseFloat($("#ProductDiscountedPrice").val().trim());
    var totalQuantity = isNonNegativeNumber($("#TotalQuantity").val().trim()) == false ? 0 : parseInt($("#TotalQuantity").val().trim());

    var totalPriceBeforeDiscount = (unitPrice * totalQuantity);

    $('#TotalPrice').val(parseFloat(totalPriceBeforeDiscount).toFixed(2));

    var totalPriceWithdiscount = parseFloat(productDiscountedPrice * totalQuantity).toFixed(2);

    $("#FinalPrice").val(parseFloat(totalPriceBeforeDiscount).toFixed(2));

    //Calculate productwise vat =============================================================

    var productVatPercent = $("#productVatPercent").val();
    var productVatAmount = parseFloat(totalPriceWithdiscount * (productVatPercent / 100)).toFixed(2);
    $("#productVatAmount").val(parseFloat(productVatAmount).toFixed(2));


}


function totalPriceProduct(unitPrice, pices) {
    var value = pices * unitPrice;
    $('#TotalPrice').val(parseFloat(value).toFixed(2));
}

//function calculateAllPrice() {
//    var UnitPrice, TotalQuantity, TotalPrice, DiscountAmount, DiscountType, ProductDiscountedPrice, FinalPrice;

//    UnitPrice = isNonNegativeNumber($('#UnitPrice').val().trim()) == false ? 0 : parseFloat($('#UnitPrice').val().trim());
//    TotalQuantity = isNonNegativeNumber($('#TotalQuantity').val().trim()) == false ? 0 : parseInt($('#TotalQuantity').val().trim());
//    TotalPrice = UnitPrice * TotalQuantity;
//    $('#TotalPrice').val(TotalPrice);

//    DiscountAmount = isNonNegativeNumber($('#DiscountAmount').val().trim()) == false ? 0 : parseFloat($('#DiscountAmount').val().trim());
//    DiscountType = $('input[name=DiscountType]:checked').val();
//    ProductDiscountedPrice = DiscountCalculator(DiscountAmount, DiscountType, TotalPrice).discountValue;
//    FinalPrice = parseFloat(TotalPrice).toFixed(2) - parseFloat(ProductDiscountedAmount).toFixed(2);
//    $('#ProductDiscountedPrice').val(ProductDiscountedPrice);
//    $('#FinalPrice').val(FinalPrice);
//}
function calculateDiscount() {
    var unitPrice = isNonNegativeNumber($('#UnitPrice').val().trim()) == false ? 0 : parseFloat($('#UnitPrice').val().trim());
    var discountAmount = isNonNegativeNumber($("#DiscountAmount").val().trim()) == false ? 0 : parseFloat($("#DiscountAmount").val().trim());
    var discountType = $('[name="DiscountType"]:checked').val();
    var productDiscountedPrice = parseFloat($("#ProductDiscountedPrice").val().trim());


    if (discoutAmountValidation(discountType, discountAmount, unitPrice) == false) {
        alert("Invalid Discount");
        $("#DiscountAmount").val(0);
        discountAmount = 0;
    }
    var totalDiscount = DiscountCalculator(discountAmount, discountType, unitPrice).discountValue;
    productDiscountedPrice = isNaN(unitPrice - totalDiscount) == true ? parseFloat(unitPrice).toFixed(3) : parseFloat(unitPrice - totalDiscount).toFixed(3);

    $("#ProductDiscountedPrice").val(parseFloat(productDiscountedPrice).toFixed(2));

    calculateTotalPrice();
}

function TotalFinalTotal() {
    var sum = 0;
    var vatSum = 0;
    $('.TotalProductPrice').each(function () {
        sum += parseFloat($(this).text());
    });
    $('.totalVat').each(function () {
        vatSum += parseFloat($(this).text());
    });
   

    $('#FinalTotal').text(parseFloat(sum).toFixed(2));

    $('#totalVatAmount').val(vatSum.toFixed(2));

   
    GrandTotal();
}

function CalculateTotal() {
    var sum = 0;
    var vatSum = 0;
    $("#productTable tbody").find(".TotalProductPrice").each(function () {
        sum += parseFloat($(this).val());
    });
    $("#productTable tbody").find(".TotalProductVat").each(function () {
        vatSum += parseFloat($(this).val());
    });
    //console.log('Sum:' + sum + " vat:" + vatSum);
    $('#footerTotal').text(parseFloat(sum).toFixed(2));
    $('#TotalPrice').val(parseFloat(sum).toFixed(2));
    $('#TotalVat').val(vatSum.toFixed(2));
    CalculateGrandTotal();
}

function GrandTotal() {
    var UnitPrice, TotalQuantity, FinalTotal, TotalDiscountAmount, TotalDiscountType, invoiceDiscountedAmount, FinalValue, totalVatCalc, grandTotal;

    FinalTotal = parseFloat($('#FinalTotal').text());

    TotalDiscountAmount = isNonNegativeNumber($('#TotalDiscountAmount').val().trim()) == false ? 0 : parseFloat($('#TotalDiscountAmount').val().trim());
    TotalDiscountType = $('input[name=TotalDiscountType]:checked').val();
    invoiceDiscountedAmount = DiscountCalculator(TotalDiscountAmount, TotalDiscountType, FinalTotal).discountValue;
    FinalValue = parseFloat(FinalTotal).toFixed(2) - parseFloat(invoiceDiscountedAmount).toFixed(2);

    $('#totalDiscountedAmount').val(parseFloat(invoiceDiscountedAmount).toFixed(2));
    $('#afterDiscountAmount').val(parseFloat(FinalValue).toFixed(2));

    //var vatPercent = parseFloat($("#VatPercentage").val());
    //var totalVat = parseFloat(FinalValue * (vatPercent / 100)).toFixed(2);
   // $("#totalVatAmuont").val(totalVat);
    totalVatCalc = parseFloat($('#totalVatAmount').val());

    grandTotal = parseFloat(FinalValue + totalVatCalc).toFixed(2);
    $('#FinalValue').val(parseFloat(grandTotal).toFixed(2));
    $('#CashPaid').val(parseFloat(grandTotal).toFixed(2));
}

//New--------------------------------------
function CalculateGrandTotal() {
    var TotalDiscountAmount, TotalDiscountType, invoiceDiscountedAmount, TotalPrice, FinalTotal, totalVatCalc, grandTotal;

    TotalPrice = parseFloat($('#TotalPrice').val());
    TotalDiscountAmount = isNonNegativeNumber($('#DiscountAmount').val().trim()) == false ? 0 : parseFloat($('#DiscountAmount').val().trim());
    TotalDiscountType = $('input[name=DiscountType]:checked').val();
    totalVatCalc = parseFloat($('#TotalVat').val());
    //console.log(TotalPrice, TotalDiscountAmount, TotalDiscountType, totalVatCalc);
    //discount validation checking
    if (discoutAmountValidation(TotalDiscountType, TotalDiscountAmount, TotalPrice) == false) {
        $.alert("Invalid Discount!.", '<i class="fa fa-exclamation-circle" aria-hidden="true"> Alert</i>');
        $("#DiscountAmount").val(0);
        TotalDiscountAmount = 0;
    }

    invoiceDiscountedAmount = DiscountCalculator(TotalDiscountAmount, TotalDiscountType, TotalPrice).discountValue;
    FinalTotal = parseFloat(TotalPrice).toFixed(2) - parseFloat(invoiceDiscountedAmount).toFixed(2);
   
    grandTotal = parseFloat(FinalTotal + totalVatCalc).toFixed(2);
    $('#grandTotal').val(parseFloat(grandTotal).toFixed(2));

    var paid = isNonNegativeNumber($('#PaidAmount').val().trim()) == false ? 0 : parseFloat($('#PaidAmount').val().trim());
    //$('#CashPaid').val(parseFloat(grandTotal).toFixed(2));  //due not allow for retail sale 
    $('#CashPaid').val(parseFloat(paid).toFixed(2));    //due allowed for retail sale 
    
    // $('#PaidAmount').val(parseFloat(grandTotal).toFixed(2));
    $('#DueAmount').val(parseFloat(grandTotal - paid).toFixed(2));

}

function addProductForSale() {
    var productName = $('#Product').val();
    var productId = $('#SelectedProductId').val();
    var pices = isNonNegativeNumber($("#TotalQuantity").val().trim()) == false ? 0 : parseFloat($("#TotalQuantity").val().trim());
    var unitPrice = isNonNegativeNumber($("#UnitPrice").val().trim()) == false ? 0 : parseFloat($("#UnitPrice").val().trim());
    var totalPrice = $('#TotalPrice').val();
    var unitPriceBeforeDisc = $('#beforeDisc').text();
    var beforeDisc = $('.discountSpanInput').text();
    var DiscountAmount = parseFloat($('#DiscountAmount').val());
    var DiscountType = $('input[name=DiscountType]:checked').val();
    var FinalPrice = parseFloat($('#FinalPrice').val()).toFixed(2);
    
 
    //var beforeDiscountUnitPrice = (DiscountAmount > 0) ? " (Before " + DiscountCalculator(DiscountAmount, DiscountType, beforeDisc).discountValueShow + " Discount: " + $('#beforeDisc').text() + ")" : " ";
    var beforeDiscountUnitPrice = (DiscountAmount > 0) ? " (" + beforeDisc+")" : " ";

    var vatPercent = parseFloat($("#VatPercentage").val());
    var totalVat = parseFloat($("#totalVat").val());

    var productVatAmount = parseFloat($("#productVatAmount").val()).toFixed(2);
    var remainingStocks = parseFloat($("#Stock").val());
    var totalVatString = totalVat + " (" + vatPercent + "%)";
    if (productId == '' || productName == '' || unitPrice == '' || pices == '') {
        if (productId == '') {
            $.alert("Invalid product entered", '<i class="fa fa-exclamation-circle" aria-hidden="true"> Alert</i>');
        } else {
            $.alert("Please enter all required field value", '<i class="fa fa-exclamation-circle" aria-hidden="true"> Alert</i>');
        }
       

    } else {
        if (pices > remainingStocks) {
            $.alert("Given quantity not available in stock", '<i class="fa fa-exclamation-circle" aria-hidden="true"> Alert</i>');

        }
        else {
       
            var str = '';
            //str += '<tr><td class="hidden productId">' + productId + '</td><td class="col-md-3">' + productName + '</td><td class="col-md-1 unitPrice">' + unitPrice + '</td><td class="col-md-1 pices">' + pices + '</td><td class="col-md-1"><span class="totalprice">' + totalPrice + '</span></td><td class="col-md-2"><span class="hidden productDiscountType">' + DiscountType + '</span><span class="hidden productDiscount">' + DiscountAmount + '</span>' + DiscountCalculator(DiscountAmount, DiscountType, totalPrice).discountValueShow + '</td><td class="col-md-1"><span class="finalTotalPrice">' + FinalPrice + '</span></td><td class="col-md-2"><span class="totalVat hidden">' + totalVat + '</span>' + totalVatString + '</td><td class="col-md-1"><span class="btn btn-xs btn-danger remove">Remove</span></td></tr>';
            str += '<tr><td class="hidden productId">' + productId + '</td><td class="col-md-3">' + productName + '</td><td class="col-md-2"><span class="unitPrice">' + unitPrice + '</span> <span>' + beforeDiscountUnitPrice + '</span></td><td class="col-md-1 pices">' + pices + '</td><td class="hidden col-md-1"><span class=" totalprice">' + totalPrice + '</span></td><td class="hidden col-md-1"><span class="hidden productDiscountType">' + DiscountType + '</span><span class="hidden productDiscount">' + DiscountAmount + '</span>' + (DiscountCalculator(DiscountAmount, DiscountType, unitPriceBeforeDisc).discountValue * pices) + '</td><td class="col-md-1"><span class="finalTotalPrice">' + FinalPrice + '</span></td><td class="hidden col-md-2"><span class="totalVat hidden">' + productVatAmount + '</span>' + totalVatString + '</td><td class="col-md-1"><span class="btn btn-xs btn-danger remove">Remove</span></td></tr>';
            $('#productTable').append(str);
            $('.remove').on('click', function () {
                $(this).closest('tr').remove();

                remainingStocks = remainingStocks + parseFloat($(this).closest('tr').find('.pices').text().trim());
                $('#Stock').val(remainingStocks);
                TotalFinalTotal();
            });

            TotalFinalTotal();
            remainingStocks = remainingStocks - pices;
            $('#Stock').val(remainingStocks);
        }
    }
}

// New----------------------
function addProductToSell(productId, productFullName, unitPrice, unit, productDiscount, productDiscountType, productVatPercent, stock, defaultWarehouse, barcode) {

    barcode = (barcode == undefined || barcode == '') ? '' : barcode;
    var uniqueProductId = (barcode == undefined || barcode == '') ? productId : productId + "-" + barcode;
    //console.log(stock);
    var productExist = $("#productTable tbody").find(".productId-" + uniqueProductId).val();
    if (productExist == undefined || productExist == "") {
        //if (stock < 1) {
        //    $.alert( "Stocks unavailable !", '<i class="fa fa-exclamation-circle" aria-hidden="true"> Alert</i>');
        //}
        //else{
        var discountedProductPrice = unitPrice - DiscountCalculator(productDiscount, productDiscountType, unitPrice).discountValue;
        var quantity = 1;
        var totalProductPrice = parseFloat(discountedProductPrice * quantity).toFixed(2);
        var totalProductVat = parseFloat(totalProductPrice * (productVatPercent/100)).toFixed(2);
        var productRowMarkup = '';
   
        productRowMarkup += '<tr><td class="col-md-5 paddingReduce">' 
            + '<input class="productId-' + uniqueProductId + '"  id="InvoiceProducts_' + productId + '_ProductId" name="InvoiceProducts[' + productId + '].ProductId" type="hidden" value="' + productId + '">'
            + '<input class="Barcode" id="InvoiceProducts_' + productId + '_Barcode" name="InvoiceProducts[' + productId + '].Barcode" type="hidden" value="' + barcode + '">'
            + '<input class="defaultWarehouse" id="InvoiceProducts_' + productId + '_DefaultZoneId" name="InvoiceProducts[' + productId + '].DefaultZoneId" type="hidden" value="' + defaultWarehouse + '">'
            + '<input class="ProductDiscount" id="InvoiceProducts_' + productId + '_ProductDiscount" name="InvoiceProducts[' + productId + '].DiscountAmount" type="hidden" value="' + productDiscount + '">'
            + '<input class="ProductDiscountType" id="InvoiceProducts_' + productId + '_ProductDiscountType" name="InvoiceProducts[' + productId + '].DiscountType" type="hidden" value="' + productDiscountType + '">'
            + '<input class="Vat" id="InvoiceProducts_' + productId + '_Vat" name="InvoiceProducts[' + productId + '].Vat" type="hidden" value="' + productVatPercent + '">' + productFullName + '</td>'
            + '<td class="col-md-2 paddingReduce"><input  value="' + discountedProductPrice + '" class="form-control input-sm UnitPrice" id="InvoiceProducts_' + productId + '_UnitPrice" name="InvoiceProducts[' + productId + '].UnitPrice" type="text" onkeyup="updateTotalProductPriceInQuantityChange(this)"></td>'
            +'<td class="col-md-2 paddingReduce">'
            +'<div class="col-md-12 paddingReduce input-group ">' 
            //+ '<input value="' + quantity + '" class="form-control input-sm Quantity" id="InvoiceProducts_' + productId + '_Quantity" name="InvoiceProducts[' + productId + '].Quantity" type="text" onkeyup="checkStockUpdateTotalProductPrice(this)">' // make warning for stock unavailable message 
            + '<input value="' + quantity + '" class="form-control input-sm Quantity" id="InvoiceProducts_' + productId + '_Quantity" name="InvoiceProducts[' + productId + '].Quantity" type="text" onkeyup="updateTotalProductPriceInQuantityChange(this)">'  //do not make warning for stock unavailable message 
            +' <span class="input-group-addon productUnit">' + unit + '</span>'
            + '</div><span class="hidden stock">' + stock + '</span> </td>'
            + '<td class="col-md-2 paddingReduce"><input value="' + totalProductPrice + '" class="form-control input-sm TotalProductPrice" readonly="True" id="InvoiceProducts_' + productId + '_TotalProductPrice" name="InvoiceProducts[' + productId + '].TotalPrice" type="text">'
            + '<input class="TotalProductVat" id="InvoiceProducts_' + productId + '_TotalProductVat" name="InvoiceProducts[' + productId + '].TotalProductVat" type="hidden" value="' + totalProductVat + '"></td>'
            +'<td class="col-md-1 paddingReduce"><span class="btn btn-xs btn-danger remove"><i class="fa fa-times"></i></span></td></tr>';

        $('#productTable').append(productRowMarkup);
    //}
    }
    else {
        var currentUnit = parseFloat($("#productTable tbody").find(".productId-" + uniqueProductId).closest('tr').find(".Quantity").val());
        currentUnit += 1;
        //if (stock < currentUnit) {
        //    $.alert("Stocks unavailable !", '<i class="fa fa-exclamation-circle" aria-hidden="true"> Alert</i>');

        //} else {
            $("#productTable tbody").find(".productId-" + uniqueProductId).closest('tr').find(".Quantity").val(currentUnit);
            var currentUnitPrice = parseFloat($("#productTable tbody").find(".productId-" + uniqueProductId).closest('tr').find(".UnitPrice").val());
            var currentTotalPrice = parseFloat(currentUnit * currentUnitPrice).toFixed(2);
            var currentTotalProductVat = parseFloat(currentTotalPrice * (productVatPercent / 100)).toFixed(2);
            $("#productTable tbody").find(".productId-" + uniqueProductId).closest('tr').find(".TotalProductPrice").val(currentTotalPrice);
            $("#productTable tbody").find(".productId-" + uniqueProductId).closest('tr').find(".TotalProductVat").val(currentTotalProductVat);
        //}
    }
    CalculateTotal();
}
$(document).on("click", ".remove", function () {
    $(this).closest('tr').remove();
    CalculateTotal();
});

function cashPaidChangeEffect() {
    var cashPaid = isNonNegativeNumber($('#CashPaid').val().trim()) == false ? 0 : parseFloat($('#CashPaid').val().trim());
    //var grandValue = parseFloat($('#FinalValue').val());
    var grandValue = parseFloat($('#grandTotal').val());

    var cashChange = parseFloat(cashPaid) - grandValue;
   
    $('#CashChange').val(cashChange.toFixed(2));
}
function paidAmountChangeEffect() {
    var cashPaid = isNonNegativeNumber($('#PaidAmount').val().trim()) == false ? 0 : parseFloat($('#PaidAmount').val().trim());
    var grandValue = parseFloat($('#grandTotal').val());
    var cashChange = grandValue - parseFloat(cashPaid);

    $('#DueAmount').val(cashChange.toFixed(2));
}
function checkStockUpdateTotalProductPrice(obj) {

    var unitPrice = isNonNegativeNumber($(obj).closest("tr").find(".UnitPrice").val().trim()) == false ? 0 : parseFloat($(obj).closest("tr").find(".UnitPrice").val().trim()).toFixed(2);
    var quantity = isNonNegativeNumber($(obj).closest("tr").find(".Quantity").val().trim()) == false ? 0 : parseFloat($(obj).closest("tr").find(".Quantity").val().trim());
    var stock = isNonNegativeNumber($(obj).closest("tr").find(".stock").text()) == false ? 0 : parseFloat($(obj).closest("tr").find(".stock").text());
    if (quantity > stock) {
        $.alert("Stocks unavailable !", '<i class="fa fa-exclamation-circle" aria-hidden="true"> Alert</i>');
        quantity = 1;
        $(obj).closest("tr").find(".Quantity").val(quantity);
    } 
    var totalProductPrice = unitPrice * quantity;
    var productVatPercent = isNonNegativeNumber($(obj).closest("tr").find(".Vat").val().trim()) == false ? 0 : parseFloat($(obj).closest("tr").find(".Vat").val().trim());
    var totalProductVat = parseFloat(totalProductPrice * (productVatPercent / 100)).toFixed(2);
    $(obj).closest("tr").find(".TotalProductPrice").val(totalProductPrice);
    $(obj).closest("tr").find(".TotalProductVat").val(totalProductVat);
    //console.log(unitPrice, quantity, totalProductPrice);
    CalculateTotal();

}
function updateTotalProductPriceInQuantityChange(obj) {

    var unitPrice = isNonNegativeNumber($(obj).closest("tr").find(".UnitPrice").val().trim()) == false ? 0 : parseFloat($(obj).closest("tr").find(".UnitPrice").val().trim()).toFixed(2);
    var quantity = isNonNegativeNumber($(obj).closest("tr").find(".Quantity").val().trim()) == false ? 0 : parseFloat($(obj).closest("tr").find(".Quantity").val().trim());
    
    var totalProductPrice = parseFloat(unitPrice * quantity).toFixed(2);
    var productVatPercent = isNonNegativeNumber($(obj).closest("tr").find(".Vat").val().trim()) == false ? 0 : parseFloat($(obj).closest("tr").find(".Vat").val().trim()).toFixed(2);
    var totalProductVat = parseFloat(totalProductPrice * (productVatPercent / 100)).toFixed(2);
    $(obj).closest("tr").find(".TotalProductPrice").val(totalProductPrice);
    $(obj).closest("tr").find(".TotalProductVat").val(totalProductVat);
    //console.log(unitPrice, quantity, totalProductPrice);
    CalculateTotal();

}

//#region return calculation---
function updateTotalReturnInQuantityChange(obj) {
    
    var unitPrice = isNonNegativeNumber($(obj).closest("tr").find(".UnitPrice").val().trim()) == false ? 0 : parseFloat($(obj).closest("tr").find(".UnitPrice").val().trim()).toFixed(2);
    var returnQuantity = isNonNegativeNumber($(obj).closest("tr").find(".ReturnQty").val().trim()) == false ? 0 : parseFloat($(obj).closest("tr").find(".ReturnQty").val().trim());
    console.log(returnQuantity);
    var quantity = isNonNegativeNumber($(obj).closest("tr").find(".InvoiceProductQuantity").val().trim()) == false ? 0 : parseFloat($(obj).closest("tr").find(".InvoiceProductQuantity").val().trim());
    
    if (returnQuantity > quantity) {
        $.alert("Invalid Input !", '<i class="fa fa-exclamation-circle" aria-hidden="true"> Alert</i>');
        returnQuantity = 0;
        $(obj).closest("tr").find(".ReturnQty").val(returnQuantity);
    }
    var returnAmount = parseFloat(unitPrice * returnQuantity).toFixed(2);
    //var productVatPercent = isNonNegativeNumber($(obj).closest("tr").find(".Vat").val().trim()) == false ? 0 : parseFloat($(obj).closest("tr").find(".Vat").val().trim()).toFixed(2);
    //var totalProductVat = parseFloat(totalProductPrice * (productVatPercent / 100)).toFixed(2);
    $(obj).closest("tr").find(".ReturnAmount").val(returnAmount);
    //$(obj).closest("tr").find(".TotalProductVat").val(totalProductVat);
    //console.log(unitPrice, quantity, totalProductPrice);
    CalculateTotalReturn();

}

function CalculateTotalReturn() {
    var sum = 0;
    var vatSum = 0;
    $("#productTable tbody").find(".ReturnAmount").each(function () {
        sum += parseFloat($(this).val());
    });
    var discAmount = $("#Invoice_DiscountAmount").val().trim();
    var discType = $("#InvoiceDiscountType").val().trim();
    
    var sumWithoutDiscount = sum-DiscountCalculator(discAmount, discType, sum).discountValue;
   
    //$("#productTable tbody").find(".TotalProductVat").each(function () {
    //    vatSum += parseFloat($(this).val());
    //});
    //console.log('Sum:' + sum + " vat:" + vatSum);
    $('#footerTotal').text(parseFloat(sum).toFixed(2));
    $('#TotalReturn').val(parseFloat(sumWithoutDiscount).toFixed(2));
    //$('#TotalVat').val(vatSum.toFixed(2));
   // CalculateGrandTotal();
}
//#endregion ----------------------------------
//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : January 2017
//=======================================================================================//