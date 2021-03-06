import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { GameHubService } from 'src/app/communication/signal/game-hub';

@Component({
  selector: 'app-game-commands',
  templateUrl: './commands.component.html',
  styleUrls: ['./commands.component.css']
})
export class CommandsComponent implements OnInit {
  @ViewChild('commandRef') commandRef: ElementRef;

  public form: FormGroup = new FormGroup({
    command: new FormControl()
  });

  private hub: GameHubService;

  constructor(
    gameHub: GameHubService
  ) {
    this.hub = gameHub;
  }

  ngOnInit() {

  }

  sendMessage() {
    const commandControl = this.form.get('command');
    this.hub.AddCommand(commandControl.value);
    // commandControl.setValue('');

    // select the text instead of erasing it
    const inputElem = <HTMLInputElement>this.commandRef.nativeElement;
    inputElem.select();

  }

}
