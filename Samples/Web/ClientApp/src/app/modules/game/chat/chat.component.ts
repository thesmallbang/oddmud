import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { GameHubService } from 'src/app/communication/signal/game-hub';
import { InboundChannels } from 'src/app/communication/signal/InboundChannels';

@Component({
  selector: 'app-game-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit {
  @ViewChild('chatbox') chatBoxRef: ElementRef;


  public messages: string[] = [];

  private hub: GameHubService;

  constructor(
    gameHub: GameHubService
  ) {
    this.hub = gameHub;
  }

  ngOnInit() {
    this.hub.Subscribe(InboundChannels.ChatStream, (data: any) => {
      const received = `${data}`;
      this.messages.push(received);
    });
  }

  public scrollChatToBottom() {
    const chatElement = this.chatBoxRef.nativeElement;
    chatElement.scrollIntoView(false);
  }


}
