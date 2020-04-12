var directionsService = null;
var directionsRenderer = null;
var map = null;
function initMap() {
    directionsService = new google.maps.DirectionsService();
    directionsRenderer = new google.maps.DirectionsRenderer();
    var chicago = new google.maps.LatLng(41.850033, -87.6500523);
    var mapOptions = {
        zoom: 7,
        center: chicago
    }
    map = new google.maps.Map(document.getElementById('map'), mapOptions);
    directionsRenderer.setMap(map);
}

function calcRoute() {
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
            //directionsRenderer.setDirections(result);
            for (var i = 0, len = result.routes.length; i < len; i++) {
                var polylineOptionsActual = new google.maps.Polyline({
                    strokeColor: getRandomColor(),
                    strokeOpacity: 1.0,
                    strokeWeight: 6
                });
                new google.maps.DirectionsRenderer({
                    map: map,
                    directions: result,
                    routeIndex: i,
                    polylineOptions: polylineOptionsActual              
                });
            }
        }
    });
}

function getRandomColor() {
    var letters = '0123456789ABCDEF';
    var color = '#';
    for (var i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
}