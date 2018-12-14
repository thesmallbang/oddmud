import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MainComponent } from './main/main.component';
import { WorldComponent } from './world/world.component';
import { ChatComponent } from './chat/chat.component';
import { CommandsComponent } from './commands/commands.component';
import { GameRoutingModule } from './game-routing';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material';
import { FlexLayoutModule } from '@angular/flex-layout';

@NgModule({
  declarations: [MainComponent, WorldComponent, ChatComponent, CommandsComponent],
  imports: [
    ReactiveFormsModule,
    MatButtonModule,
    FlexLayoutModule,
    CommonModule,
    GameRoutingModule
  ]
})
export class GameModule { }
