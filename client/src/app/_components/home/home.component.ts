import { Component, OnInit } from '@angular/core';
import { map, take } from 'rxjs';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { ToDoTask } from 'src/app/_models/task';
import { BsModalService } from 'ngx-bootstrap/modal';
import { EditTaskComponent } from 'src/app/_component/edit-task/edit-task.component';
import { ModalsService } from 'src/app/_services/modals.service';
import { ToastrService } from 'ngx-toastr';
import { ListService } from 'src/app/_services/list.service';
import { List } from 'src/app/_models/list';

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
    private listService: ListService
  ) { }

  ngOnInit(): void
  {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: (response) =>
      {
        if (response) {
          this.user = response;
          console.log(response);

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
            list.tasks.forEach(task => this.collapseMap.set(task.id, true));
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

  showEditModal(task: ToDoTask)
  {
    this.modalService.editTask(task)?.subscribe({
      next: (editedTask) =>
      {
        if (editedTask)
          this.listService.updateTask(editedTask).subscribe(
            {
              next: () =>
              {
                this.refreshLists();
              }
            }
          );
      }
    });
  }

  deleteTask(task: ToDoTask)
  {
    this.listService.deleteTask(task).subscribe({
      next: () =>
      {
        this.refreshLists();
      }
    })
  }

}
