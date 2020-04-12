var api_key = "AIzaSyBN2flaovRh2XfVjhrADu7F7r-mxZSJ6iA";
function initMap() {
    // The location of Uluru
    var uluru = { lat: -25.344, lng: 131.036 };
    // The map, centered at Uluru
    var map = new google.maps.Map(
        document.getElementById('map'), { zoom: 4, center: uluru });
    // The marker, positioned at Uluru
    //var image = "https://encrypted-tbn1.gstatic.com/images?q=tbn:ANd9GcQxFoh469eOsZQkuPOLpZn3R6yyIExkZCxOxf4ywfeY3v330EwP3Q";
    //var marker = new google.maps.Marker({
    //    position: uluru,
    //    map: map,
    //    icon: image
    //});
    //var marker = new google.maps.Marker({ position: uluru, icon:"", map: map });
}

function getRouteValues() {
    let origin = document.getElementById("origin").value;
    let destination = document.getElementById("destination").value;
  
    let url = `https://maps.googleapis.com/maps/api/directions/json?` +
        `origin=${origin}&destination=${destination}&key=${api_key}`;

    //$.ajax({
    //    type: "Get",
    //    url: url,
    //    dataType: 'json',
    //    success: function () {
    //        console.log("Hello");
    //    },
    //    failure: function (Error, errr, error) {
    //        var a = error;
    //    }
    //})

    $.ajax({
        url: "https://maps.googleapis.com/maps/api/directions/json",
        type: "GET",
        data: {
            origins: origin,
            destination: value,
            key: api_key
        },
        success: function (data) {
            console.log(data);
        }
    });
}

