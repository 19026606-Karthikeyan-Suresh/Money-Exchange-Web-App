var dataTable;
var clickId;

$(document).ready(function () {
    loadDataTable();
});
//Use the code below when you want to add a modal
/*data - bs - toggle="modal" data - bs - target="#DepositCurrencyModal"*/

function loadDataTable() {
    dataTable = $('#AWalltbl').DataTable({
        "ajax": {
            "url":"/Stock/GetWallet"
        },
        "columns": [
            { "data": "iso", "width": "5%" },
            { "data": "amount", "width": "10%" },
            {
                "data": "stockId",
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">

                        <a href="/Stock/DepositIntoWallet/${data}" class="btn btn-outline-primary mx-2">
                        <i class="fas fa-donate"></i> Deposit</a>

                        <a href="/Stock/WithdrawFromWallet/${data}" class="btn btn-outline-primary mx-2">
                        <i class="fas fa-hand-holding-usd"></i> Withdraw</a>

                        <a href="/DepOrWith/DOWTransactions/${data}" class="btn btn-outline-secondary mx-2">
                        <i class="fas fa-receipt"></i> Transactions</a>
                        </div>
                            `
                },
                "width" : "10%"
            }
            ]
    });
}

/*$("#myModal").on('show.bs.modal', function (event) {
    $(event.currentTarget).find('asp-route-id').val(getId);
    var btn = $(this).find('#modalDeleteButton');
    console.log(clickId)
    btn.attr('formaction', '/administrator/DeleteCategory/' + clickId);
});

function getId(clicked_id) {
    this.clickId = clicked_id;
}*/