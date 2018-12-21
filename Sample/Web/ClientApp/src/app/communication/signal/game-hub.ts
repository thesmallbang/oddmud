import { HubConnection } from '@aspnet/signalr';
import * as signalR from '@aspnet/signalr';

import { Injectable } from '@angular/core';
import { CommandModel } from 'src/app/models/CommandModel';
import { InboundChannels } from './InboundChannels';
import { OutboundChannels } from './OutboundChannels';


@Injectable({
    providedIn: 'root',
})
export class GameHubService {

    private _hub: HubConnection | undefined;

    public Connect(): void {
        if (this.IsConnected()) {
            return;
        }
        this._hub = new signalR.HubConnectionBuilder()
          .withUrl('/gameHub')
       //  .withHubProtocol(new msgPack.MessagePackHubProtocol())
            .configureLogging(signalR.LogLevel.Information)
            .build();
        this._hub.start().catch(err => console.error(err.toString()));
    }

    public Subscribe(channel: InboundChannels, callback: (responseData: any) => void): void {
        this._hub.on(channel, callback);
    }

    private Invoke(channel: OutboundChannels, data: any) {
        if (!this.IsConnected()) {
            console.error('invalid hub connection state');
            return;
        }
        this._hub.invoke(channel, data);
    }

    public AddCommand(data: string) {
        const command = new CommandModel();
        command.RawCommand = data;
        this.Invoke(OutboundChannels.CommandStream, command);
    }

    public IsConnected(): boolean {
        return this._hub && this._hub.state === signalR.HubConnectionState.Connected;
    }

}


