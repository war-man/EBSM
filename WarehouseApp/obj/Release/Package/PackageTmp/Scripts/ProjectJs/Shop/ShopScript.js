//Basic function for ajax call get data ========================================================================================================================

function getDataShop(url, paramData, callback, method) {
    method = method == null ? "POST" : method;
    console.log(url, paramData);
    $.ajax({
        type: method,
        url: url,
        data: JSON.stringify(paramData),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            //callback(data);
            console.log(callback);
            if (callback == 'getProduct') {
                getProduct(data);
            }

            else if (callback == 'getProductDetails') {
                getProductDetails(data);
            } else if (callback == 'getProductDetailsModal') {
                console.log(data);
                getProductDetailsModal(data);
            }
            else if (callback == 'renderBarcodeSearchForPurchase') {
                renderBarcodeSearchForPurchase(data);
            } else if (callback == 'renderBarcodeSearchForStockIn') {
                renderBarcodeSearchForStockIn(data);
            } else if (callback == 'renderBarcodeSearchForSale') {
                renderBarcodeSearchForSale(data);
            }
else if (callback == 'renderBarcodeSearchForPrint') {
    renderBarcodeSearchForPrint(data);
} else if (callback == 'renderBarcodeSearchForDamage') {
    renderBarcodeSearchForDamage(data);
            }

            else if (callback == 'damageProduct') {
                damageProduct(data);
            }

            else if(callback=='getAccountNo'){
                
                getAccountNo(data);
            }
            else if (callback == 'getAccountNoForPurchaseCost') {
                
                getAccountNoForPurchaseCost(data);
            }

            else if (callback == 'renderExpenseNameDropdown') {
                renderExpenseNameDropdown(data);
            }
            else if (callback == 'renderCustomerInfoLoadInvoice') {
                
                renderCustomerInfoLoadInvoice(data);
            } else if (callback == 'getCustomerProjects') {
                
                getCustomerProjects(data);
            } else if (callback == 'duePaymmentData') {
              
                duePaymmentData(data);
            }
            
            else if (callback == 'GetDamage') {
                //   console.log(data);
                GetDamage(data);
            } else if (callback == 'renderZoneSelectLisInPurchase') {
                 console.log(data);
                 return data.selectListString;
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            $.alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown+ "! please try again", '<i class="fa fa-exclamation-circle" aria-hidden="true"> Alert</i>');
        }
    });
}
function getProductDetailsModal(data) {
    $('#View_ProdName').text(data.product.ProductFullName);
    $('#View_RetailPrice').text(parseFloat(data.product.SalePrice).toFixed(2));
    $('#View_PurchasePrice').text(parseFloat(data.product.PurchasePrice).toFixed(2));
    $('#View_Code').text(data.product.ProductCode);
    $('#View_StockZone').text(data.product.DefaultWarehouseName);
    $('#View_Manufacturer').text(data.product.Manufacturer);
    $('#View_Discount').text(data.product.Discount);
    $('#View_Categories').text(data.categories);
    $('#View_Specification').text(data.specifications);
    $('#customerRelationArea').html(data.customerOptions);
    $("#editButtonModal").attr("href", "/Product/SelectAttributeSet?productId=" + data.product.ProductId);
    $('#productDetailsModal').modal('show');
}
function duePaymmentData(data) {
    $('#PurchaseNumber').val(data.result.PurchaseNumber);
    $('#Supplier').val(data.result.SupplierName);
    //$('#PurcDate').val(formatDateString(data.purchase.PurchaseDate,"-"));
    $('#Total').val(data.result.TotalPrice);
    $('#Paid').val(data.result.PaidAmount);
    var due = parseFloat(data.result.TotalPrice - data.result.PaidAmount);
    $('#DueAmount').val(due);

}
function GetDamage(data) {
    
    $('#DamageId').val(data.DamageId);
    $('#StockId').val(data.StockId);
    $('#ProductName').val(data.ProductName);
    $('#OldQuantity').val(data.Quantity);
    $('#Quantity').val(data.Quantity);
    $('#ZoneId').val(data.DefaultZoneId);
}
function renderBarcodeSearchForPurchase(data) {
    addProductToPurchase(data[0].ProductId, data[0].ProductFullName, data[0].PurchasePrice,data[0].Unit,data[0].Barcode);
   
}function renderBarcodeSearchForStockIn(data) {
    addProductToStockIn(data[0].ProductId, data[0].ProductName, data[0].Unit, data[0].Barcode);
   
} function addProductToStockIn(data) {
    addProductToPurchase(data[0].ProductId, data[0].ProductFullName,data[0].Unit,data[0].Barcode);
   
} function renderBarcodeSearchForSale(data) {
    //console.log(data);
    addProductToSell(data[0].ProductId, data[0].ProductFullName, data[0].SalePrice, data[0].Unit, data[0].DiscountAmount, data[0].DiscountType, data[0].Vat, data[0].stock, data[0].DefaultWarehouse, data[0].Barcode);
   
} function renderBarcodeSearchForPrint(data) {
    addBarcodeToPrint(data[0].ProductId, data[0].ProductName, data[0].UnitPrice, data[0].DiscountAmount, data[0].DiscountType, data[0].Barcode);
} function renderBarcodeSearchForDamage(data) {
    addProductToDamage(data[0].ProductId, data[0].ProductName, data[0].DefaultWarehouse);
}

function SavePurchaseProduct(url, paramData, callback, method) {

    method = method == null ? "POST" : method;
    $.ajax({
        type: method,
        url: url,
        data: JSON.stringify(paramData),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            //callback(data);
            if (callback == 'savePurchase') {
                savePurchase(data);
            }
            else if (callback == 'saveProduct') {
               
                saveProduct(data);
            }
            else if (callback == 'saveInvoice') {
               
                saveInvoice(data);
            }

            else if (callback == 'getProductDetails') {
                getProductDetails(data);
            }
            else if (callback == 'saveDamage') {
                saveDamage(data);
            }
            
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {            
            $.alert(textStatus + "! please try again", '<i class="fa fa-exclamation-circle" aria-hidden="true"> Alert</i>');
        }
    });

}
//render callback functions for get data-------------------------------

function updatePurchaseProduct(url, paramData, callback, method) {
   
    method = method == null ? "POST" : method;
    $.ajax({
        type: method,
        url: url,
        data: JSON.stringify(paramData),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            //callback(data);
            if (callback == 'saveProduct') {
                saveProduct(data);
            }

            else if (callback == 'getProductDetails') {
                getProductDetails(data);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {          
            $.alert(textStatus + "! please try again", '<i class="fa fa-exclamation-circle" aria-hidden="true"> Alert</i>');
        }
    });

}
//render callback functions for get data-------------------------------

function saveProduct(data) {    
    if (data == "OK") {
        if (confirm('Are you again add purchase?')){
            location.reload();
        }
        else {
            window.location = '/Product/Index';
        }
        
    }
   
}

function saveDamage(data) {

    if (data.result == "OK") {
            window.location.href = data.returnUrl;
        }
    else {
       
        HideProgressMessage('#addDamages', '<i class="fa fa-save"></i> Save Damage');
            $.alert("There is something wrong! Please try again.", '<i class="fa fa-exclamation-circle" aria-hidden="true"> Alert</i>');
        }

    

}

function savePurchase(data) {
    if (data.result == "OK") {
        window.location.href = data.returnUrl;
        }
    else {
        //$('#savePurchase').html('<i class="fa fa-save"></i> Save Purchase').prop("disabled", false);
        HideProgressMessage('#savePurchase', '<i class="fa fa-spinner fa-lg fa-spin"></i> Save Purchase');
            $.alert("There is something wrong! Please try again.", '<i class="fa fa-exclamation-circle" aria-hidden="true"> Alert</i>');
        }
    }

function getProduct(data) {    
    $('#productlist').append(data.selectListString);

   
} function saveInvoice(data) {
  
    if (data.result == "OK") {
        window.location.href = data.returnUrl;
    }
    else {
        //$('#saveSales').html('<i class="fa fa-save"></i> Save Sales & Print').prop("disabled", false);
        HideProgressMessage('#saveSales', '<i class="fa fa-save"></i> Save Sales Only');
        HideProgressMessage('#savePrintSales', '<i class="fa fa-print"></i> Save Sales & Print');
        $.alert("There is something wrong! Please try again.", '<i class="fa fa-exclamation-circle" aria-hidden="true"> Alert</i>');
    }
   
}
function getProduct(data) {    
    $('#productlist').append(data.selectListString);


    }

    function damageProduct(data) {
        $('#ProductId').append(data.selectListString);

    }

    function getAccountNo(data) {
        $('#TransactionModeId').append(data.selectListString);

    } function renderExpenseNameDropdown(data) {
        $('#ExpenseTypeId').html(data.selectListString);
    } function getAccountNoForPurchaseCost(data) {
        $('#PcTransactionModeId').append(data.selectListString);
    }

    function getProductDetails(data) {
        var PerCarton = data.product[0].PerCarton;
        var tp = data.product[0].Tp;
        $('#unitCarton').val(PerCarton);
        $('#UnitPrice').val(tp);
    }
    

    

   

    function getStockByProductId(paramData) {
        var method = "POST", total;
        $.ajax({
            type: method,
            url: '/InvoiceProduct/getProductStock',
            data: JSON.stringify(paramData),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                total = data.TotalPieces;
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                $.alert(textStatus + "! Your Entered Quantity is more then Stock ", '<i class="fa fa-exclamation-circle" aria-hidden="true"> Alert</i>');
            }
        });
        return total;
    }


function renderCustomerInfoLoadInvoice(data) {

    if (data.hasValue) {
        $("#CustomerId").val(data.customer[0].CustomerId);
        $("#CustomerName").val(data.customer[0].FullName);
        $("#CustomerEmail").val(data.customer[0].Email);}
    else {
        $("#CustomerId").val(0);
        $("#CustomerName").val("");
        $("#CustomerEmail").val("");

    }}
function getCustomerProjects(data) {
    $('#CustomerBranchId').empty();
    $('#CustomerBranchId').append(data.selectListString);

}

    // Ajax for POST =====================================================
    function postShopData(url, paramData, callback, obj, method) {
        method = method == null ? "POST" : method;
      
        $.ajax({
            type: method,
            url: url,
            data: JSON.stringify(paramData),
            //processData: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                
                if (callback == 'renderExpenseNameAdd') {
                    renderExpenseNameAdd(data, obj);
                }
                else if (callback == 'renderExpenseTypeAdd') {
                    renderExpenseTypeAdd(data);
                }
                else if (callback == 'renderGroupNameAdd') {
                    
                    renderGroupNameAdd(data);
                }
                else if (callback == 'updateStatus') {
                    updateStatus(data, obj);
                } else if (callback == 'updateGroupNameStatus') {
                    updateGroupNameStatus(data, obj);
                } else if (callback == 'renderAttributeSetSave') {
                    renderAttributeSetSave(data);
                } else if (callback == 'renderNewCustomerAddedToSales') {
                    renderNewCustomerAddedToSales(data);
                } else if (callback == 'renderNewCustomerBranchAddedToSales') {
                    renderNewCustomerBranchAddedToSales(data);
                } else if (callback == 'renderNewSupplierAddedToPurchase') {
                    renderNewSupplierAddedToPurchase(data);
                } else if (callback == 'renderNewManufactureAddedToProductForm') {
                    renderNewManufactureAddedToProductForm(data);
                } 

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                $.alert(textStatus + "! please try again", '<i class="fa fa-exclamation-circle" aria-hidden="true"> Alert</i>');

                }
        
        });


    }

    function renderExpenseNameAdd(data, obj) {
        if (data.isExist == "No") {
            /* $(obj).parent('td').parent('tr').parent('tbody').append('<tr><td>' + data.expenseType.TypeName + '</td></tr>');
             $(obj).parent('td').parent('tr').html('');*/
            location.reload();
        } else {
            $.alert("This name already exist!",
                   '<i class="fa fa-exclamation-circle" aria-hidden="true"> Alert</i>');
        }

    }

    function renderExpenseTypeAdd(data) {
        if (data.isExist == "No") {

            location.reload();
        } else {
            $.alert("This name already exist!",
                   '<i class="fa fa-exclamation-circle" aria-hidden="true"> Alert</i>');
        }
    }
    function renderGroupNameAdd(data) {
        
        if (data.isExist == "No") {
            location.reload();
        } else {
            $.alert("This name already exist or something wrong!",
                   '<i class="fa fa-exclamation-circle" aria-hidden="true"> Alert</i>');
        }
    }

    function updateStatus(data, obj) {
        if (data.Result == "OK") {
            location.reload();
            $(obj).html('<a href="javascript:void(0);" data-id="@item.ExpenseTypeId" class="activeStatus"><span class="btn btn-xs btn-danger">Inactive</span></a>');
        } else {
            $.alert("Something wrong please try again!", '<i class="fa fa-exclamation-circle" aria-hidden="true"> Alert</i>');
        }
    } function updateGroupNameStatus(data, obj) {
        if (data.Result == "OK") {
            location.reload();
          //  $(obj).html('<a href="javascript:void(0);" data-id="@item.GroupNameId" class="activeStatus"><span class="btn btn-xs btn-danger">Inactive</span></a>');
        } else {
            $.alert("Something wrong please try again!", '<i class="fa fa-exclamation-circle" aria-hidden="true"> Alert</i>');
        }
    }

    function renderAttributeSetSave(data) {
        if (data.result == "Ok") {
            window.location.href = data.returnUrl;
        } else {
            $.alert("Something Wrong!",
                   '<i class="fa fa-exclamation-circle" aria-hidden="true"> Alert</i>');
        }
    } function renderNewCustomerAddedToSales(data) {
    if (data.inserted == true) {
       
            $("#createInvoiceForm #CustomerId").append("<option value='" + data.customer.CustomerId + "'>" + data.customer.FullName + "</option>");
            $('#customerAddModal').modal('hide');
        }
    } function renderNewCustomerBranchAddedToSales(data) {
    if (data.inserted == true) {
       
        $("#createInvoiceForm #CustomerBranchId").append("<option value='" + data.customerBranch.CustomerProjectId + "'>" + data.customerBranch.ProjectName + "</option>");
        $('#customerBranchAddModal').modal('hide');
        }
}

function renderNewSupplierAddedToPurchase(data) {
        if (data.inserted == true) {
         
            $("#createPurchaseForm #SupplierId").append("<option value='" + data.supplier.SupplierId + "'>" + data.supplier.SupplierName + "</option>");
            $('#supplierAddModal').modal('hide');
        } 
    }function renderNewManufactureAddedToProductForm(data) {
        if (data.inserted == true) {
         
            $("#addProductForm #ManufacturerId").append("<option value='" + data.supplier.SupplierId + "'>" + data.supplier.SupplierName + "</option>");
            
            $('#supplierAddModal').modal('hide');
        } 
    }
    function updateTotalProductPrice(obj) {
  
        var unitPrice = isNonNegativeNumber($(obj).closest("tr").find(".UnitPrice").val().trim()) == false ? 0 : parseFloat($(obj).closest("tr").find(".UnitPrice").val().trim());
        var quantity = isNonNegativeNumber($(obj).closest("tr").find(".Quantity").val().trim()) == false ? 0 : parseFloat($(obj).closest("tr").find(".Quantity").val().trim());
        var totalProductPrice = unitPrice * quantity;
        $(obj).closest("tr").find(".TotalProductPrice").val(totalProductPrice);
        //console.log(unitPrice, quantity, totalProductPrice);
        CalculatePurchaseTotal();

    }

    function addProductToGenearateBarcode(productId, productFullName) {

        var productExist = $("#genearateBarcodeTable tbody").find(".productId-" + productId).val();
        if (productExist == undefined || productExist == "") {
            var quantity = 1;
            var productRowMarkup = '';
            productRowMarkup += '<tr><td class="col-md-5 paddingReduce">'
                + '<input class="productId-' + productId + '"  id="ProductBarcodes_' + productId + '_ProductId" name="ProductBarcodes[' + productId + '].ProductId" type="hidden" value="' + productId + '">' + productFullName + '</td>'
                + '<td class="col-md-2 paddingReduce"><div class="col-md-12 paddingReduce input-group"><input  class="form-control input-sm exp rfwDatepicker" id="ProductBarcodes_' + productId + '_UnitPrice" name="ProductBarcodes[' + productId + '].Exp" type="text" placeholder = "dd-mm-yyyy"> <span class="input-group-addon rfwDatepickerIcon"><i class="glyphicon glyphicon-calendar"></i></span>  </div></td>'
                + '<td class="col-md-2 paddingReduce"><input value="' + quantity + '" class="form-control input-sm printQuantity"  id="ProductBarcodes_' + productId + '_PrintQuantity" name="ProductBarcodes[' + productId + '].PrintQuantity" type="text">'
                + '<td class="col-md-1 paddingReduce"><span class="btn btn-xs btn-danger remove"><i class="fa fa-times"></i></span></td></tr>';
            $('#genearateBarcodeTable').append(productRowMarkup);
        }
        else {
            var currentUnit = parseFloat($("#genearateBarcodeTable tbody").find(".productId-" + productId).closest('tr').find(".printQuantity").val());
            currentUnit += 1;
            $("#genearateBarcodeTable tbody").find(".productId-" + productId).closest('tr').find(".printQuantity").val(currentUnit);
        }
    }
    function addBarcodeToPrint(productId, productFullName,unitPrice,discountAmount,discountType,barcode) {
        var uniqueProductId = (barcode == undefined) ? productId : productId + "-" + barcode;

        var productExist = $("#printBarcodeTable tbody").find(".productId-" + uniqueProductId).val();
        if (productExist == undefined || productExist == "") {

            var quantity = 1;
            var productRowMarkup = '';
            productRowMarkup += '<tr><td class="col-md-5 paddingReduce">'
                + '<input class="productId-' + uniqueProductId + '"  id="ProductBarcodes_' + productId + '_ProductId" name="ProductBarcodes[' + productId + '].ProductId" type="hidden" value="' + productId + '">'
                + '<input  id="ProductBarcodes_' + productId + '_ProductFullName" name="ProductBarcodes[' + productId + '].Product.ProductFullName" type="hidden" value="' + productFullName + '">'
                + '<input   id="ProductBarcodes_' + productId + '_Dp" name="ProductBarcodes[' + productId + '].Product.Dp" type="hidden" value="' + unitPrice + '">'
                + '<input  id="ProductBarcodes_' + productId + '_DiscountAmount" name="ProductBarcodes[' + productId + '].Product.DiscountAmount" type="hidden" value="' + discountAmount + '">'
                + '<input  id="ProductBarcodes_' + productId + '_DiscountType" name="ProductBarcodes[' + productId + '].Product.DiscountType" type="hidden" value="' + discountType + '">' + productFullName + '</td>'
                + '<td class="col-md-2 paddingReduce"><input id="ProductBarcodes_' + productId + '_Barcode" name="ProductBarcodes[' + productId + '].Barcode" type="hidden" value="' + barcode + '">' + barcode + '</td>'
                + '<td class="col-md-2 paddingReduce"><input value="' + quantity + '" class="form-control input-sm printQuantity"  id="ProductBarcodes_' + productId + '_PrintQuantity" name="ProductBarcodes[' + productId + '].PrintQuantity" type="text">'
                + '<td class="col-md-1 paddingReduce"><span class="btn btn-xs btn-danger remove"><i class="fa fa-times"></i></span></td></tr>';
            $('#printBarcodeTable').append(productRowMarkup);
        }
        else {
            var currentUnit = parseFloat($("#printBarcodeTable tbody").find(".productId-" + uniqueProductId).closest('tr').find(".printQuantity").val());
            currentUnit += 1;
            $("#printBarcodeTable tbody").find(".productId-" + uniqueProductId).closest('tr').find(".printQuantity").val(currentUnit);
        }
    }
    function addProductToDamage(productId, productFullName, defaultWarehouse) {
        //here productId means StockId
        //barcode = (barcode == undefined || barcode == '') ? '' : barcode;
        //var uniqueProductId = (barcode == undefined || barcode == '') ? productId : productId + "-" + barcode;
        var uniqueProductId = productId;

        var productExist = $("#damageProductTable tbody").find(".productId-" + uniqueProductId).val();
        if (productExist == undefined || productExist == "") {

            var quantity = 1;
           
            var productRowMarkup = '';
            productRowMarkup += '<tr><td class="col-md-5 paddingReduce">'
                + '<input class="productId-' + uniqueProductId + '"  id="DamageProducts_' + productId + '_StockId" name="DamageProducts[' + productId + '].StockId" type="hidden" value="' + productId + '">' + productFullName + '</td>'
                + '<input class="defaultWarehouse" id="DamageProducts_' + productId + '_DefaultZoneId" name="DamageProducts[' + productId + '].DefaultZoneId" type="hidden" value="' + defaultWarehouse + '">'
                + '<td class="col-md-2 paddingReduce"><input value="' + quantity + '" class="form-control input-sm quantity"  id="DamageProducts_' + productId + '_Quantity" name="DamageProducts[' + productId + '].Quantity" type="text">'
                + '<td class="col-md-3 paddingReduce"><input  class="form-control input-sm note"  id="DamageProducts_' + productId + '_Note" name="DamageProducts[' + productId + '].Note" type="text">'
                + '<td class="col-md-1 paddingReduce"><span class="btn btn-xs btn-danger remove"><i class="fa fa-times"></i></span></td></tr>';
            $('#damageProductTable').append(productRowMarkup);
        }
        else {
            var currentUnit = parseFloat($("#damageProductTable tbody").find(".productId-" + uniqueProductId).closest('tr').find(".Quantity").val());
            currentUnit += 1;
            $("#damageProductTable tbody").find(".productId-" + uniqueProductId).closest('tr').find(".Quantity").val(currentUnit);
        }
    } function addProductToArticleTransfer(productId, productFullName, defaultWarehouse) {
        //here productId means StockId
        //barcode = (barcode == undefined || barcode == '') ? '' : barcode;
        //var uniqueProductId = (barcode == undefined || barcode == '') ? productId : productId + "-" + barcode;
        var uniqueProductId = productId;

        var productExist = $("#damageProductTable tbody").find(".productId-" + uniqueProductId).val();
        if (productExist == undefined || productExist == "") {

            var quantity = 1;
            var paramData = { stockIdNotIn: productId };
            var stockToSelectList = GetStockSelectList(paramData);

            var productRowMarkup = '';
            productRowMarkup += '<tr><td class="col-md-4 paddingReduce">'
                + '<input class="productId-' + uniqueProductId + '"  id="ArticleTransfers_' + productId + '_FromStockId" name="ArticleTransfers[' + productId + '].FromStockId" type="hidden" value="' + productId + '">' + productFullName + '</td>'
                + '<input class="defaultWarehouse" id="ArticleTransfers_' + productId + '_DefaultZoneId" name="ArticleTransfers[' + productId + '].DefaultZoneId" type="hidden" value="' + defaultWarehouse + '">'
            + '<td class="col-md-4 paddingReduce"><select class="form-control input-sm valid" id="ArticleTransfers_' + productId + '_ToStockId" name="ArticleTransfers[' + productId + '].ToStockId">' + stockToSelectList + '</select></td>'
               + '<td class="col-md-2 paddingReduce"><input value="' + quantity + '" class="form-control input-sm quantity"  id="ArticleTransfers_' + productId + '_TransferQuantity" name="ArticleTransfers[' + productId + '].TransferQuantity" type="text">'
                + '<td class="col-md-1 paddingReduce"><span class="btn btn-xs btn-danger remove"><i class="fa fa-times"></i></span></td></tr>';
            $('#damageProductTable').append(productRowMarkup);
        }
        else {
            var currentUnit = parseFloat($("#damageProductTable tbody").find(".productId-" + uniqueProductId).closest('tr').find(".Quantity").val());
            currentUnit += 1;
            $("#damageProductTable tbody").find(".productId-" + uniqueProductId).closest('tr').find(".Quantity").val(currentUnit);
        }
    }
    function addProductToProductTransfer(productId, productFullName, defaultWarehouse) {
        //here productId means StockId
        //barcode = (barcode == undefined || barcode == '') ? '' : barcode;
        //var uniqueProductId = (barcode == undefined || barcode == '') ? productId : productId + "-" + barcode;
        var uniqueProductId = productId;

        var productExist = $("#damageProductTable tbody").find(".productId-" + uniqueProductId).val();
        if (productExist == undefined || productExist == "") {

            var quantity = 1;
            var paramData1 = { stockId: productId };
            var fromZoneSelectList  = GetZoneSelectListByStockId(paramData1);

            var paramData2 = { productId: productId };
            var toZoneSelectList = GetZoneSelectListByProductId(paramData2);

            var productRowMarkup = '';
            productRowMarkup += '<tr><td class="col-md-4 paddingReduce">'
                + '<input class="productId-' + uniqueProductId + '"  id="TransferProductss_' + productId + '_StockId" name="TransferProducts[' + productId + '].StockId" type="hidden" value="' + productId + '">' + productFullName + '</td>'
                + '<input class="defaultWarehouse" id="TransferProducts_' + productId + '_DefaultZoneId" name="TransferProducts[' + productId + '].DefaultZoneId" type="hidden" value="' + defaultWarehouse + '">'
            + '<td class="col-md-4 paddingReduce"><select class="form-control input-sm valid" id="TransferProducts_' + productId + '_ZoneFromId" name="TransferProducts[' + productId + '].ZoneFromId">' + fromZoneSelectList + '</select></td>'
            + '<td class="col-md-4 paddingReduce"><select class="form-control input-sm valid" id="TransferProducts_' + productId + '_ZoneToId" name="TransferProducts[' + productId + '].ZoneToId">' + toZoneSelectList + '</select></td>'
               + '<td class="col-md-2 paddingReduce"><input value="' + quantity + '" class="form-control input-sm quantity"  id="TransferProducts_' + productId + '_TransferQuantity" name="TransferProducts[' + productId + '].TransferQuantity" type="text">'
                + '<td class="col-md-1 paddingReduce"><span class="btn btn-xs btn-danger remove"><i class="fa fa-times"></i></span></td></tr>';
            $('#damageProductTable').append(productRowMarkup);
        }
        else {
            var currentUnit = parseFloat($("#damageProductTable tbody").find(".productId-" + uniqueProductId).closest('tr').find(".Quantity").val());
            currentUnit += 1;
            $("#damageProductTable tbody").find(".productId-" + uniqueProductId).closest('tr').find(".Quantity").val(currentUnit);
        }
    }
    function GetZoneSelectListByProductId(paramData) {
        var method = "POST", selectListString;
        $.ajax({
            type: method,
            url: '/WarehouseZone/GetZoneSelectListByProductId',
            data: JSON.stringify(paramData),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                selectListString = data.selectListString;
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                $.alert(textStatus + "! Something Wrong", '<i class="fa fa-exclamation-circle" aria-hidden="true"> Alert</i>');
            }
        });
        return selectListString;
    }function GetZoneSelectListByStockId(paramData) {
        var method = "POST", selectListString;
        $.ajax({
            type: method,
            url: '/WarehouseZone/GetZoneSelectListByStockId',
            data: JSON.stringify(paramData),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                selectListString = data.selectListString;
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                $.alert(textStatus + "! Something Wrong", '<i class="fa fa-exclamation-circle" aria-hidden="true"> Alert</i>');
            }
        });
        return selectListString;
    }
    function GetStockSelectList(paramData) {
        var method = "POST", selectListString;
        $.ajax({
            type: method,
            url: '/Stock/GetStockSelectList',
            data: JSON.stringify(paramData),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                selectListString = data.selectListString;
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                $.alert(textStatus + "! Something Wrong ", '<i class="fa fa-exclamation-circle" aria-hidden="true"> Alert</i>');
            }
        });
        return selectListString;
    }
    $(document).on("click", ".remove", function () {
        $(this).closest('tr').remove();
    });

   
//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : January 2017
//=======================================================================================//

