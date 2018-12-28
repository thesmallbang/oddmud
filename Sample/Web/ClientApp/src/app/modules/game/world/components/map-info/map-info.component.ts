import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-map-info',
  templateUrl: './map-info.component.html',
  styleUrls: ['./map-info.component.css']
})
export class MapInfoComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }


  public SetData(data: any) {
    console.log('setting map data', data);


  }
}
