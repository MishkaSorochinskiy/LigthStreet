var directionsService = null;
var directionsRenderer = null;
var clickListenerHandler = null;
var map = null;

var directions = [];

var points = [];
var iseditable = false;
var isadmin = true;

function initMap() {
    if (isadmin) {
        document.getElementById("editbtn").style.visibility = "visible";
    }

    directionsService = new google.maps.DirectionsService();
    directionsRenderer = new google.maps.DirectionsRenderer();
    var lviv = new google.maps.LatLng(49.84070662559602, 24.026379088646518);
    var mapOptions = {
        zoom: 13,
        center: lviv,
        
    }
    map = new google.maps.Map(document.getElementById('map'), mapOptions);
    directionsRenderer.setMap(map);
}

function calcRoute() {
    clearRoutes();

    var start = document.getElementById('start').value;
    var end = document.getElementById('end').value;
    var request = {
        origin: start,
        destination: end,
        travelMode: 'DRIVING',
        provideRouteAlternatives: true
    };
    directionsService.route(request, function (result, status) {
        if (status == 'OK') {
            for (var i = 0, len = result.routes.length; i < len; i++) {
                var polylineOptionsActual = new google.maps.Polyline({
                    strokeColor: getRandomColor(),
                    strokeOpacity: 1.0,
                    strokeWeight: 6
                });

                directions.push(new google.maps.DirectionsRenderer({
                    map: map,
                    directions: result,
                    routeIndex: i,
                    polylineOptions: polylineOptionsActual
                }));
            }
        }
        else {
            alert(`request is invalid: ${status}`);
        }
    });
}

function clickListener(event) {
    var latitude = event.latLng.lat();
    var longitude = event.latLng.lng();

    var marker = new google.maps.Marker({
        position: { lat: latitude, lng: longitude },
        map: map
    });

    let newpoint = new Point(marker);
    newpoint.index = points.length;
    newpoint.setInfoWindow(new google.maps.InfoWindow(), map);

    points.push(newpoint);
}

function clearRoutes() {
    for (let i = 0; i < directions.length; ++i) {
        directions[i].setMap(null);
    }
}

function switchEdit() {
    if (isadmin) {
        if (iseditable) {
            iseditable = false;
            document.getElementById("savebtn").style.visibility = "hidden";
            clickListenerHandler.remove();
            for (let i = 0; i < points.length; ++i) {
                points[i].circle.setMap(null);
            }
        }
        else {
            iseditable = true;
            document.getElementById("savebtn").style.visibility = "visible";
            clickListenerHandler = map.addListener("click", clickListener);
        }
    }
}

function saveMarkers() {
    document.getElementById("savebtn").style.visibility = "hidden";

    //request to server

    switchEdit();
}

function getMarkers() {

    //request to server

}

function getRandomColor() {
    var letters = '0123456789ABCDEF';
    var color = '#';
    for (var i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
}