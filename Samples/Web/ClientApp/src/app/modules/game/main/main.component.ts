import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { InboundChannels } from 'src/app/communication/signal/InboundChannels';
import { GameHubService } from 'src/app/communication/signal/game-hub';
import { FormGroup, FormControl } from '@angular/forms';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent implements OnInit {

  private hub: GameHubService;

  constructor(
    gameHub: GameHubService
  ) {
    this.hub = gameHub;
  }

  // just connect to our hub so all the other components can use it.
  // "starting our game client" basically
  ngOnInit(): void {
    if (!this.hub.IsConnected()) {
      this.hub.Connect();
    }

  }


}
