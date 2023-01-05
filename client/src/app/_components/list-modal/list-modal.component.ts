import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { List } from 'src/app/_models/list';

@Component({
  selector: 'app-list-modal',
  templateUrl: './list-modal.component.html',
  styleUrls: ['./list-modal.component.css']
})
export class ListModalComponent implements OnInit
{
  inputList: List = {} as List;
  resultList: List = {} as List;
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
    this.resultList = JSON.parse(JSON.stringify(this.inputList));
    this.editForm = this.formBuilder.group({
      title: this.resultList.title,
      description: this.resultList.description,
    })
  }

  save()
  {
    this.ref.hide();
  }

  cancel()
  {
    this.resultList = JSON.parse(JSON.stringify(this.inputList));
    this.ref.hide();
  }

}
