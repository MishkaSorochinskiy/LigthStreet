var tasks = {
    bindDatatable: function (dotNetReference) {
        clearArrays();
        table = $('#Tasks').DataTable({
            language: {
                searchPlaceholder: "Search records",
                search: "",
                lengthMenu: "_MENU_"
            },
            responsive: true,
            "bServerSide": true,
            "sAjaxSource": "https://localhost:5001/api/review/reviews?count=",
            "fnServerData": function (sSource, aoData, fnCallback) {
                debugger;
                $.ajax({
                    type: "Get",
                    url: sSource + aoData[4].value + "&page=" + ((aoData[3].value + aoData[4].value) / aoData[4].value - 1) + "&sEcho=" + aoData[0].value + "&status=0" + "&searchQuery=" + aoData[30].value,
                    success: fnCallback,
                    error: function (error) {
                        dangerAlert(error.responseText);
                    }
                })
            },
            "aoColumns": [
                {
                    "className": 'details-control',
                    "orderable": false,
                    "data": null,
                    "defaultContent": ''
                },
                { "mData": "subject" },
                { "mData": "createdBy" },
                { "mData": "applyOn" },
                {
                    "mData": "state",
                    "render": function (state) {
                        if (state == 0) {
                            return "Active";
                        }
                        else if (state == 1) {
                            return "Blocked";
                        }
                        else {
                            return "Postpone";
                        }
                    }
                }
            ]
        });

        $('#Tasks tbody').on('click', 'td.details-control', function () {
            var tr = $(this).closest('tr');
            var row = table.row(tr);
            dotNetReference.invokeMethodAsync('AddText', row.data());
        });
    }
}