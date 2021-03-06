var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#trtable').DataTable({
        "ajax": {
            "url": "/ConvTransaction/GetAllConvTransactions"
        },
        "columns": [
            { "data": "transactionId", "width": "5%" },
            { "data": "baseCurrency", "width": "5%" },
            { "data": "baseAmount", "width": "10%" },
            { "data": "quoteCurrency", "width": "5%" },
            { "data": "quoteAmount", "width": "10%" },
            { "data": "exchangeRate", "width": "10%" },
            { "data": "transactionDate", "width" : "25%"},
            { "data": "doneBy", "width": "20%" },
            {
                "data": "transactionId",
                "render": function (data) {
                    return `
                        <a href="/ConvTransaction/ConvTransactionEdit/${data}"><i class="fas fa-user-edit"></i></a>
                        |
                        <a href="/ConvTransaction/SoftDeleteConvTransaction/${data}"><i class="fas fa-trash-alt"></i></a>
                        |
                        <a href="/ConvTransaction/ConvTransactionDetails/${data}"><i class="fas fa-eye"></i></a>
                            `
                },
                "width": "10%"
            }
        ]
    });
}