
//$(document).ready(function () {
//    $(".product").click(function () {

//        var productName = $(this).find(".product-name").text();
//        var productPrice = parseFloat($(this).find(".unitPrice").text());
//        var productUnit = 1;
//        var totalPrice = productUnit * productPrice;

//        var orderMarkup = "<tr><td>" + productName + "<br />" + productPrice +
        					
//        					  "</td><td class='unit'><span class='numOfUnit'>" + productUnit + '</span><button class="btn btn-xs btn-link increasebutton" ><i class="fa fa-plus"></i></button>' + '<button class="btn btn-xs btn-link decreasebutton" ><i class="fa fa-minus"></i></button>' +
//        					  "</td><td class='price'>" + totalPrice +
//        					  "</td><td>" + '<a class="btn btn-xs btn-danger removebutton" href="javascript:void(0);"><i class="fa fa-times"></i></a>' + "</td></tr>";

//        $("#order-list tbody").append(orderMarkup);
//        calculateSum();
//    });
//});

//$(document).ready(function () {
   
//    $(document).on("click", ".removebutton", function () {
//        $(this).parents("tr").remove();
//        calculateSum();
//    });
//});

//$(document).ready(function () {
   
//    $(document).on("click", ".increasebutton", function () {
//        var currentUnit = parseInt($(this).parent('.unit').find('.numOfUnit').text());
//        //$("#solution")
//        //console.log(currentUnit);
//        currentUnit += 1;
//        $(this).parent('.unit').find('.numOfUnit').text(currentUnit);
//        var productPrice = stringRplace($(this).parent('.unit').parent("tr").find(".unitPrice").text());
//        console.log(productPrice);
//        var totalPrice = currentUnit * productPrice;
//        $(this).parent('.unit').parent("tr").find('.price').text(totalPrice);
//        calculateSum();
//    });
//});

//$(document).ready(function () {
 

//    $(document).on("click", ".decreasebutton", function () {
//        var currentUnit = parseInt($(this).parent('.unit').find('.numOfUnit').text());

//        if (currentUnit > 0) {
//            currentUnit -= 1;
//            $(this).parent('.unit').find('.numOfUnit').text(currentUnit);
//            var productPrice = stringRplace($(this).parent('.unit').parent("tr").find(".unitPrice").text());
//            console.log(productPrice);
//            var totalPrice = currentUnit * productPrice;
//            $(this).parent('.unit').parent("tr").find('.price').text(totalPrice);
//            calculateSum();
//        } else if (currentUnit == -1 || currentUnit < -1) {
//            alert("Amount can not be less than 0");
//        }

//    });
//});

//function calculateSum() {
//    var totalUnit       = 0;
//    var totalPrice      = 0;
//    var vatPercentage   = 4.5;
//    var vatAmount       = 0;
//    var grandTotal      = 0;
//    $('tr').find('.numOfUnit').each(function () {
//        var combat = $(this).text();
//        if (!isNaN(combat) && combat.length !== 0) {
//            totalUnit += parseFloat(combat);
//        }
//    });
//    $("#unit-total").text(totalUnit);

//    $('tr').find('td.price').each(function () {
//        var combat = $(this).text();
//        if (!isNaN(combat) && combat.length !== 0) {
//            totalPrice += parseFloat(combat);
//        }
//    });
//    $("#price-total").text(totalPrice);
//    vatAmount = (totalPrice * vatPercentage) / 100;

//    $("#vat-percentage").text(vatPercentage + " %");
//    $("#vat-total").text(vatAmount);
//    grandTotal = totalPrice + vatAmount;
//    $("#grand-total").text(grandTotal);
//}

//function stringRplace(a) {
//    var x = a;
//    var y = x.replace(/\$|\,/g, "");
//    return y;
//}