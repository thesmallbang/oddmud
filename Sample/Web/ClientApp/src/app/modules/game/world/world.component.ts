import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { InboundChannels } from 'src/app/communication/signal/InboundChannels';
import { GameHubService } from 'src/app/communication/signal/game-hub';

@Component({
  selector: 'app-game-world',
  templateUrl: './world.component.html',
  styleUrls: ['./world.component.css']
})
export class WorldComponent implements OnInit {
  @ViewChild('worldbox') worldBoxRef: ElementRef;

  public messages: string[] = [];
  private hub: GameHubService;
  public worldHtml: string;

  constructor(
    gameHub: GameHubService
  ) {
    this.hub = gameHub;
  }

  ngOnInit() {
    this.hub.Subscribe(InboundChannels.WorldStream, (data: any) => {
      const received = `${data}`;
      this.messages.push(received);
      this.worldHtml = this.messages.join('');
      this.scrollToBottom();
    });
  }

  public scrollToBottom() {
    const element = this.worldBoxRef.nativeElement;
    element.scrollIntoView(false);
  }

}
