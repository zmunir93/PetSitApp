let map;


console.log(window.model);

const model = window.model;
const sitterArray = model.sitters;
console.log(sitterArray);



function initMap() {
    map = new google.maps.Map(document.getElementById("map"), {
        center: { lat: model.zipLat, lng: model.zipLng }, //{ lat: -34.397, lng: 150.644 },
        zoom: 8,
    });
}

window.initMap = initMap;

