var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#FAQData').DataTable({
        "ajax": {
            "url": "/FAQ/GetAllFAQs"
        },
        "columns": [
            { "data": "faqId", "width": "5%" },
            { "data": "question", "width": "25%" },
            { "data": "answer", "width": "15%" },
            {
                "data": "faqId",
                "render": function (data) {
                    return `
                        <a href="/FAQ/FAQEdit/${data}"><i class="fas fa-edit" data-bs-toggle="tooltip" data-bs-placement="top" title="Edit FAQ"></i></a>
                        |
                        <a href="/FAQ/FAQDelete/${data}"><i class="fas fa-trash-alt" data-bs-toggle="tooltip" data-bs-placement="top" title="Permanently delete FAQ"></i></a>
                            `
                },
                "width": "10%"
            }
        ]
    });
}