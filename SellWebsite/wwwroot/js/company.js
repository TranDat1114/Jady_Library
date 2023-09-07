
$(document).ready(function () {
    loadDataTable();
});
function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/company/getall' },
        "columns": [
            { data: 'companyId' },
            { data: 'name' },
            { data: 'address' },
            { data: 'phoneNumber' },
            { data: 'email' },
            {
                data: 'companyId',
                "render": function (data) {
                    return `
                    <td>
                        <a href="/admin/company/details?id=${data}" class="btn btn-primary text-nowrap w-100"><i class="bi bi-pencil-square"></i> Details</a>
                    </td>                  
                        `
                }
            },
            {
                data: 'companyId',
                "render": function (data) {
                    return `
                    <td>
                        <a href="/admin/company/upsert?id=${data}" class="btn btn-primary text-nowrap w-100"><i class="bi bi-pencil-square"></i> Edit</a>
                    </td>                  
                        `
                }
            },
            {
                data: 'companyId',
                "render": function (data) {
                    return `
                    <td>
                        <a onClick=Delete('/admin/company/delete?id=${data}') class="btn btn-danger text-nowrap w-100"><i class="bi bi-trash3-fill"></i> Delete</a>
                    </td>                 
                        `
                }
            }
        ]
    });
}

