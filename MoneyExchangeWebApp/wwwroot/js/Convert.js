    function Convert() {
        var Amount = $("#Amount").val();
        var fromCurr = $("#fromCurr").val();
        var toCurr = $("#toCurr").val();
        var apiUrl = "/api/HomeApi/Convert/" + fromCurr + "/" + toCurr + "/" + Amount;
        var currConvertedAmt = $("#currConvertedAmt");
        if (fromCurr == 0 || toCurr == 0){
            alert("Please ensure you have picked the 2 currencies you wish to convert.");
        return;
                }
        if (!$.isNumeric(Amount)){
            alert("Please ensure Amount is a numeric number");
        return;
                }
        else if (Amount < 0){
            alert("Please ensure Amount is a positive number");
        return;
                }
        else {
            currConvertedAmt.html("");

        $.getJSON(apiUrl, function (result) {
            currConvertedAmt.append("<h5> ≈ " + toCurr + result.toFixed(2) + "</h5>");
             });
        }
    }

    $(document).ready(function () {
        $("#btnConvert").click(Convert);
    })