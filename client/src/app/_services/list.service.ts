import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { ToDoTask } from '../_models/task';
import { List } from '../_models/list';

@Injectable({
  providedIn: 'root'
})
export class ListService
{
  baseUrl = environment.apiUrl + "list/";
  constructor (private http: HttpClient) { }

  getLists()
  {
    return this.http.get<List[]>(this.baseUrl);
  }
}
