//Basic function for ajax call get data ========================================================================================================================

function getRestaurantData(url, paramData, callback, method) {
    method = method == null ? "POST" : method;
    $.ajax({
        type: method,
        url: url,
        data: JSON.stringify(paramData),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            //callback(data);
            //if (callback == 'getProduct') {
            //    getProduct(data);
            //}

            //else if (callback == 'getProductDetails') {
            //    getProductDetails(data);
            //}

           
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            $.alert(textStatus + "! please try again", '<i class="fa fa-exclamation-circle" aria-hidden="true"> Alert</i>');
        }
    });
}


    // Ajax for POST =====================================================
    function postRestaurantData(url, paramData, callback, obj, method) {
        method = method == null ? "POST" : method;
      
        $.ajax({
            type: method,
            url: url,
            data: JSON.stringify(paramData),
            //processData: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                
                if (callback == 'updateOrderStatus') {
                    updateOrderStatus(data, obj);
                }
                //else if (callback == 'renderExpenseTypeAdd') {
                //    renderExpenseTypeAdd(data);
                //}
                

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                $.alert(textStatus + "! please try again", '<i class="fa fa-exclamation-circle" aria-hidden="true"> Alert</i>');

                }
        
        });


    }

 function updateOrderStatus(data, obj) {
        if (data.Result == "OK") {
            location.reload();
          
        } else {
            $.alert("Something wrong please try again!", '<i class="fa fa-exclamation-circle" aria-hidden="true"> Alert</i>');
        }
    }

   
//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : January 2017
//=======================================================================================//

