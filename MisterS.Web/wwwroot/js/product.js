$(document).ready(function () {
    $('#producttable').DataTable({
        "processing": true,
        "serverside": false,
        "filter": true,
        "orderMulti": true
    });
});