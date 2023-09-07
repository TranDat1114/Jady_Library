
$(document).ready(function () {
    loadDataTable();
    console.log("helo");
});
function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/order/getall' },
        "columns": [
            { data: 'orderId' },
            { data: 'name' },
            { data: 'email' },
            { data: 'phoneNumber' },
            { data: 'orderStatus' },
            { data: 'orderTime' },
            { data: 'returnTime' },
            {
                data: 'isReturn',
                "render": function (isReturn) {

                    return isReturn == true ? `
                    <td>
                        <input type="checkbox" checked>
                    </td>                  
                        `:
                        `
                    <td>
                        <input type="checkbox" >
                    </td>                  
                        `

                }
            },
            {
                data: 'orderId',
                "render": function (data) {
                    return `
                    <td>
                        <a href="/admin/order/GetOrderDetail?idOrder=${data}" class="btn btn-primary text-nowrap w-100"><i class="bi bi-pencil-square"></i> Details</a>
                    </td>                  
                        `
                }
            },
            {
                data: 'orderId',
                "render": function (data) {
                    return `
                    <td>
                        <a onClick=Approve('/admin/order/ApproveOderStatus?idOrder=${data}') class="btn btn-primary text-nowrap w-100"><i class="bi bi-pencil-square"></i> Approve</a>
                    </td>                  
                        `
                }
            },
            {
                data: 'orderId',
                "render": function (data) {
                    return `
                   <td>
                        <a onClick=Rejected('/admin/order/RejectedOderStatus?idOrder=${data}') class="btn btn-primary text-nowrap w-100"><i class="bi bi-pencil-square"></i> Reject</a>
                    </td>                     
                        `
                }
            }
        ]
    });
}

