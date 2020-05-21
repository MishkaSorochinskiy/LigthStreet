var blockedUsers = {
    bindDatatable: function () {
        clearArrays();
        table = $('#Blocked').DataTable({
            language: {
                searchPlaceholder: "Search records",
                search: "",
                lengthMenu: "_MENU_"
            },
            responsive: true,
            "bServerSide": true,
            "sAjaxSource": "https://localhost:5001/api/user/approved?count=",
            "fnServerData": function (sSource, aoData, fnCallback) {
                debugger;
                $.ajax({
                    type: "Get",
                    data: aoData,
                    url: sSource + aoData[4].value + "&page=" + ((aoData[3].value + aoData[4].value) / aoData[4].value - 1) + "&sEcho=" + aoData[0].value + "&status=1" + "&searchQuery=" + aoData[45].value,
                    success: fnCallback,
                    error: function (error) {
                        dangerAlert(error.responseText);
                    }
                })
            },
            "aoColumns": [
                { "mData": "email" },
                {
                    "mData": "modifiedAt",
                    "render": function (modifiedAt, type, row) {
                        return moment(modifiedAt).format('MMMM Do YYYY, h:mm:ss a');
                    }
                },
                { "mData": "createdByUserName" },
                { "mData": "firstName" },
                { "mData": "lastName" },
                {
                    "mData": "roleName",
                    "render": function (roleName) {
                        if (roleName == "GrafanaUser") {
                            return "Employee";
                        }
                        return roleName;
                    }
                },
                {
                    "mData": "tags",
                    "render": function (tags, type, row) {
                        var a = '<div class="tag" id ="' + row.userName + '"  style="display: flex; flex-wrap:wrap; width: 200px; ">';
                        for (let i = 0; i < tags.length; i++) {
                            a +=
                                '<div class="chip" id = ' + tags[i].id + '  style="position: relative;"  data-toggle="tooltip" data-placement="left" title="' + tags[i].name + '">' +
                                '<span class = "titleName">' + tags[i].name + '</span>' +
                            '<span class="closebtn" style="position: absolute; left: 15px;" onclick="approvedUsers.deleteTag(event, \'' + row.userName + '\', \'' + row.id + '\', \'' + i + '\')">&times;</span>' +
                                '</div>'
                        }
                        a += ' </div>';
                        return a;
                    }
                },
                {
                    "mData": "Status",
                    "render": function (tag, type, row, none) {
                        return '<select class="form-control" style="width:auto;" onchange = "approvedUsers.changeStatus(event, \'' + row.id + '\',\'#Blocked\')"><option selected disabled value="-1">Actions</option><option value="0">Approve</option></option><option value="2">Unregister</option></select>'
                    }
                },
            ]

        });

        $('#Blocked').on('page.dt', function () {
            clearArrays();
        });
    },
    save: function () {
        var data = { addedTagList: null, deletedTagList: deletedTags };
        $.ajax({
            type: "Post",
            contentType: "application/json",
            data: JSON.stringify(data),
            url: "https://localhost:5001/api/user/tag",
            error: function (error) {
                dangerAlert(error.responseText);
            },
            success: function () {
                deletedTags = [];
                approvedTags = [];
                $('#Blocked').DataTable().ajax.reload(null, false);
                successAlert("Tags have been deleted");
            }
        });

        document.getElementById("stickySave").style.display = "none";
        isVisible = false;
    }
}