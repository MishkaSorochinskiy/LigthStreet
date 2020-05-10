var pendingUsers = {
    bindDatatable: function () {
        clearArrays();
        table = $('#Pending').DataTable({
            responsive: true,
            "bServerSide": true,
            "sAjaxSource": "https://localhost:5001/api/user/pending?count=",
            "fnServerData": function (sSource, aoData, fnCallback) {
                $.ajax({
                    type: "Get",
                    data: aoData,
                    url: sSource + aoData[4].value + "&page=" + ((aoData[3].value + aoData[4].value) / aoData[4].value - 1) + "&sEcho=" + aoData[0].value + "&pendingType=0" + "&searchQuery=" + aoData[40].value,
                    success: fnCallback,
                    error: function (error) {
                        dangerAlert(error.responseText);
                    }
                })
            },
            "aoColumns": [
                { "mData": "email" },
                { "mData": "createdAt" },
                { "mData": "firstName" },
                { "mData": "lastName" },
                {
                    "mData": null,
                    "render": function (tag, type, row) {
                        return '<select class="form-control" id="selected\'' + row.id + '\'" style="width:auto;"><option selected disabled value="-1">Choose role</option><option value="1">Admin</option><option value="2">LogViewer</option></select>'
                    }
                },
                {
                    "mData": null,
                    "render": function (tag, type, row) {

                        var a = '<input style="height:24px;" type="text" class="form-control" onkeydown = "pendingUsers.addTag(event, \'' + row.id + '\');"/>' + '<div class="tag" id ="' + row.id + '"  style="display: flex; flex-wrap:wrap; width: 200px; ">';
                        for (let i = 0; i < pendingTags.length; i++) {
                            if (row.deviceId === pendingTags[i].deviceId) {
                                for (let j = 0; j < pendingTags[i].tags.length; j++) {
                                    a +=
                                        '<div class="chip" id = ' + pendingTags[i].tags[j].tagId + '  style="position: relative;"  data-toggle="tooltip" data-placement="left" title="' + pendingTags[i].tags[j].value + '">' +
                                        '<span class = "titleName">' + pendingTags[i].tags[j].value + '</span>' +
                                        '<span class="closebtn" style="position: absolute; left: 15px;" onclick="pendingUsers.deleteTempTag(event, \'' + row.id + '\')">&times;</span>' +
                                        '</div>'
                                }
                            }
                        }
                        a += ' </div>';
                        return a;
                    }
                },
                {
                    "mData": "Status",
                    "render": function (tag, type, row) {
                        return '<select class="form-control"  id="status\'' + row.id + '\'" style="width:auto;" onchange = "pendingUsers.changeStatus(event, \'' + row.id + '\')"><option selected disabled value="-1">Actions</option><option value="0">Approve</option><option value="1">Block</option></option><option value="2">Unregister</option></select>'
                    }
                },
            ],
            columnDefs: [{
                targets: 1, render: function (data) {
                    return moment(data).format('MMMM Do YYYY, h:mm:ss a');
                }
            }]
        });
    },

    changeStatus: function (event, userId) {
        if (event.target.value != 2) {
            if (document.getElementById("selected'" + userId + "'").value != -1) {
                var roleid = document.getElementById("selected'" + userId + "'").value;
                var data = { Status: event.target.value, UserId: userId, Tags: pendingUsers.getTags(userId), RoleId: roleid };
                $.ajax({
                    type: "Post",
                    contentType: "application/json",
                    data: JSON.stringify(data),
                    url: "https://localhost:5001/api/user/pending/status",
                    error: function (error) {
                        dangerAlert(error.responseText);
                    },
                    success: function () {
                        userIndex = pendingTags.indexOf(pendingTags.find(x => x.userId === userId));
                        pendingTags[userIndex] = 1;
                        pendingTags.splice(userIndex, 1);
                        $('#Pending').DataTable().ajax.reload(null, false);
                        successAlert("Status have been changed");
                    }
                });
            }
            else {
                document.getElementById("status'" + userId + "'").value = -1;
                warningAlert("Please choose role before action");
            }
        }
        else {
            pendingUsers.unregister(userId);
        }
    },

    unregister: function (userId) {
        $.ajax({
            type: "Get",
            url: "https://localhost:5001/api/user/pending/unAuthorize?userId=" + userId,
            error: function (error) {
                dangerAlert(error.responseText);
            },
            success: function () {
                userIndex = pendingTags.indexOf(pendingTags.find(x => x.userId === userId));
                pendingTags[userIndex] = 1;
                pendingTags.splice(userIndex, 1);
                $('#Pending').DataTable().ajax.reload(null, false);
                successAlert("The user has been unregistered");
            }
        });
    },

    getTags: function (userId) {
        tags = [];
        var userIndex = pendingTags.indexOf(pendingTags.find(x => x.userId === userId));
        if (userIndex !== -1) {
            for (let i = 0; i < pendingTags[userIndex].tags.length; i++) {
                tags.push(pendingTags[userIndex].tags[i].value);
            }
            return tags;
        }
        else
            return null;
    },

    addTag: function (e, userId) {
        if (e.key == "Enter") {
            var tagName = e.target.value;

            var tag = pendingTags.find(x => x.userId === userId);
            if (tag === undefined) {
                tagMaxId--;
                pendingTags.push({
                    userId: userId,
                    tags: []
                });
                pendingTags[pendingTags.length - 1].tags.push({
                    tagId: tagMaxId,
                    value: tagName
                });
            }
            else {
                if (pendingTags.find(x => x.userId === userId).tags.find(x => x.value === tagName) === undefined) {
                    tagMaxId--;
                    pendingTags.find(x => x.userId === userId).tags.push({
                        tagId: tagMaxId,
                        value: tagName
                    });
                }
                else {
                    e.target.value = null;
                    return;
                }
            }

            document.getElementById(userId).innerHTML += '<div class="chip" id = ' + tagMaxId + ' data-toggle="tooltip" data-placement="left" title="' + e.target.value + '">' +
                '<span>' + e.target.value + '</span>' +
                '<span class="closebtn" style="position: absolute;left: 15px;" onclick="pendingUsers.deleteTempTag(event,\'' + userId + '\')" >&times;</span>' +
                '</div>'
            e.target.value = null;
        }
    },

    deleteTempTag: function (event, userId) {
        var tagId = event.target.parentElement.getAttribute("id");
        var userIndex = pendingTags.indexOf(pendingTags.find(x => x.userId === userId));
        var nameTagIndex = pendingTags[userIndex].tags.indexOf(pendingTags[userIndex].tags.find(x => x.tagId == tagId));
        pendingTags[userIndex].tags.splice(nameTagIndex, 1);
        document.getElementById(tagId).remove();
    }
}