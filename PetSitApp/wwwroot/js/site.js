// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function () {
    $("#date").datepicker();
});

$(function () {
    $("#start-date").datepicker();
});

$(function () {
    $("#end-date").datepicker();
});

document.getElementById('boarding-check').addEventListener('change', function() {
    var rateInput = document.getElementById('boarding-rate');
    if (this.checked) {
        rateInput.removeAttribute('disabled');
    } else {
        rateInput.setAttribute('disabled', 'disabled');
    }
});

document.getElementById('home-check').addEventListener('change', function () {
    var rateInput = document.getElementById('home-rate');
    if (this.checked) {
        rateInput.removeAttribute('disabled');
    } else {
        rateInput.setAttribute('disabled', 'disabled');
    }
});

//// Initialize and add the map
//let map;

//async function initMap() {
//    const position = { lat: 40.758896, lng: -73.985130 };
//    //Request needed libraries
//    const { Map } = await google.maps.importLibrary("maps");
//    const { AdvancedMarkerElement } = await google.maps.importLibrary("marker");

//    // The map, centered
//    map = new Map(document.getElementById("map"), {
//        zoom: 4,
//        center: position,
//        mapId: "DEMO_MAP_ID",
//    });

//    // Map's marker position
//    const marker = new AdvancedMarkerElement({
//        map: map,
//        position: position,
//        title: "NY"
//    });
//}

//initMap();

//let map;

//async function initMap() {
//    const { Map } = await google.maps.importLibrary("maps");

//    map = new Map(document.getElementById("map"), {
//        center: { lat: -34.397, lng: 150.644 },
//        zoom: 8,
//    });
//}
//await initMap();

//let map;

//function initMap() {
//    return google.maps.importLibrary("maps").then(({ Map }) => {
//        map = new Map(document.getElementById("map"), {
//            center: { lat: -34.397, lng: 150.644 },
//            zoom: 8,
//        });
//    });
//}

//initMap(); // No need for await if not in an async function


