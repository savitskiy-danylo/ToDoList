import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ToDoTask } from '../_models/task';

@Injectable({
  providedIn: 'root'
})
export class TaskService
{

  baseUrl = environment.apiUrl + "task/";
  constructor (private http: HttpClient) { }

  updateTask(task: ToDoTask)
  {
    return this.http.put(this.baseUrl, task);
  }


  deleteTask(task: ToDoTask)
  {
    return this.http.delete(this.baseUrl + `?id='${task.id}'`);
  }

  addTask(task: ToDoTask)
  {
    return this.http.put<ToDoTask>(this.baseUrl + 'add-task', task);
  }
}
