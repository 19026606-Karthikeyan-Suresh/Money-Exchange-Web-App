var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#AWalltbl').DataTable({
        "ajax": {
            "url":"/Wallet/GetAdminWallet"
        },
        "columns": [
            { "data": "iso", "width": "5%" },
            { "data": "amount", "width": "5%" },
            {
                "data": "stockId",
                "render": function (data) {
                    return `
                        <a href="/Wallet/DepositMoney/${data}"><i class="fas fa-donate"></i></a>
                        |
                        <a href="/Wallet/WithdrawMoney/${data}"><i class="fas fa-hand-holding-usd"></i></a>
                        |
                        <a href="/DepOrWith/TransactionHistory/${data}"><i class="fas fa-receipt"></i></a>
                            `
                },
                "width" : "10%"
            }
            ]
    });
}

/*function Delete(url) {
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
}*/