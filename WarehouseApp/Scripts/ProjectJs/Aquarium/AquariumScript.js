//Basic function for ajax call get data ========================================================================================================================

function getData(url, paramData, callback, method) {
    method = method == null ? "POST" : method;
    $.ajax({
        type: method,
        url: url,
        data: JSON.stringify(paramData),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            //callback(data);
            if (callback == 'renderCustomerInfoLoad') {
                renderCustomerInfoLoad(data);
            }
            else if (callback == 'renderCustomerInfoLoadInvoice') {
                renderCustomerInfoLoadInvoice(data);
            }
            else if (callback == 'gameEditForm') {
                gameEditForm(data);
            }
            else if (callback == 'renderRoleDropdown') {

                renderRoleDropdown(data);
            }
            
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            $.alert(textStatus + "! please try again", '<i class="fa fa-exclamation-circle" aria-hidden="true"> Alert</i>');
        }
    });
}
//render callback functions for get data-------------------------------

function renderCustomerInfoLoad(data) {

    if (data.hasValue) {
        $("#CustomerId").val(data.customer[0].CustomerId);
        $("#FullName").val(data.customer[0].FullName);
        $("#Email").val(data.customer[0].Email);
        $("#Address").text(data.customer[0].Address);
        $("input[name=Gender][value='" + data.customer[0].Gender + "']").prop("checked", true);
        $("#Age").val(data.customer[0].Age);

    } else {
        $("#CustomerId").val(0);
        $("#FullName").val("");
        $("#Email").val("");
        $("#Address").text("");
        $("input[name=Gender][value='Male']").prop("checked", true);
        $("#Age").val("");
    }
}


function gameEditForm(data) {
    $("#TicketPriceConfigId").val(data[0].TicketPriceConfigId);
    $("#GameName").val(data[0].GameName);
    $("#GamePrefix").val(data[0].GamePrefix);
    $("#Price").val(data[0].Price);
    $("#Description").val(data[0].Description);
}


function paidPopOver(data) {
    alert(data.payable.BankName);
    var st = "<table class='table'><tr><td>Amount</td><td>" + data.payable.Amount + "</td></tr><tr><tr><td>Bank</td><td>" + data.payable.BankName + "</td></tr><tr><td>Paid Date</td><td>" + data.payable.ActualPaymentDate + "</td><tr></tr><td>Fx Rate</td><td>" + data.payable.ActualFxRate + "</td></tr></table>";
    return st;
}


function renderRoleDropdown(data) {
    $('#RoleId').html(data.selectListString);

}

//basic ajax function for post data====================================================================================================================

function postData(url, paramData, callback, method) {
    method = method == null ? "POST" : method;
    
    $.ajax({
        type: method,
        url: url,
        //data: paramData,
        data: JSON.stringify(paramData),
        processData: false,
        //contentType: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            //console.log(data);
            //callback(data);
            if (callback == 'beneficiarySaved') {
                beneficiarySaved(data);
            }
            else if (callback == 'renderSubmitBankFloat') {
                renderSubmitBankFloat(data);
            }
           
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            $.alert(textStatus + "! please try again", '<i class="fa fa-exclamation-circle" aria-hidden="true"> Alert</i>');
        }
    });
}
// callback functions for post data-----------------------------------
function beneficiarySaved(data) {
    var formData = new FormData();
    var totalFiles = document.getElementById("BeneficiryFiles").files.length;
    if (totalFiles > 0) {
        for (var i = 0; i < totalFiles; i++) {
            var file = document.getElementById("BeneficiryFiles").files[i];
            formData.append("BeneficiryFiles", file);
        }
        $.ajax({
            type: "POST",
            url: '/Beneficiary/UploadBeneficiaryFiles?id=' + data.thisBeneficiaryId,
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (data1) {
                //alert('succes!!');
                if (data1.Result == "OK") {
                    $.alert({
                        title: '<i class="fa fa-exclamation-circle" aria-hidden="true"> Alert</i>',
                        content: data.message,
                        confirm: function () {
                            window.location.href = data.successReuturnUrl;
                        }
                    });
                } else {
                    $.alert(data1.Result, '<i class="fa fa-exclamation-circle" aria-hidden="true"> Alert</i>');

                }
            },
            error: function (error) {
                $.alert('Error', '<i class="fa fa-exclamation-circle" aria-hidden="true"> Alert</i>');

            }
        });
    } else {
        $.alert({
            title: '<i class="fa fa-exclamation-circle" aria-hidden="true"> Alert</i>',
            content: data.message,
            confirm: function () {

                window.location.href = data.successReuturnUrl;
            }
        });
    }


}

function renderSubmitBankFloat(data) {

    $.alert({
        title: '<i class="fa fa-exclamation-circle" aria-hidden="true"> Alert</i>',
        content: data.message,
        confirm: function () {
            $('#BankFloatingModal').modal('hide');
            $("#bankFloatSubmitArea").show();
            $("#bankFloatSubmitAreaWait").html('');
            location.reload(); // shorthand

        }
    });
}