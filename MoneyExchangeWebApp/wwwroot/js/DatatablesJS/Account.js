var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url":"/Account/GetAll"
        },
        "columns": [
            { "data": "accountId", "width": "5%" },
            { "data": "emailAddress", "width": "25%" },
            { "data": "firstName", "width": "15%" },
            { "data": "lastName", "width": "15%" },
            { "data": "phoneNumber", "width": "10%" },
            { "data": "role", "width": "5%" },
            { "data": "dateCreated", "width": "15%" },
            {
                "data": "accountId",
                "render": function (data) {
                    return `
                        <a href="/Account/EditUsers/${data}"><i class="fas fa-user-edit" data-bs-toggle="tooltip" data-bs-placement="top" title="Edit User Account"></i></a>
                        |
                        <a href="/Account/Delete/${data}"><i class="fas fa-trash-alt" data-bs-toggle="tooltip" data-bs-placement="top" title="Delete User Account"></i></a>
                        |
                        <a href="/Account/AccountDetails/${data}"><i class="fas fa-eye" data-bs-toggle="tooltip" data-bs-placement="top" title="View Account Details"></i></a>
                            `
                },
                "width" : "10%"
            }
            ]
    });
}