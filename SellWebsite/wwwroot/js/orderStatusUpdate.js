
function Rejected(Url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#151515',
        cancelButtonColor: '#d9534f',
        confirmButtonText: 'Yes, rejected it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: Url,
                type: 'Put',
                success: (data) => {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    })
}


function Approve(Url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#151515',
        cancelButtonColor: '#d9534f',
        confirmButtonText: 'Yes, approve it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: Url,
                type: 'Put',
                success: (data) => {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    })
}


function Returned(Url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#151515',
        cancelButtonColor: '#d9534f',
        confirmButtonText: 'Yes, approve it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: Url,
                type: 'Put',
                success: (data) => {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    })
}
