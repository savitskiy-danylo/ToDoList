import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { AccountService } from './_services/account.service';
import { User } from './_models/user';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit
{
  title = 'To-Do-List';

  constructor (
    private titleService: Title,
    private accountService: AccountService
  )
  {
    titleService.setTitle(this.title);
  }
  ngOnInit(): void
  {
    this.setCurrentUser();
  }
  setCurrentUser()
  {
    const storage = localStorage.getItem('user');
    if (!storage) return;

    const user = JSON.parse(storage);
    this.accountService.setCurrentUser(user);
  }
}
