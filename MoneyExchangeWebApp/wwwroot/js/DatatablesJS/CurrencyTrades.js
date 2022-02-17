var dataTable;

$(document).ready(function () {
    loadDataTable();
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl)
    })
});

function loadDataTable() {
    dataTable = $('#CTtable').DataTable({
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
                "type": "date",
                "render": function (data, type, row) {
                    if (type === "sort" || type === "type") {
                        return data;
                    }
                    return moment(data).format("MM-DD-YYYY HH:mm");
                },
                "width": "25%"
            },
            {
                "data": "transactionId",
                "render": function (data) {
                    return `
                        <a asp-controller="CurrencyTrade"
                                           asp-action="CurrencyTradeEdit"
                                           asp-route-id="${data}"><i class="fas fa-edit" data-bs-toggle="tooltip" data-bs-placement="top" title="Edit"></i></a>
                                            |
                                            <a asp-controller="CurrencyTrade"
                                           asp-action="SoftDeleteCurrencyTrade"
                                           asp-route-id="${data}"><i class="fas fa-trash-alt" data-bs-toggle="tooltip" data-bs-placement="top" title="Delete"></i></a>
                                            |
                                            <a asp-controller="CurrencyTrade"
                                           asp-action="CurrencyTradeDetails"
                                           asp-route-id="${data}"><i class="fas fa-eye" data-bs-toggle="tooltip" data-bs-placement="top" title="Details"></i></a>
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