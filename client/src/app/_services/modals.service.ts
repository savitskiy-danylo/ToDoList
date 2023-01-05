import { Injectable } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { EditTaskComponent } from '../_component/edit-task/edit-task.component';
import { ToDoTask } from '../_models/task';
import { Observable, map, of } from 'rxjs';
import { List } from '../_models/list';
import { ListModalComponent } from '../_components/list-modal/list-modal.component';

@Injectable({
  providedIn: 'root'
})
export class ModalsService
{
  private taskModalRef?: BsModalRef<EditTaskComponent>;
  private listModalRef?: BsModalRef<ListModalComponent>;

  constructor (private modalService: BsModalService) { }

  modalTask(task: ToDoTask, title: string)
  {
    const config = {
      initialState: {
        inputTask: task,
        title: title
      }
    };

    this.taskModalRef = this.modalService.show(EditTaskComponent, config);
    return this.taskModalRef.onHidden?.pipe(
      map(() =>
      {
        return this.taskModalRef!.content!.resultTask;
      })
    );
  }

  modalList(list: List, title: string)
  {
    const config = {
      initialState: {
        inputList: list,
        title: title
      }
    };

    this.listModalRef = this.modalService.show(ListModalComponent, config);
    return this.listModalRef.onHidden?.pipe(
      map(() =>
      {
        return this.listModalRef!.content!.resultList;
      })
    )
  }

}
