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
            { "data": "emailAddress", "width": "15%" },
            { "data": "firstName", "width": "15%" },
            { "data": "lastName", "width": "15%" },
            { "data": "phoneNumber", "width": "15%" },
            { "data": "role", "width": "15%" },
            { "data": "dateCreated", "width": "20%" },
            {
                "data": "accountId",
                "render": function (data) {
                    return `
                        <a href="/Account/EditUsers/${data}"><i class="fas fa-pencil-alt"></i> </a>
                        |
                        <a href="/Account/Delete/${data}"> <i class="fas fa-trash-alt"></i></a>
                        |
                        <a href="/Account/Delete/${data}"> <i class="fas fa-trash-alt"></i></a>
                            `
                },
                "width" : "15%"
            }
            ]
    });
}

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload()
                        Swal.fire(
                            'Deleted!',
                            'Your file has been deleted.',
                            'success'
                        );
                    }
                }
            })
        }
    })
}