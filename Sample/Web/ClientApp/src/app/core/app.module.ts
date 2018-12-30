import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from '../nav-menu/nav-menu.component';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatButtonModule, MatProgressBarModule } from '@angular/material';
import { FlexLayoutModule, BREAKPOINT } from '@angular/flex-layout';
import { GameHubService } from '../communication/signal/game-hub';
import { FullLayoutComponent } from '../layout/full-layout/full-layout.component';
import { AngularDraggableModule } from 'angular2-draggable';
import {
  PerfectScrollbarModule, PerfectScrollbarConfigInterface,
  PERFECT_SCROLLBAR_CONFIG
} from 'ngx-perfect-scrollbar';

const PRINT_BREAKPOINTS = [{
  alias: 'xs.print',
  suffix: 'XsPrint',
  mediaQuery: 'print and (max-width: 297px)',
  overlapping: false
}];

const DEFAULT_PERFECT_SCROLLBAR_CONFIG: PerfectScrollbarConfigInterface = {
  wheelPropagation: true
};



@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    FullLayoutComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    BrowserAnimationsModule,
    FlexLayoutModule.withConfig({ useColumnBasisZero: false }),
    HttpClientModule,
    AngularDraggableModule,
    MatProgressBarModule,
    MatButtonModule,
    PerfectScrollbarModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      {
        path: '',
        redirectTo: 'game',
        pathMatch: 'full',
      },
      {
        path: '',
        component: FullLayoutComponent,
        data: {
          title: 'nav.menu.home'
        },
        children: [
          {
            path: 'game',
            loadChildren: '../../app/modules/game/game.module#GameModule'
          }
        ]
      }
    ])
  ],
  providers: [
    { provide: BREAKPOINT, useValue: PRINT_BREAKPOINTS, multi: true },
    GameHubService,
    {
      provide: PERFECT_SCROLLBAR_CONFIG,
      useValue: DEFAULT_PERFECT_SCROLLBAR_CONFIG
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
