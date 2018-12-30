import { Component, OnInit, ViewChild, ElementRef, ViewEncapsulation, Renderer2, OnDestroy, ComponentFactoryResolver } from '@angular/core';
import { InboundChannels } from 'src/app/communication/signal/InboundChannels';
import { GameHubService } from 'src/app/communication/signal/game-hub';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { MapInfoComponent } from './components/map-info/map-info.component';
import { PlayerHealthComponent } from './components/player-health/player-health.component';
import { EncounterInfoComponent } from './components/encounter-info/encounter-info.component';

@Component({
  selector: 'app-game-world',
  templateUrl: './world.component.html',
  styleUrls: ['./world.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class WorldComponent implements OnInit, OnDestroy {
  @ViewChild('worldbox') worldBoxRef: ElementRef;
  @ViewChild(MapInfoComponent) mapInfo: MapInfoComponent;
  @ViewChild(PlayerHealthComponent) playerInfo: PlayerHealthComponent;
  @ViewChild(EncounterInfoComponent) encounterInfo: EncounterInfoComponent;


  public messages: string[] = [];
  public worldHtml: SafeHtml;

  public listeners: Function[] = [];
  private position;
  private size;


  constructor(
    private hub: GameHubService,
    public renderer: Renderer2,
    private componentFactoryResolver: ComponentFactoryResolver
  ) {

  }

  ngOnDestroy() {
    this.listeners.forEach((listener) => {
      listener();
    });
  }

  ngOnInit() {

    this.loadWindowStore();

    this.hub.Subscribe(InboundChannels.WorldStream, (data: any) => {


      data.viewComponents.forEach((message) => {


        switch (message.componentType) {
          case 0:
            this.processWindowData(message.data);
            break;
          case 1:
            this.processPlayerData(message.data);
            break;
          case 4:
            this.processEncounterData(message.data);
            break;
        default:
            console.log('unknown message type');
      }
        

      });

this.scrollToBottom();
    });
  }

  public scrollToBottom() {
  const element = this.worldBoxRef.nativeElement;
  element.scrollIntoView(false);
}

  // callback from link items
  public command(cmd: string) {

  this.hub.AddCommand(cmd);
}


  private processWindowData(data) {
  this.mapInfo.SetData(data);
}
  private processPlayerData(data) {
  this.playerInfo.SetData(data);
}

  public loadWindowStore() {
  var lastPosition = JSON.parse(localStorage.getItem("main_position"));
  var lastSize = JSON.parse(localStorage.getItem("main_size"));

  if (lastPosition) {
    this.position = lastPosition;
    console.log("loaded position", this.position);
  }
  if (lastSize) {
    this.size = lastSize;
  }
}

  public savePosition(evt: any) {
  console.log("saving position", evt);
  localStorage.setItem("main_position", JSON.stringify(evt));
}

  public saveSize(evt: any) {
  console.log("saving size", evt);
  localStorage.setItem("main_size", JSON.stringify(evt));
}

  private processEncounterData(data: any) {
    console.log("encounter", data);
    this.encounterInfo.SetData(data);
  }


}
