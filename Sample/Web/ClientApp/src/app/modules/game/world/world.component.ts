import { Component, OnInit, ViewChild, ElementRef, ViewEncapsulation, Renderer2, OnDestroy, ComponentFactoryResolver } from '@angular/core';
import { InboundChannels } from 'src/app/communication/signal/InboundChannels';
import { GameHubService } from 'src/app/communication/signal/game-hub';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { MapInfoComponent } from './components/map-info/map-info.component';

@Component({
  selector: 'app-game-world',
  templateUrl: './world.component.html',
  styleUrls: ['./world.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class WorldComponent implements OnInit, OnDestroy {
  @ViewChild('worldbox') worldBoxRef: ElementRef;
  @ViewChild(MapInfoComponent) mapInfo: MapInfoComponent;

  public messages: string[] = [];
  public worldHtml: SafeHtml;

  public listeners: Function[] = [];


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
    this.hub.Subscribe(InboundChannels.WorldStream, (data: any) => {


      data.viewComponents.forEach((message) => {

        if (message.componentType === 0)
          this.processWindowData(message.data);
        else
          console.log('unknown message type');

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

    // build angular components from server message data

    this.mapInfo.SetData(data);
  }


}
