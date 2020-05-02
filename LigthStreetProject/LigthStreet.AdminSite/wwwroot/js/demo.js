function updateLabel(value) {
    document.getElementById("sliderLabel").innerHTML = `${value}%`;
}
function demoPhotoSelected(event) {
    var selectedFile = event.target.files[0];
    var reader = new FileReader();

    var imgtag = document.getElementById("photoToDemo");
    imgtag.title = selectedFile.name;

    reader.onload = function (event) {
        imgtag.src = event.target.result;
    };

    reader.readAsDataURL(selectedFile);
}

function calcPhoto() {

    var file = document.getElementById("demophotoload").files[0];

    if (file != undefined) {

        var formData = new FormData();
        formData.append("file", file);

        let select = document.getElementById("selectValue");
        var value = select.options[select.selectedIndex].value;

        if (value == 1) {
            let lightness = document.getElementById("sliderValue").value;
            $.ajax(
                {
                    url: `${url}DemoImage/lightpixels?lightness=${lightness}`,
                    data: formData,
                    processData: false,
                    contentType: false,
                    type: "POST",
                    success: function (data) {
                        console.log(data);
                        document.getElementById("calculatedPhoto").src = `data:image/jpeg;base64,${data}`;
                    }
                });
        }
        else {
            $.ajax(
                {
                    url: `${url}DemoImage/sortpixels`,
                    data: formData,
                    processData: false,
                    contentType: false,
                    type: "POST",
                    success: function (data) {
                        console.log(data);
                        document.getElementById("calculatedPhoto").src = `data:image/jpeg;base64,${data}`;
                    }
                });
        }
    }
}