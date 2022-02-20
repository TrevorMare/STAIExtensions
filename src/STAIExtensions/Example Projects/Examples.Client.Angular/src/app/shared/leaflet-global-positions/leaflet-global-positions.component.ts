import { Component, OnInit } from '@angular/core';
import { circle, latLng, Marker, MarkerClusterGroup, tileLayer } from 'leaflet';
import { environment } from 'src/environments/environment';


export class positionalMarker {

  lat: number;
  lng: number;


}



@Component({
  selector: 'shared-leaflet-global-positions',
  templateUrl: './leaflet-global-positions.component.html',
  styleUrls: ['./leaflet-global-positions.component.scss']
})
export class LeafletGlobalPositionsComponent implements OnInit {

  options: any = null;

  circle = {
    id: 'circle',
    name: 'Circle',
    enabled: true,
    layer: circle([ 46.95, -122 ], { radius: 5000 })
  };

  markerClusterGroup: MarkerClusterGroup;
  markerClusterData = [];


  constructor() { 
    var tileLayerUrl: string = environment.mapBoxEnabled === true ? 'http://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png' : `https://api.mapbox.com/styles/v1/${environment.mapBoxUserName}/${environment.mapBoxId}/tiles/256/{z}/{x}/{y}@2x?access_token=${environment.mapBoxKey}`;
    var mapLayerAttribution = environment.mapBoxEnabled === true ? '...' : 'Map data &copy; <a href=&quot;https://www.openstreetmap.org/&quot;>OpenStreetMap</a> contributors, <a href=&quot;https://creativecommons.org/licenses/by-sa/2.0/&quot;>CC-BY-SA</a>, Imagery &copy; <a href=&quot;https://www.mapbox.com/&quot;>Mapbox</a>';

    this.markerClusterGroup = new MarkerClusterGroup({removeOutsideVisibleBounds: true});
    this.markerClusterGroup.addLayer(new Marker([ 46.95, -122 ]));

    this.options = {
      layers: [
          tileLayer(tileLayerUrl, { maxZoom: 18, attribution: mapLayerAttribution },
        ),
        this.markerClusterGroup.getLayers()
      ],
      zoom: 2,
      center: latLng(46.879966, -121.726909)
    };
  

  }


  ngOnInit(): void {
  }

}
