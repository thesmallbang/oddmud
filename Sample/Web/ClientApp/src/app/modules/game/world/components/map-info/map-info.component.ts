import { Component, OnInit, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'app-map-info',
  templateUrl: './map-info.component.html',
  styleUrls: ['./map-info.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class MapInfoComponent implements OnInit {

  public Id: number;
  public Title: string;
  public Description: string;
  public Exits: string[];


  constructor() { }

  ngOnInit() {
  }


  public SetData(data: any) {
    console.log('setting map data', data);

    this.Id = data.id;
    this.Title = data.title;
    this.Description = data.description;
    this.Exits = data.exits;


  }
}
