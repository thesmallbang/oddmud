import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-player-health',
  templateUrl: './player-health.component.html',
  styleUrls: ['./player-health.component.css']
})
export class PlayerHealthComponent implements OnInit {

  public position;
  public Id;
  public Name = "Guest";
  public Health = 0;
  public Mana = 100;
  public Stamina = 100;
  

  constructor() { }

  ngOnInit() {
    this.loadPosition();

  }

  public loadPosition() {
    var lastPosition = JSON.parse(localStorage.getItem("player_position"));
    if (lastPosition) {
      this.position = lastPosition;
    }
  }

  public savePosition(evt: any) {
    localStorage.setItem("player_position", JSON.stringify(evt));
  }

  public SetData(data: any) {
    this.Id = data.id;
    this.Name = data.name;
    this.Health = data.health;

  }

}
