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

  updateTask(task: ToDoTask)
  {
    return this.http.put<List>(this.baseUrl, task);
  }

  getLists()
  {
    return this.http.get<List[]>(this.baseUrl);
  }

  deleteTask(task: ToDoTask)
  {
    return this.http.delete(this.baseUrl + `delete-task/?id='${task.id}'`);
  }
}
