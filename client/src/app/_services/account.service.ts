import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject, map, take } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class AccountService
{
  baseUrl = environment.apiUrl + 'account/';
  currentUserSource = new ReplaySubject<User>(1);
  currentUser$ = this.currentUserSource.asObservable();

  constructor (private http: HttpClient) { }

  register(model: any)
  {
    return this.http.post<User>(this.baseUrl + "register", model).pipe(
      map((response: User) =>
      {
        if (response)
          this.setCurrentUser(response);
      })
    );
  }

  login(model: any)
  {
    return this.http.post<User>(this.baseUrl + 'login', model).pipe(
      map((response) =>
      {
        if (response)
          this.setCurrentUser(response);
      })
    );
  }

  setCurrentUser(user: User)
  {
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);

  }

  isLoggedIn(): boolean
  {
    let currentUser: User | undefined;
    this.currentUser$.pipe(take(1)).subscribe({
      next: (user) =>
      {
        if (user)
          currentUser = user;
      }
    });

    return (currentUser !== null) && (currentUser !== undefined);
  }
}
