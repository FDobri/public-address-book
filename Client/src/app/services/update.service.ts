import { Injectable } from '@angular/core';
import { HttpTransportType, HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { ContactDTO } from '../models/contact.model';

@Injectable({
  providedIn: 'root'
})
export class UpdateService {
  public data: ContactDTO[]

  hubUrl = environment.hubUrl;
  public hubConnection: HubConnection;
  constructor() { }

  createHubConnection(){
    this.hubConnection = new HubConnectionBuilder()
    .configureLogging(LogLevel.Debug)
    .withUrl(this.hubUrl + 'update', {
      skipNegotiation: true,
      transport: HttpTransportType.WebSockets
    })
    .withAutomaticReconnect()
    .build();

    this.hubConnection
      .start()
      .catch(error => console.log(error));

    this.hubConnection.on('UpdateContacts', (data) => {
      this.data = data;
      console.log('Contacts updated.');
    });
  }
}
