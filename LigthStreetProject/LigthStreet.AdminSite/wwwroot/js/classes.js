class Point {

    constructor(marker,photo) {
        this.photo = photo;
        this.marker = marker;
        this.latitude = marker.position.lat();
        this.longitude = marker.position.lng();
        this.setInfoWindow.bind(this);
    }

    getInfoContent(photo) {
        let content = `<div>
                            <div class="row">
                                <div class="col-sm text-right">
                                    <a onclick="loadLinkClicked(${this})">
                                        Load
                                    </a>
                                    <input id="loadphoto" class="none" type="file" accept=".jpeg,.jpg,.png">
                                 </div>
                            </div>
                        <img class="pointphoto" src="${photo}">
                      </div>`;
            
        return content;
    }

    setInfoWindow(infoWindow,map) {
        this.infoWindow = infoWindow;
        this.infoWindow.content = getInfoContent();
        var marker = this.marker;
        this.marker.addListener('click', function () {
            infoWindow.open(map, marker);
        });
    }
}


function loadLinkClicked(point) {
    console.log(point);
    let input = document.getElementById("loadphoto");

    input.onchange = (event) => {
        getBase64(event.target.files[0])
            .then(res => {
                console.log(res);
            });
    }

    input.click();
}


function getBase64(file) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = () => resolve(reader.result);
        reader.onerror = error => reject(error);
    });
}