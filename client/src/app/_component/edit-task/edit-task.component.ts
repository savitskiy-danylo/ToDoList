import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ToDoTask } from 'src/app/_models/task';
import { ListService } from 'src/app/_services/list.service';

@Component({
  selector: 'app-edit-task',
  templateUrl: './edit-task.component.html',
  styleUrls: ['./edit-task.component.css']
})
export class EditTaskComponent implements OnInit
{
  inputTask: ToDoTask = {} as ToDoTask;
  resultTask: ToDoTask = {} as ToDoTask;
  changed: boolean = false;
  editForm: FormGroup;

  constructor (
    private ref: BsModalRef,
    private formBuilder: FormBuilder
  )
  {
    this.editForm = formBuilder.group({});
  }

  ngOnInit(): void
  {
    this.resultTask = JSON.parse(JSON.stringify(this.inputTask));
    this.editForm = this.formBuilder.group({
      title: this.resultTask.title,
      description: this.resultTask.description,
      expiryDate: this.resultTask.expiryDate,
      isCompleted: this.resultTask.isCompleted,
    })
  }

  save()
  {
    if (JSON.stringify(this.resultTask) === JSON.stringify(this.inputTask)) {
      this.changed = false;
    } else {
      this.changed = true;
    }
    this.ref.hide();
  }

  cancel()
  {
    this.changed = false;
    this.resultTask = JSON.parse(JSON.stringify(this.inputTask));
    this.ref.hide();
  }
}
