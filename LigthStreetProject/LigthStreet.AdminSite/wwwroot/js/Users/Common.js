var approvedTags = [];

var pendingTags = []

var deletedTags = [];

var tagMaxId = 0;

var table = null;

var DeviceId = "";

var isVisible = false;

function clearArrays() {
    pendingTags = [];
    approvedTags = [];
    deletedTags = [];
}

function showSaveButton() {
    if (isVisible === false) {
        document.getElementById("stickySave").style.display = "block";
        isVisible = true
    }
}