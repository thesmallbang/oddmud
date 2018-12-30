import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MainComponent } from './main/main.component';
import { WorldComponent } from './world/world.component';
import { ChatComponent } from './chat/chat.component';
import { CommandsComponent } from './commands/commands.component';
import { GameRoutingModule } from './game-routing';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule, MatProgressBarModule } from '@angular/material';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MapInfoComponent } from './world/components/map-info/map-info.component';
import { PlayerHealthComponent } from './world/components/player-health/player-health.component';
import { AngularDraggableModule } from 'angular2-draggable';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { EncounterInfoComponent } from './world/components/encounter-info/encounter-info.component';


@NgModule({
  declarations: [MainComponent, WorldComponent, ChatComponent, CommandsComponent, MapInfoComponent, PlayerHealthComponent, EncounterInfoComponent],
  imports: [
    ReactiveFormsModule,
    MatButtonModule,
    MatProgressBarModule,
    FlexLayoutModule,
    PerfectScrollbarModule,
    AngularDraggableModule,
    CommonModule,
    GameRoutingModule
  ]
})
export class GameModule { }
