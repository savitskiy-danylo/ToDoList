import { AfterViewInit, Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ToDoTask } from 'src/app/_models/task';

@Component({
  selector: 'app-edit-task',
  templateUrl: './edit-task.component.html',
  styleUrls: ['./edit-task.component.css']
})
export class EditTaskComponent implements OnInit
{
  inputTask: ToDoTask = {} as ToDoTask;
  resultTask: ToDoTask = {} as ToDoTask;
  editForm: FormGroup;
  date: string = new Date().toDateString();
  title: string = "";

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
    this.resultTask.expiryDate = new Date(this.resultTask.expiryDate);
    this.editForm = this.formBuilder.group({
      title: this.resultTask.title,
      description: this.resultTask.description,
      expiryDate: this.resultTask.expiryDate,
      isCompleted: this.resultTask.isCompleted,
    })
  }

  save()
  {
    this.ref.hide();
  }

  cancel()
  {
    this.resultTask = JSON.parse(JSON.stringify(this.inputTask));
    this.ref.hide();
  }
}
