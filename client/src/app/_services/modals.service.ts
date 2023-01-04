import { Injectable } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { EditTaskComponent } from '../_component/edit-task/edit-task.component';
import { ToDoTask } from '../_models/task';
import { Observable, map, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ModalsService
{
  private editModalRef?: BsModalRef<EditTaskComponent>;
  constructor (private modalService: BsModalService) { }

  modalTask(task: ToDoTask)
  {
    const config = {
      initialState: {
        inputTask: task
      }
    }

    this.editModalRef = this.modalService.show(EditTaskComponent, config);
    return this.editModalRef.onHidden?.pipe(
      map(() =>
      {
        return this.editModalRef!.content!.resultTask;
      })
    );
  }

}
