var dataTable;

$(document).ready(function () {
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl)
    });

    loadDataTable();
});

function loadDataTable() {

    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl)
    });
    dataTable = $('#CTStable').DataTable({
        "deferRender": true,
        "ajax": {
            "url": "/CurrencyTrade/GetAllCurrencyTrades"
        },
        "columns": [
            { "data": "transactionId", "width": "5%" },
            { "data": "baseCurrency", "width": "5%" },
            { "data": "baseAmount", "width": "10%" },
            { "data": "quoteCurrency", "width": "5%" },
            { "data": "quoteAmount", "width": "10%" },
            { "data": "exchangeRate", "width": "10%" },
            { "data": "doneBy", "width": "20%" },
            {
                "data": "transactionDate",
                render: $.fn.dataTable.render.moment('x', 'M/DD/YYYY, HH:mm a'),
                "width": "25%"
            },
            {
                "data": "transactionId",
                "render": function (data) {
                    return `
                        <a href="CurrencyTrade/CurrencyTradeEdit/${data}"><i class="fas fa-edit" data-bs-toggle="tooltip" data-bs-placement="top" title="Edit"></i></a>
                        |
                        <a href="CurrencyTrade/SoftDeleteCurrencyTrade/${data}"><i class="fas fa-trash-alt" data-bs-toggle="tooltip" data-bs-placement="top" title="Delete"></i></a>
                        |
                        <a href="CurrencyTrade/CurrencyTradeDetails/${data}"><i class="fas fa-eye" data-bs-toggle="tooltip" data-bs-placement="top" title="Details"></i></a>
                            `
                },
                "width": "10%"
            }
        ],
        dom: 'Bfrtip',
        buttons: [
            'copy',
            'pdf',
            'excel',
            'print'
        ],
        ordering: true,
        paging: true,
        searching: true,
        info: true,
        lengthChange: true,
        pageLength: 10

    });

}