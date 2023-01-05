import { Component, OnInit } from '@angular/core';
import { take } from 'rxjs';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { ToDoTask } from 'src/app/_models/task';
import { ModalsService } from 'src/app/_services/modals.service';
import { ToastrService } from 'ngx-toastr';
import { ListService } from 'src/app/_services/list.service';
import { TaskService } from 'src/app/_services/task.service';
import { List } from 'src/app/_models/list';
import { BsNavigationDirection } from 'ngx-bootstrap/datepicker/models';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit
{
  user: User = {} as User;
  collapseMap: Map<string, boolean> = new Map<string, boolean>();
  constructor (
    private accountService: AccountService,
    private modalService: ModalsService,
    private toastr: ToastrService,
    private listService: ListService,
    private taskService: TaskService
  ) { }

  ngOnInit(): void
  {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: (response) =>
      {
        if (response) {
          this.user = response;
        }
      }
    }
    );
    this.refreshLists();

  }

  refreshLists()
  {
    this.listService.getLists().subscribe({
      next: (result) =>
      {
        if (result) {
          let newLists = result;
          this.collapseMap.clear();
          newLists.forEach(list =>
          {
            list.tasks.sort((a, b) => this.sortTasks(a, b))
            list.tasks.forEach(task =>
            {
              this.collapseMap.set(task.id, true);
            });
          })
          this.user.lists = newLists;
        }
      }
    });
  }

  sortTasks(a: ToDoTask, b: ToDoTask)
  {
    if (a.index > b.index)
      return 1;

    if (a.index < b.index)
      return -1;

    return 0;
  }

  getCollapse(id: string): boolean
  {
    let value = this.collapseMap.get(id);
    if (value == undefined) {
      return false;
    }
    return value;
  }

  changeState(id: string)
  {
    this.collapseMap.set(id, !this.collapseMap.get(id));
  }

  getStyles(task: ToDoTask): string
  {

    if (task.isCompleted) return "green";
    let expiry = new Date(task.expiryDate);
    let current = new Date();
    if (expiry.getTime() < current.getTime()) return "red";
    return "yellow";
  }

  getText(task: ToDoTask): string
  {
    if (task.isCompleted) return "OK";
    let expiry = new Date(task.expiryDate);
    let current = new Date();
    if (expiry.getTime() < current.getTime()) return "OVERDUE";
    return "PENDING...";
  }

  editTaskModal(task: ToDoTask)
  {
    this.modalService.modalTask(task, "Edit Task: ")?.subscribe({
      next: (editedTask) =>
      {
        if (editedTask && !this.compareTasks(task, editedTask)) {
          this.taskService.updateTask(editedTask).subscribe(
            {
              next: () =>
              {
                this.refreshLists();
              }
            }
          );
        }
      }
    });
  }

  addTaskModal(list: List)
  {
    let task: ToDoTask = {} as ToDoTask;
    task.title = "Title";
    task.isCompleted = false;
    task.listId = list.id;
    task.description = "Who is W.W.?";
    task.createdAt = new Date();
    task.expiryDate = new Date();
    task.expiryDate.setDate(task.createdAt.getDate() + 1);
    task.index = list.tasks.length;
    this.modalService.modalTask(task, "New Task: ")?.subscribe({
      next: (addedTask) =>
      {
        if (addedTask && !this.compareTasks(addedTask, task)) {
          this.taskService.addTask(addedTask).subscribe(
            {
              next: () =>
              {
                this.refreshLists();
              }
            }
          );
        }
      }
    });
  }

  deleteTask(task: ToDoTask)
  {
    this.taskService.deleteTask(task).subscribe({
      next: () =>
      {
        this.refreshLists();
      }
    })
  }

  addListModal()
  {
    let list: List = {} as List;
    list.title = "Title";
    list.description = "Description";
    list.userId = this.user.id;
    this.modalService.modalList(list, "New List: ")?.subscribe({
      next: (addedList) =>
      {
        if (addedList && !this.compareLists(addedList, list)) {
          this.listService.addList(addedList).subscribe({
            next: () =>
            {
              this.refreshLists();
            }
          })
        }
      }
    })
  }

  editListModal(list: List)
  {
    this.modalService.modalList(list, "Edit Task: ")?.subscribe({
      next: (editedList) =>
      {
        if (editedList && !this.compareLists(list, editedList)) {
          this.listService.updateList(editedList).subscribe(
            {
              next: () =>
              {
                this.refreshLists();
              }
            }
          );
        }
      }
    });
  }

  deleteList(list: List)
  {
    this.listService.deleteList(list).subscribe({
      next: () =>
      {
        this.refreshLists();
      }
    })
  }

  compareTasks(a: ToDoTask, b: ToDoTask): boolean
  {
    if (new Date(a.expiryDate).toUTCString() !== new Date(b.expiryDate).toUTCString() || new Date(a.createdAt).toUTCString() !== new Date(b.createdAt).toUTCString()) {
      return false
    };

    return a.description === b.description &&
      a.id === b.id &&
      a.index === b.index &&
      a.isCompleted === b.isCompleted &&
      a.listId === b.listId &&
      a.title === b.title
  }

  compareLists(a: List, b: List): boolean
  {
    return a.title === b.title &&
      a.description === b.description &&
      a.userId === b.userId;
  }
}


