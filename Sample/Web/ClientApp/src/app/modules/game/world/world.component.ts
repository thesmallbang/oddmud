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


      const received = `${data.output}`
      console.log('inbound data', data);
      // set
      if (data.commandType === 0) {
        this.messages.length = 0;
      }

      // set or append
      if (data.commandType === 0 || data.commandType === 1)
        this.messages.push(received);

      // replace
      if (data.commandType === 2) {

        var subset = received.substring(received.indexOf('>') + 1);
        var word = subset.split(' ')[0];
        console.log('word to find', word);
        console.log('messages', this.messages);
        var messageIndex = this.messages.findIndex((m) => m.substring(m.indexOf('>') + 1).startsWith(word));

        // if we cant find a match just push it as a new message, otherwise replace the current one.
        if (messageIndex < 0)
          this.messages.push(received);
        else
          this.messages[messageIndex] = received;

      }

      this.worldHtml = this.messages.join('');
      this.scrollToBottom();
    });
  }

  public scrollToBottom() {
    const element = this.worldBoxRef.nativeElement;
    element.scrollIntoView(false);
  }

}
