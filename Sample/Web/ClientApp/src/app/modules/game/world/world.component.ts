import { Component, OnInit, ViewChild, ElementRef, ViewEncapsulation } from '@angular/core';
import { InboundChannels } from 'src/app/communication/signal/InboundChannels';
import { GameHubService } from 'src/app/communication/signal/game-hub';

@Component({
  selector: 'app-game-world',
  templateUrl: './world.component.html',
  styleUrls: ['./world.component.css'],
  encapsulation: ViewEncapsulation.None,
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

      if (data.commandType = "Set")
        this.messages.length = 0;

      const received = `${data.output}`;
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
