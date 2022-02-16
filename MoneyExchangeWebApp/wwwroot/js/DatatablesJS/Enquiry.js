﻿var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#EnqTbl').DataTable({
        "ajax": {
            "url": "/Enquiry/GetAllEnquiries"
        },
        "columns": [
            { "data": "enquiryId", "width": "5%" },
            { "data": "emailAddress", "width": "10%" },
            { "data": "subject", "width": "5%" },
            { "data": "question", "width": "10%" },
            { "data": "enquiryDate", "width": "5%" },
            { "data": "status", "width": "5%" },
            {
                "data": "enquiryId",
                "render": function (data) {
                    return `
                        <a href="/Enquiry/EnquiryReply/${data}"><i class="fas fa-reply"></i></a>
                            `
                },
                "width": "10%"
            }
        ],
        dom: 'Bfrtip',
        buttons: [
            'copy',
            'pdf',
            'docs',
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