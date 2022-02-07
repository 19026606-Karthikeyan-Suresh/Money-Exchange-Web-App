var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#EXtbl').DataTable({
        "ajax": {
            "url": "/Currency/GetAllExchangeRates"
        },
        "columns": [
            { "data": "baseCurrency", "width": "5%" },
            { "data": "quoteCurrency", "width": "5%" },
            { "data": "exchangeRate", "width": "5%" }
        ]
    });
}