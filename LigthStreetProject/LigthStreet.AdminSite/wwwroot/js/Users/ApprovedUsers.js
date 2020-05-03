var approvedUsers = {
    bindDatatable: function () {
        clearArrays();
        table = $('#Approved').DataTable({
            responsive: true,
            "bServerSide": true,
            "sAjaxSource": "https://localhost:5001/api/user/approved?count=",
            "fnServerData": function (sSource, aoData, fnCallback) {
                debugger;
                $.ajax({
                    type: "Get",
                    url: sSource + aoData[4].value + "&page=" + ((aoData[3].value + aoData[4].value) / aoData[4].value - 1) + "&sEcho=" + aoData[0].value + "&status=0" + "&searchQuery=" + aoData[45].value,
                    success: fnCallback,
                    error: function (error) {
                        dangerAlert(error.responseText);
                    }
                })
            },
            "aoColumns": [
                { "mData": "userName" },
                { "mData": "modifiedAt" },
                { "mData": "createdByUserName" },
                { "mData": "firstName" },
                { "mData": "lastName" },
                {
                    "mData": "roleName",
                    "render": function (roleName, type, row) {
                        if (row.userName !== "Admin") {
                            var a = '<select class="form-control" onchange = "approvedUsers.changeRole(event, \'' + row.id + '\')">';
                            if (row.roleName === null) {
                                a += '<option selected disabled value = "-1">Choose role</option>';
                            }
                            else {
                                a += '<option disabled value = "-1">Choose role</option>';
                            }
                            if (row.roleId == 1) {
                                a += '<option selected value = "1">Admin</option>';
                            }
                            else {
                                a += '<option value="1">Admin</option>';
                            }
                            if (row.roleId == 2) {
                                a += '<option selected value = "2">LogViewer</option ></select> ';
                            }
                            else {
                                a += '<option value = "2">Grafana</option ></select > ';
                            }
                            return a;
                        }
                        return row.roleName;
                    }
                },
                {
                    "mData": "tags",
                    "render": function (tags, type, row) {
                            var a = '<input style="height:24px;" type="text" class="form-control" onkeydown="approvedUsers.addTag(event, \'' + row.id + '\', \'' + row.userName + '\');" />' + '<div class="tag" id="' + row.userName + '" style="display: flex; flex-wrap:wrap; width: 200px; ">';
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
                        if (row.userName !== "Admin") {
                            return '<select class="form-control" style="width:auto;" onchange = "approvedUsers.changeStatus(event, \'' + row.id + '\', \'#Approved\')"><option selected disabled value="-1">Actions</option><option value="1">Block</option><option value="2">Unregister</option></select>'
                        }
                        return "Admin";
                    }
                },
            ],
            columnDefs: [{
                targets: 1, render: function (data) {
                    return moment(data).format('MMMM Do YYYY, h:mm:ss a');
                }
            }]

        });

        $('#Approved').on('page.dt', function () {
            clearArrays();
        });
    },

    changeRole: function (event, userId) {
        data = {UserId:userId, RoleId: event.target.value}
        $.ajax({
            type: "Post",
            contentType: "application/json",
            data: JSON.stringify(data),
            url: "/api/UserRedirected/changerole",
            error: function (error) {
                dangerAlert(error.responseText);
            },
            success: function () {
                $('#Approved').DataTable().ajax.reload(null, false);
                successAlert("Role have been changed");
            }
        });
    },

    save: function () {
        addedTagList = [];

        if (approvedTags.length !== 0) {
            for (let i = 0; i < approvedTags.length; i++) {
                addedTagList.push({
                    name: approvedTags[i].name,
                    userIds: []
                })
                for (let j = 0; j < approvedTags[i].users.length; j++) {
                    addedTagList[i].userIds.push(approvedTags[i].users[j].value)
                }
            }

        }
        var data = { addedTagList: addedTagList, deletedTagList: deletedTags };
        $.ajax({
            type: "Post",
            contentType: "application/json",
            data: JSON.stringify(data),
            url: "/api/UserRedirected/tag",
            error: function (error) {
                dangerAlert(error.responseText);
            },
            success: function () {
                deletedTags = [];
                approvedTags = [];
                $('#Approved').DataTable().ajax.reload(null, false);
                successAlert("Tags have been saved");
            }
        });

        document.getElementById("stickySave").style.display = "none";
        isVisible = false;
    },

    addTag: function (e, userId, userName) {
        if (e.key == "Enter") {
            showSaveButton();
            var tagName = e.target.value;

            var tag = approvedTags.find(x => x.name === tagName);
            if (tag === undefined) {
                tagMaxId--;
                approvedTags.push({
                    name: tagName,
                    users: []
                });
                approvedTags[approvedTags.length - 1].users.push({
                    tagId: tagMaxId,
                    value: userId
                });
            }
            else {
                if (approvedTags.find(x => x.name === tagName).users.find(x => x.value === userId) === undefined) {
                    tagMaxId--;
                    approvedTags.find(x => x.name === tagName).users.push({
                        tagId: tagMaxId,
                        value: userId
                    });
                }
                else {
                    e.target.value = null;
                    return;
                }
            }

            document.getElementById(userName).innerHTML += '<div class="chip" id = ' + tagMaxId + ' style="position: relative; background-color:  rgba(193, 247, 175, 0.62)" data-toggle="tooltip" data-placement="left" title="' + e.target.value + '">' +
                '<span>' + e.target.value + '</span>' +
                '<span class="closebtn" style="position: absolute;left: 15px;" onclick="approvedUsers.deleteTempTag(event,\'' + userName + '\' )" >&times;</span>' +
                '</div>'
            e.target.value = null;
        }
    },

    deleteTag: function (event, elementId, userId, nodeId) {
        showSaveButton();
        var tagId = document.getElementById(elementId).childNodes[nodeId].id;
        document.getElementById(elementId).childNodes[nodeId].style.backgroundColor = "rgba(255, 0, 0, 0.4)";
        document.getElementById(elementId).childNodes[nodeId].getElementsByClassName("closebtn")[0].hidden = true;
        document.getElementById(elementId).childNodes[nodeId].title = "Deleted: " + document.getElementById(elementId).childNodes[nodeId].title;
        var tag = deletedTags.find(x => x.userId === userId);
        if (tag === undefined) {
            deletedTags.push({
                userId: userId,
                tagIds: []
            });
            deletedTags[deletedTags.length - 1].tagIds.push(tagId);
        }
        else {
            deletedTags.find(x => x.userId === userId).tagIds.push(tagId);
        }
    },

    cancelChanges: function () {
        if (deletedTags.length !== 0) {
            for (let i = 0; i < deletedTags.length; i++) {
                for (let j = 0; j < deletedTags[i].tagIds.length; j++) {
                    let child = getChildElementByID(document.getElementById(deletedTags[i].userId).childNodes, deletedTags[i].tagIds[j]);
                    child.style.backgroundColor = "rgba(100,160,237, 0.62)";
                    child.getElementsByClassName("closebtn")[0].hidden = false;
                    child.title = child.getElementsByClassName("titleName")[0].innerHTML;
                }
            }
            deletedTags = [];
        }
        if (approvedTags.length !== 0) {
            for (let i = 0; i < approvedTags.length; i++) {
                for (let j = 0; j < approvedTags[i].users.length; j++) {
                    document.getElementById(approvedTags[i].users[j].tagId).remove();
                }
            }
            approvedTags = [];
        }
        document.getElementById("stickySave").style.display = "none";
        isVisible = false;
    },

    changeStatus: function (event, userId, tableId) {
        if (event.target.value != 2) {
            const data = { Status: event.target.value, userId: userId };
            $.ajax({
                type: "Post",
                contentType: "application/json",
                data: JSON.stringify(data),
                url: "/api/UserRedirected/approved/status",
                error: function (error) {
                    dangerAlert(error.responseText);
                },
                success: function () {
                    $(tableId).DataTable().ajax.reload(null, false);
                    successAlert("Status have been changed");
                }
            });
        }
        else {
            approvedUsers.unregister(userId, tableId)
        }
    },

    unregister: function (userId, tableId) {
        $.ajax({
            type: "Get",
            url: "/api/UserRedirected/approved/unAuthorize?userId=" + userId,
            error: function (error) {
                dangerAlert(error.responseText);
            },
            success: function () {
                userIndex = pendingTags.indexOf(pendingTags.find(x => x.userId === userId));
                pendingTags[userIndex] = 1;
                pendingTags.splice(userIndex, 1);
                $(tableId).DataTable().ajax.reload(null, false);
                successAlert("The user has been unregistered");
            }
        });
    },

    deleteTempTag: function (event, userName) {
        var tagId = event.target.parentElement.getAttribute("id");
        var tagName = event.target.offsetParent.title;
        var nameTagIndex = approvedTags.indexOf(approvedTags.find(x => x.name === tagName));
        var deviceIndex = approvedTags[nameTagIndex].users.indexOf(approvedTags[nameTagIndex].users.find(x => x.tagId == tagId));
        approvedTags[nameTagIndex].users.splice(deviceIndex, 1);
        document.getElementById(tagId).remove();
    }

}