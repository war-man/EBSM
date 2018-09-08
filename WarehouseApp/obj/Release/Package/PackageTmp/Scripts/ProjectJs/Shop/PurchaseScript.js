


//========================================== Purchase Script=======================================================
function purchaseAddProduct(){
    var purchaseDate = $('#PurchaseDate').val();
    var expiryDate = $('#ExpiryDate').val();
    var productName = $('#Product').val();
    var productId = $('#SelectedProductId').val();
    var pices = isNonNegativeNumber($("#TotalQuantity").val().trim()) == false ? 0 : parseFloat($("#TotalQuantity").val().trim());
    var unitPrice = isNonNegativeNumber($("#UnitPrice").val().trim()) == false ? 0 : parseFloat($("#UnitPrice").val().trim());
    var totalPrice = $('#TotalPrice').val();
    if (productId == ''|| productName == '' || unitPrice == '' || pices == '') {
        if (productId == '') {
            $.alert("Invalid product entered", '<i class="fa fa-exclamation-circle" aria-hidden="true"> Alert</i>');
        } else {
            $.alert("Please enter required field value", '<i class="fa fa-exclamation-circle" aria-hidden="true"> Alert</i>');
        }
    } else {
        var str = '';
        str += '<tr><td class="hidden purchaseDate">' + purchaseDate + '</td><td class="hidden expiryDate">' + expiryDate + '</td><td class="hidden productId">' + productId + '</td><td class="col-md-5">' + productName + '</td><td class="col-md-2 unitPrice">' + unitPrice + '</td><td class="col-md-2 pices">' + pices + '</td><td><span class="col-md-2 totalprice">' + totalPrice + '</span></td><td class="col-md-1"><span class=" btn btn-xs btn-danger remove">Remove</span></td></tr>';
        $('#productTable').append(str);
        $('.remove').on('click', function () {
            $(this).closest('tr').remove();
            finalTotal();
        });
        finalTotal();
    }
}

function finalTotal() {
    var sum = 0;
    $('.totalprice').each(function () {
        sum += parseFloat($(this).text());
    });
    var pcost = isNonNegativeNumber($("#PurchaseCost").val().trim()) == false ? 0 : parseFloat($("#PurchaseCost").val().trim());
    var purchsDiscount = isNonNegativeNumber($('#PurchaseDiscount').val().trim()) == false ? 0 : parseFloat($('#PurchaseDiscount').val().trim());
    var totalWithDiscount = parseFloat(sum).toFixed(2) - purchsDiscount;
    var grandValue = totalWithDiscount + pcost;
    $('#FinalTotal').text(parseFloat(sum).toFixed(2));
    $('#PaidAmount').val(parseFloat(totalWithDiscount).toFixed(2));
    $('#FinalValue').val(parseFloat(grandValue).toFixed(2));
  

    //var fTotal = isNonNegativeNumber($("#FinalTotal").text()) == false ? 0 : parseFloat($("#FinalTotal").text());
    var paidAmount = isNonNegativeNumber($("#PaidAmount").val().trim()) == false ? 0 : parseFloat($("#PaidAmount").val().trim());
    var remaining = totalWithDiscount - paidAmount;
    $('#remaining').val(parseFloat(remaining).toFixed(2));
}

function totalPriceProduct() {
    var unitPrice = isNonNegativeNumber($('#UnitPrice').val().trim()) == false ? 0 : parseFloat($('#UnitPrice').val().trim()).toFixed(2);
    if (unitPrice == '') {
        unitPrice = 0;
    }

    var pices = isNonNegativeNumber($("#TotalQuantity").val().trim()) == false ? 0 : parseFloat($('#TotalQuantity').val().trim());
    if (pices == "") {
        pices = 0;
    }

    var value = pices * unitPrice;
    $('#TotalPrice').val(parseFloat(value).toFixed(2));
}
function purchaseCostChangeEffect() {

    var cost, finalTotal, newfinal;
    var purchsDiscount = isNonNegativeNumber($('#PurchaseDiscount').val().trim()) == false ? 0 : parseFloat($('#PurchaseDiscount').val().trim());

    cost = isNonNegativeNumber($('#PurchaseCost').val().trim()) == false ? 0 : parseFloat($('#PurchaseCost').val().trim());
    finalTotal = parseFloat($('#FinalTotal').text()) - purchsDiscount;
   
    newfinal = parseFloat(finalTotal) + parseFloat(cost);
    $('#FinalValue').val(parseFloat(newfinal).toFixed(2));
    $('#PaidPurchaseCost').val(cost);
    
}

function paidPurchaseCostChangeEffect() {

    var cost, paidPc, finalTotal, newfinal;
    paidPc = isNonNegativeNumber($('#PaidPurchaseCost').val().trim()) == false ? 0 : parseFloat($('#PaidPurchaseCost').val().trim());
    cost = isNonNegativeNumber($("#PurchaseCost").val().trim()) == false ? 0 : parseFloat($("#PurchaseCost").val().trim());
    if (paidPc > cost) {
        $.alert("Invalid Amount.", '<i class="fa fa-exclamation-circle" aria-hidden="true"> Alert</i>');
        $(this).val(0);
    }
    $('#remainingPc').val(cost-paidPc);
}
function paidAmountChangeEffect() {
    var paidAmount, finalTotal, remaining,purchsDiscount;
    paidAmount = isNonNegativeNumber($('#PaidAmount').val().trim()) == false ? 0 : parseFloat($('#PaidAmount').val().trim());
    purchsDiscount = isNonNegativeNumber($('#PurchaseDiscount').val().trim()) == false ? 0 : parseFloat($('#PurchaseDiscount').val().trim());
    finalTotal = $('#FinalTotal').text();
    remaining = (parseFloat(finalTotal) - purchsDiscount) - parseFloat(paidAmount);
    $('#remaining').val(parseFloat(remaining).toFixed(2));
}
function purchaseDiscountChangeEffect()
{
  var totalProductPrice = $('#FinalTotal').text();
    var pcost = isNonNegativeNumber($("#PurchaseCost").val().trim()) == false ? 0 : parseFloat($("#PurchaseCost").val().trim());
    var purchsDiscount = isNonNegativeNumber($('#PurchaseDiscount').val().trim()) == false ? 0 : parseFloat($('#PurchaseDiscount').val().trim());
    var totalWithDiscount = parseFloat(totalProductPrice).toFixed(2) - purchsDiscount;
    var grandValue = totalWithDiscount + pcost;
    $('#PaidAmount').val(parseFloat(totalWithDiscount).toFixed(2));
    $('#FinalValue').val(parseFloat(grandValue).toFixed(2));


    //var fTotal = isNonNegativeNumber($("#FinalTotal").text()) == false ? 0 : parseFloat($("#FinalTotal").text());
    var paidAmount = isNonNegativeNumber($("#PaidAmount").val().trim()) == false ? 0 : parseFloat($("#PaidAmount").val().trim());
    var remaining = totalWithDiscount - paidAmount;
    $('#remaining').val(parseFloat(remaining).toFixed(2));
}


// New----------------------
function addProductToPurchase(productId, productFullName, unitPrice, unit, barcode) {
    //console.log(barcode);
    barcode = (barcode == undefined || barcode=='') ? '' : barcode;
    var uniqueProductId = (barcode == undefined || barcode == '') ? productId : productId + "-" + barcode;
   //console.log(barcode);
    var productExist = $("#productTable tbody").find(".productId-" + uniqueProductId).val();
    if (productExist == undefined || productExist == "") {
        var quantity = 1;
        var totalProductPrice = parseFloat(unitPrice * quantity).toFixed(2);
        var productRowMarkup = '';
        var paramData = { productId: productId };
        var zoneSelectList = GetZoneSelectListByProductId(paramData);
        productRowMarkup += '<tr><td class="col-md-5 paddingReduce">'
            + '<input class="productId-' + uniqueProductId + '"  id="PurchaseProducts_' + productId + '_ProductId" name="PurchaseProducts[' + productId + '].ProductId" type="hidden" value="' + productId + '">'
            + '<input  id="PurchaseProducts_' + productId + '_Barcode" name="PurchaseProducts[' + productId + '].Barcode" type="hidden" value="' + barcode + '">' + productFullName + '</td>'
            + '<td class="col-md-2 paddingReduce"><input  value="' + unitPrice + '" class="form-control input-sm UnitPrice" id="PurchaseProducts_' + productId + '_UnitPrice" name="PurchaseProducts[' + productId + '].UnitPrice" type="text" onkeyup="updateTotalProductPrice(this)"></td>'
            + '<td class="col-md-2 paddingReduce">'
            + '<div class="col-md-12 paddingReduce input-group ">'
            + '<input value="' + quantity + '" class="form-control input-sm Quantity" id="PurchaseProducts_' + productId + '_Quantity" name="PurchaseProducts[' + productId + '].Quantity" type="text" onkeyup="updateTotalProductPrice(this)">'
            + ' <span class="input-group-addon productUnit">' + unit + '</span>'
            + '</div> </td>'
            + '<td class="col-md-2 paddingReduce"><input value="' + totalProductPrice + '" class="form-control input-sm TotalProductPrice" readonly="True" id="PurchaseProducts_' + productId + '_TotalPrice" name="PurchaseProducts[' + productId + '].TotalPrice" type="text">'
            + '<td class="col-md-2 paddingReduce"><select class="form-control input-sm valid" id="PurchaseProducts_' + productId + '_ZoneId" name="PurchaseProducts[' + productId + '].ZoneId">' + zoneSelectList + '</select></td>'
            + '<td class="col-md-1 paddingReduce"><span class="btn btn-xs btn-danger remove"><i class="fa fa-times"></i></span></td></tr>';

        $('#productTable').append(productRowMarkup);
    }
    else {
        var currentUnit = parseFloat($("#productTable tbody").find(".productId-" + uniqueProductId).closest('tr').find(".Quantity").val());
        currentUnit += 1;
        $("#productTable tbody").find(".productId-" + uniqueProductId).closest('tr').find(".Quantity").val(currentUnit);
        var currentUnitPrice = parseFloat($("#productTable tbody").find(".productId-" + uniqueProductId).closest('tr').find(".UnitPrice").val());
        var currentTotalPrice = parseFloat(currentUnit * currentUnitPrice).toFixed(2);

        $("#productTable tbody").find(".productId-" + uniqueProductId).closest('tr').find(".TotalProductPrice").val(currentTotalPrice);
     
    }
    CalculatePurchaseTotal();
}
function addProductToStockIn(productId, productFullName,  unit, barcode) {
    //console.log(barcode);
    barcode = (barcode == undefined || barcode == '') ? '' : barcode;
    var uniqueProductId = (barcode == undefined || barcode == '') ? productId : productId + "-" + barcode;
    //console.log(barcode);
    var productExist = $("#productTable tbody").find(".productId-" + uniqueProductId).val();
    if (productExist == undefined || productExist == "") {
        var quantity = 1;
        //var totalProductPrice = parseFloat(unitPrice * quantity).toFixed(2);
        var productRowMarkup = '';
        var paramData = { productId: productId };
        var zoneSelectList = GetZoneSelectListByProductId(paramData);
        productRowMarkup += '<tr><td class="col-md-5 paddingReduce">'
            + '<input class="productId-' + uniqueProductId + '"  id="PurchaseProducts_' + productId + '_ProductId" name="PurchaseProducts[' + productId + '].ProductId" type="hidden" value="' + productId + '">'
            + '<input  id="PurchaseProducts_' + productId + '_Barcode" name="PurchaseProducts[' + productId + '].Barcode" type="hidden" value="' + barcode + '">' + productFullName + '</td>'
            //+ '<td class="col-md-2 paddingReduce"><input  value="' + unitPrice + '" class="form-control input-sm UnitPrice" id="PurchaseProducts_' + productId + '_UnitPrice" name="PurchaseProducts[' + productId + '].UnitPrice" type="text" onkeyup="updateTotalProductPrice(this)"></td>'
            + '<td class="col-md-2 paddingReduce">'
            + '<div class="col-md-12 paddingReduce input-group ">'
            + '<input value="' + quantity + '" class="form-control input-sm Quantity" id="PurchaseProducts_' + productId + '_Quantity" name="PurchaseProducts[' + productId + '].Quantity" type="text" onkeyup="updateTotalProductPrice(this)">'
            + ' <span class="input-group-addon productUnit">' + unit + '</span>'
            + '</div> </td>'
            //+ '<td class="col-md-2 paddingReduce"><input value="' + totalProductPrice + '" class="form-control input-sm TotalProductPrice" readonly="True" id="PurchaseProducts_' + productId + '_TotalPrice" name="PurchaseProducts[' + productId + '].TotalPrice" type="text">'
            + '<td class="col-md-2 paddingReduce"><select class="form-control input-sm valid" id="PurchaseProducts_' + productId + '_ZoneId" name="PurchaseProducts[' + productId + '].ZoneId">' + zoneSelectList + '</select></td>'
            + '<td class="col-md-1 paddingReduce"><span class="btn btn-xs btn-danger remove"><i class="fa fa-times"></i></span></td></tr>';

        $('#productTable').append(productRowMarkup);
    }
    else {
        var currentUnit = parseFloat($("#productTable tbody").find(".productId-" + uniqueProductId).closest('tr').find(".Quantity").val());
        currentUnit += 1;
        $("#productTable tbody").find(".productId-" + uniqueProductId).closest('tr').find(".Quantity").val(currentUnit);
        //var currentUnitPrice = parseFloat($("#productTable tbody").find(".productId-" + uniqueProductId).closest('tr').find(".UnitPrice").val());
        //var currentTotalPrice = parseFloat(currentUnit * currentUnitPrice).toFixed(2);

        //$("#productTable tbody").find(".productId-" + uniqueProductId).closest('tr').find(".TotalProductPrice").val(currentTotalPrice);

    }
   
}
$(document).on("click", ".remove", function () {
    $(this).closest('tr').remove();
   CalculatePurchaseTotal();
});

function CalculatePurchaseTotal() {
    var sum = 0;
    
    $("#productTable tbody").find(".TotalProductPrice").each(function () {
        sum += parseFloat($(this).val());
    });
    
    //console.log('Sum:' + sum + " vat:" + vatSum);
    $('#footerTotal').text(parseFloat(sum).toFixed(2));
    $('#TotalPurchasePrice').val(parseFloat(sum).toFixed(2));

    CalculatePurchaseGrandTotal();
}
function CalculatePurchaseGrandTotal() {
    var TotalDiscountAmount,  TotalPrice, FinalTotal, purchaseCost, grandTotal;

    TotalPrice = parseFloat($('#TotalPurchasePrice').val());
    TotalDiscountAmount = isNonNegativeNumber($('#PurchaseDiscount').val().trim()) == false ? 0 : parseFloat($('#PurchaseDiscount').val().trim());

    purchaseCost = isNonNegativeNumber($('#PurchaseCost').val().trim()) == false ? 0 : parseFloat($('#PurchaseCost').val().trim());
    //discount validation checking
    //if (discoutAmountValidation(TotalDiscountType, TotalDiscountAmount, TotalPrice) == false) {
    //    $.alert("Invalid Discount!.", '<i class="fa fa-exclamation-circle" aria-hidden="true"> Alert</i>');
    //    $("#DiscountAmount").val(0);
    //    TotalDiscountAmount = 0;
    //}

    FinalTotal = parseFloat(TotalPrice).toFixed(2) - parseFloat(TotalDiscountAmount).toFixed(2);
    //grandTotal = parseFloat(FinalTotal + purchaseCost).toFixed(2);
    grandTotal = parseFloat(FinalTotal).toFixed(2);
    $('#grandTotal').val(parseFloat(grandTotal).toFixed(2));
    $('#PaidAmount').val(parseFloat(grandTotal).toFixed(2));
}

//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : January 2017
//=======================================================================================//