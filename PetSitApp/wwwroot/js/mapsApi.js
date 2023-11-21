let map;

const model = window.model;
const sitterArray = model.sitters;

async function initMap() {
    // The location of Uluru
    const position = { lat: model.zipLat, lng: model.zipLng };
    // Request needed libraries.
    //@ts-ignore
    const { Map } = await google.maps.importLibrary("maps");
    const { AdvancedMarkerElement } = await google.maps.importLibrary("marker");

    // The map, centered at zip code
    map = new Map(document.getElementById("map"), {
        zoom: 11,
        center: position,
        mapId: "DEMO_MAP_ID",
    });

    let marker;

    var i = 0; len = sitterArray.length;
    while (i < len) {
        marker = new AdvancedMarkerElement({
            map: map,
            position: { lat: sitterArray[i].latitude, lng: sitterArray[i].longitude },
            title: `${sitterArray[i].firstName} ${sitterArray[i].lastName}`,
        });
        i++
    };

    
}

initMap();

