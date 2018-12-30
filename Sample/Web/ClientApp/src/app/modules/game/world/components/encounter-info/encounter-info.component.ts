import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-encounter-info',
  templateUrl: './encounter-info.component.html',
  styleUrls: ['./encounter-info.component.css']
})
export class EncounterInfoComponent implements OnInit {

  private position;
  private entities = [];
  constructor() { }


  ngOnInit() {
    this.loadPosition();

  }

  public loadPosition() {
    var lastPosition = JSON.parse(localStorage.getItem("encounter_position"));
    if (lastPosition) {
      this.position = lastPosition;
    }
  }

  public savePosition(evt: any) {
    localStorage.setItem("encounter_position", JSON.stringify(evt));
  }

  public SetData(data: any) {
    this.entities = data.entities;
    console.log("entities", this.entities);
  }

}
