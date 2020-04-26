const url = "https://localhost:44360/api/"; 
const photourl = "https://imgstorage123.blob.core.windows.net/partnersfiles1/";

class Point {
    index;
    constructor(marker,photo) {
        this.photo = photo;
        this.marker = marker;
        this.latitude = marker.position.lat();
        this.longtitude = marker.position.lng();
        this.setInfoWindow.bind(this);
    }

    getInfoContent(photo) {
        let content = `<div>
                            <div class="row">
                                <div class="col-sm text-right">
                                    <a onclick="loadLinkClicked(${this.index})">
                                        Load
                                    </a>
                                    <input id="loadphoto" class="none" type="file" accept=".jpeg,.jpg,.png">
                                 </div>
                            </div>
                        <img class="pointphoto" src="${photo}">
                      </div>`;
            
        return content;
    }

    setInfoPhoto(photo) {
        let photolink = `${photourl}${photo}.jpg`;
        this.infoWindow.setContent(this.getInfoContent(photolink));
    }

    setInfoWindow(infoWindow, map) {
        this.infoWindow = infoWindow;

        this.infoWindow.setContent(this.getInfoContent(`${photourl}camera.png`));

        var marker = this.marker;
        this.marker.addListener('click', function () {
            infoWindow.open(map, marker);
        });
    }
}


function loadLinkClicked(index) {
    let point = newpoints[index];
    if (point == undefined) {
        point = points[index];
    }
    let input = document.getElementById("loadphoto");

    input.onchange = (event) => {
        var data = { latitude: point.latitude, longtitude: point.longtitude};
        getBase64(event.target.files[0])
            .then(res => {
                res = res.substring(res.indexOf(',')+1);
                var data = { latitude: point.latitude, longtitude: point.longtitude, image : res };
                (async () => {
                    const rawResponse = await fetch(`${url}Point/point`, {
                        method: 'POST',
                        headers: {
                            'Accept': 'application/json',
                            'Content-Type': 'application/json'
                        },
                        body: JSON.stringify(data)
                    });
                    const content = await rawResponse.json();

                    point.setInfoPhoto(content);
                    points.push(point);
                    newpoints.splice(newpoints.indexOf(point), 1);
                })();

            }
           );
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