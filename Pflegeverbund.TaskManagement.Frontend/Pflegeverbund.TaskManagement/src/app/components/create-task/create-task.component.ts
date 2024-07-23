import { Component, EventEmitter, Inject, OnInit, Output } from "@angular/core";
import {v4 as uuidv4} from 'uuid';
import { FormBuilder, FormGroup } from "@angular/forms";

import {MAT_DIALOG_DATA, MatDialogContent, MatDialogRef, MatDialogTitle} from "@angular/material/dialog";
import { TaskHttpService } from "../../services/books-http-service";
import { TodoTask } from "../../todoTask";

@Component({
    selector: 'create-task',
    templateUrl: './create-task.component.html',
    styleUrls: ['./create-task.component.scss'],
})
export class TaskDialogComponent implements OnInit {

    form: FormGroup | any;
    newTaskDescription:string | null;
    newTaskDeadline : Date | null;
    errorMessage : string | null;
    @Output() taskAdded = new EventEmitter<TodoTask>();

    constructor(private taskHttpService : TaskHttpService,        
        private fb: FormBuilder,
        private dialogRef: MatDialogRef<TaskDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data : any) {

        this.newTaskDescription = data.description;
        this.newTaskDeadline = data.taskDeadline;
        this.errorMessage = "";
    }

    ngOnInit() {

        this.form = this.fb.group({
            description: [this.newTaskDescription, []],
            deadline: [this.newTaskDeadline, []],
        });
    }

    save() {
      if(this.form)
      {        
        this.dialogRef.close(this.form.value);
      }
    }

    close() {
        this.dialogRef.close();
    }

    onSubmit(): void 
    {
        if (this.newTaskDescription != null && this.newTaskDescription.length <= 10) 
        {
          this.errorMessage = 'Task description must be longer than 10 characters.';
          return;
        }

        if (!this.newTaskDeadline) 
        {
              this.errorMessage = 'Deadline must be defined.';
              return;
        }

        if ( this.newTaskDescription != null && this.newTaskDeadline)
        {    
            const newTask: TodoTask = 
            {
                id: uuidv4(),
                description: this.newTaskDescription,
                deadline: this.newTaskDeadline as Date,
                isDone: false,
                isOverdue: this.newTaskDeadline < new Date() ? true : false,
            };
        
            this.taskHttpService.createTask(newTask).subscribe((task: TodoTask) => 
            {
                this.taskAdded.emit(task);
                this.dialogRef.close(task);
            },
            (error) => 
            {
                var message = error.error;
                console.error('Error adding task', error);
                this.errorMessage = `Failed to add task. ${message}`;
            });
        }
      }
}