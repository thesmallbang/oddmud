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
    this.hub.Subscribe(InboundChannels.WorldStream, (dataList: any[]) => {


      dataList.forEach((data) => {

        const received = `${data.output}`
        var relatedId = data.relatedId;


        // a set command with no id is meant to clear the map
        if (data.operationType === 0 && !relatedId) {
          this.messages.length = 0;
          console.log("clearing all .. set command with no id messages");
        }

        // set or appennd
        if (data.operationType === 0) {

          if (!relatedId) {
            console.log("pushing message set command with no id");
            this.messages.push(received);
            return;
          }

          // find the related message
          var messageIndex = this.messages.findIndex((m) => m.includes(`id='${relatedId}'`));

          // if we cant find a match just push it as a new message, otherwise replace the current one.
          if (messageIndex < 0) {
            this.messages.push(received);
            return;
          }

          var message = this.messages[messageIndex];
          var endTag = `</div><!-- ${relatedId} -->`;
          var divStart = message.indexOf(`<div id='${relatedId}'`);
          var divEnd = message.indexOf(endTag, divStart);
          var newMessage = `${message.substring(0, divStart)}${received}${message.substring(divEnd + endTag.length)}`;
          this.messages[messageIndex] = newMessage;

        }

        if (data.operationType == 1) {

          if (!relatedId) {
            this.messages.push(received);
            return;
          }

          // find the related message
          var messageIndex = this.messages.findIndex((m) => m.includes(`id='${relatedId}'`));

          // if we cant find a match just push it as a new message, otherwise replace the current one.
          if (messageIndex < 0) {
            this.messages.push(received);
            return;
          }

          var message = this.messages[messageIndex];
          var endTag = `</div><!-- ${relatedId} -->`;
          var divStart = message.indexOf(`<div id='${relatedId}'`);
          var divEnd = message.indexOf(endTag, divStart);
          var newMessage = `${message.substring(0, divEnd)}${received}${endTag}`;
          this.messages[messageIndex] = newMessage;

        }


      });



      this.worldHtml = this.messages.join('');
      this.scrollToBottom();
    });
  }

  public scrollToBottom() {
    const element = this.worldBoxRef.nativeElement;
    element.scrollIntoView(false);
  }

}
