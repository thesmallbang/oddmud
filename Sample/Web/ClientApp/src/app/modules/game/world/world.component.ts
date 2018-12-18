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
      // set
      if (data.commandType === 0) {
        this.messages.length = 0;
      }

      // set or append
      if (data.commandType === 0 || data.commandType === 1)
        this.messages.push(received);

      // replace
      if (data.commandType === 2) {

        var tofind = data.replaceId;
        var messageIndex = this.messages.findIndex((m) => m.includes(`id='${tofind}'`)) ;

        // if we cant find a match just push it as a new message, otherwise replace the current one.
        if (messageIndex < 0)
          this.messages.push(received);
        else {
          // we need to replace the specific container/div with the inbound one in our message

          var message = this.messages[messageIndex];
          var endTag = `</div><!-- ${tofind} -->`;
          var divStart = message.indexOf(`<div id='${tofind}'`);
          var divEnd = message.indexOf(endTag, divStart);
          var newMessage = `${message.substring(0, divStart)}${received}${message.substring(divEnd + endTag.length)}`;
          this.messages[messageIndex] =  newMessage;
        }
          

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
