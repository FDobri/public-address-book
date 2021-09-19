import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ContactDTO } from './models/contact.model';
import { HttpService } from './services/http.service';
import { UpdateService } from './services/update.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  // public contacts: ContactDTO[];
  public singleContact: ContactDTO;
  constructor(private httpService: HttpService, public updateService: UpdateService) {}

  ngOnInit() {
    this.updateService.createHubConnection();
  }

  public getContacts = () => {
    const route = environment.apiUrl + 'contact';
    this.httpService.getData(route)
    .subscribe((result) => {
      this.updateService.data = result as ContactDTO[];
    },
    (error) => {
      console.error(error);
    });
  }
}
