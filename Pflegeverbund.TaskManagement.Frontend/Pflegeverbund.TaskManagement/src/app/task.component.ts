import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';  
import { TaskHttpService } from './services/books-http-service';
import { Subscription } from 'rxjs';
import { TaskStatuses, TodoTask } from './todoTask';
import { SelectionModel } from '@angular/cdk/collections';
import {v4 as uuidv4} from 'uuid';
import { MatTableDataSource } from '@angular/material/table';
import { MatSelectionList } from '@angular/material/list';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { TaskDialogComponent } from './components/create-task/create-task.component';
import { MatPaginator } from '@angular/material/paginator';
import { PaginatedResponse } from './services/pagination';

  
@Component({
  selector: 'app-root',
  templateUrl: './task.component.html',
  styleUrls: ['./task.component.scss'],  
})
export class TaskComponent implements OnDestroy, OnInit {
  
  dataSource: MatTableDataSource<TodoTask>;
  isLoading: boolean = false;  
  taskSelected : boolean = false;

  tasks: TodoTask[] = [];
  taskStatuses : string[] = [];
  newTaskDescription: string = '';
  newTaskDeadline: Date | null = null;
  errorMessage: string | null = null;
  isAnyTaskSelected : boolean | false = false;

  totalItems = 0;
  pageSize = 10; //default size
 
  @ViewChild(MatPaginator) paginator!: MatPaginator;


  private static subscriptions: Subscription[] = [];

  selection = new SelectionModel<TodoTask>(true, []);

  displayedColumns: string[] = ['select','description', 'deadline', 'status'];

  selectedTask: TodoTask | null = null;
  
  constructor(private taskHttpService : TaskHttpService, private dialog : MatDialog){
    this.dataSource = new MatTableDataSource<TodoTask>();

  }

  toggleAllSelection() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.dataSource.data.forEach(row => {
        if (!row.isDone) {
          this.selection.select(row);
        }
      });
  }

  toggleSelection(task: TodoTask) {
    this.selection.toggle(task);

    this.isAnyTaskSelected = this.selection.selected.length == 0 ? true : false;
  }

  loadPage(pageIndex: number, pageSize: number): void {
    const pageNumber = pageIndex + 1;
    this.taskHttpService.getPagedRecords(pageNumber, pageSize)
                        .subscribe(response => {
                                                  this.totalItems = response.totalItems;
                                                  this.dataSource.data = response.items;
                                                });
  }

  isAllSelected() {
    
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }
  
  ngOnInit() 
  {
    this.dataSource.paginator = this.paginator;
    this.taskStatuses.push(...Object.getOwnPropertyNames(TaskStatuses).filter(prop => isNaN(parseInt(prop))));
    this.init();
  }

  public getAllDoneTasks(){

    this.taskHttpService.getAllCompletedTasks().subscribe(
      (payloadFilteredTasks) => 
      { 
        setTimeout( ()=> {
          debugger;                    
              this.dataSource = new MatTableDataSource<TodoTask>(payloadFilteredTasks);
              this.tasks.push(...payloadFilteredTasks);
              this.isLoading = false; 
        }, 500);

        this.tasks = [];
        this.dataSource.data = [];        
        this.selection.clear(); // Clear selection after deletion
      }, 
      error => console.error('Error marking tasks as completed', error))
  }
  
  public getAllOverdueTasks(){

    this.taskHttpService.getAllOverdueTasks().subscribe(
      (payloadFilteredTasks) => 
      { 
        setTimeout( ()=> {
          debugger;                    
              this.dataSource = new MatTableDataSource<TodoTask>(payloadFilteredTasks);
              this.tasks.push(...payloadFilteredTasks);
              this.isLoading = false; 
        }, 500);

        this.tasks = [];
        this.dataSource.data = [];        
        this.selection.clear(); // Clear selection after deletion
      }, 
      error => console.error('Error marking tasks as completed', error))
  }

  public getAllInProgressTasks(){

    this.taskHttpService.getAllInProgressTasks().subscribe(
      (payloadFilteredTasks) => 
      { 
        setTimeout( ()=> {
          debugger;                    
              this.dataSource = new MatTableDataSource<TodoTask>(payloadFilteredTasks);
              this.tasks.push(...payloadFilteredTasks);
              this.isLoading = false; 
        }, 500);

          this.tasks = [];
          this.dataSource.data = [];        
          this.selection.clear(); // Clear selection after deletion
      }, 
      error => console.error('Error marking tasks as completed', error))
  }

  onFilterTask(item : string)
  {
    if ( item == "Done" )
       this.getAllDoneTasks();
    else if(item == "Overdue")
      this.getAllOverdueTasks();
    else
      this.getAllInProgressTasks();
  }

  ngAfterViewInit() 
  {    
    if(this.selection)
    {   
      this.selection.changed.subscribe(() => 
      {      
        console.log('Selection changed:', this.selection.selected);
      });
    }
  }

  appendTaskInTaskGrid(newTask : TodoTask) {
        
    this.tasks.push(newTask);
    this.dataSource.data = this.tasks;
    this.resetForm();    
  }

  deleteSelectedTasks() {
    
    const selectedTasks = this.selection.selected; // Get all selected tasks            
    const selectedIds = selectedTasks.map(a => a.id);

    if (selectedTasks.length === 0) 
      {
        alert('No tasks selected.');
        return;
      }

    this.taskHttpService.deleteTasks(selectedIds).subscribe(
      () => 
      {         
        selectedTasks.forEach(selectedTask => 
        {
          var taskIndex = this.tasks.findIndex(t => t.id == selectedTask.id);
          this.tasks.splice(taskIndex, 1);
        });

        this.dataSource.data = this.tasks; // Refresh the table data
        this.selection.clear(); // Clear selection after deletion
      },
      error => console.error('Error deleting tasks', error)
    );
  }

  markSelectedAsComplete() {

    const selectedTasks = this.selection.selected; // Get all selected tasks

    if (selectedTasks.length === 0) 
    {
      alert('No tasks selected.');
      return;
    }

    const selectedIds = selectedTasks.map(a => a.id);

    this.taskHttpService.markTasksAsCompleted(selectedIds).subscribe(
      () => 
        { 
          selectedTasks.forEach(selectedTask => 
          {
            selectedTask.isDone = true;
          });
  
          this.dataSource.data = this.tasks; // Refresh the table data
          this.selection.clear(); // Clear selection after deletion
        },
  
      error => console.error('Error marking tasks as completed', error)
    );
  }

  private resetForm() {

    this.newTaskDescription = '';
    this.newTaskDeadline = null;
    this.errorMessage = null;    
  }

  selectTask(todoTask: TodoTask): void {
    
    this.selectedTask = todoTask;
  }

  isSelected(todoTask: TodoTask): boolean 
  {
    return this.selectedTask === todoTask;
  }
 
  public init()
  {
    this.isLoading = true;


    var taskHttpSubscription = this.taskHttpService.getAllTasks()
                                   .subscribe(payload =>{
                                    setTimeout( ()=> {
                                      debugger;
                                      
                                      this.dataSource = new MatTableDataSource<TodoTask>(payload);
                                      this.tasks.push(...payload);
                                      this.isLoading = false; 
                                    }, 500);
                                 },
                                 (error) => {
                                  debugger;
                                      this.isLoading = false;

                                      this.dataSource = new MatTableDataSource<TodoTask>();
                                 });

                                 TaskComponent.subscriptions.push(taskHttpSubscription);                                          
  }

  ngOnDestroy(): void {

    TaskComponent.subscriptions.forEach(subscription => subscription ? subscription.unsubscribe() : 0);
    TaskComponent.subscriptions = [];

  }

  createTask() : void {

    const dialogConfig = new MatDialogConfig();

    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;

    dialogConfig.data = { };
  
    const dialogRef = this.dialog.open(TaskDialogComponent, dialogConfig);

    dialogRef.componentInstance.taskAdded.subscribe((newTask: TodoTask) => {
      this.appendTaskInTaskGrid(newTask);
    });
  }
}
