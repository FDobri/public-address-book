import { Component } from '@angular/core';
import { ContactDTO } from './models/contact.model';
import { HttpService } from './services/http.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  public contacts: ContactDTO[];
  public singleContact: ContactDTO;
  constructor(private httpService: HttpService){}

  public getContacts = () => {
    const route = 'https://localhost:44397/api/contact';
    this.httpService.getData(route)
    .subscribe((result) => {
      this.contacts = result as ContactDTO[];
    },
    (error) => {
      console.error(error);
    });
  }
  
  public getContact = () => {
    const route = 'https://localhost:44397/api/contact/12';
    this.httpService.getData(route)
    .subscribe((result) => {
      this.singleContact = result as ContactDTO;
    },
    (error) => {
      console.error(error);
    });
  }
}
