import { HttpClient, HttpHeaders } from  '@angular/common/http';
import {v4 as uuidv4} from 'uuid';
import { Injectable } from  '@angular/core';
import { Observable, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { TodoTask } from '../todoTask';
import { PaginatedResponse } from '../../app/services/pagination';

export class HttpCallResult {
    data: TodoTask[] = new Array();
    exception : string = "";
    success : boolean = false   
}

@Injectable({
providedIn:  'root'
})

export class TaskHttpService {

  private inProgressTasksUrl = environment.tasksApi.concat('in-progress');
  private overDueTasksUrl = environment.tasksApi.concat('overdue');
  private completedTasksUrl = environment.tasksApi.concat('completed');
  private markTasksAsCompleteTasksUrl = environment.tasksApi.concat('complete');

  private pagedRecordsTasksUrl = environment.tasksApi.concat('page');

  constructor(private http: HttpClient) {

  }  

  public createTask(todoTask : TodoTask) : Observable<TodoTask> {
        
    let headers = new HttpHeaders({ 'Content-Type': 'application/json' });        
    headers.append("Access-Control-Allow-Origin", "*");

    headers.append('Access-Control-Allow-Methods', 'GET, POST, OPTIONS, PUT, PATCH, DELETE');

    headers.append('Access-Control-Allow-Headers', 'X-Requested-With,content-type');

    return this.http.post<TodoTask>(environment.tasksApi, todoTask, {headers : headers});
  }

  public deleteTasks(ids: String[]) : Observable<void> {
        
    let headers = new HttpHeaders({ 'Content-Type': 'application/json' });        
    headers.append("Access-Control-Allow-Origin", "*");

    headers.append('Access-Control-Allow-Methods', 'GET, POST, OPTIONS, PUT, PATCH, DELETE');

    headers.append('Access-Control-Allow-Headers', 'X-Requested-With,content-type');

    return this.http.delete<void>(`${environment.tasksApi}`, { body : ids });
  }

  public markTasksAsCompleted(ids: String[]): Observable<void> {

    let headers = new HttpHeaders({ 'Content-Type': 'application/json' });        
    headers.append("Access-Control-Allow-Origin", "*");

    headers.append('Access-Control-Allow-Methods', 'GET, POST, OPTIONS, PUT, PATCH, DELETE');

    headers.append('Access-Control-Allow-Headers', 'X-Requested-With,content-type');

    return this.http.put<void>(`${this.markTasksAsCompleteTasksUrl}`, ids, { headers : headers });
  }

  public getAllTasks() : Observable<TodoTask[]> {
        
    let headers = new HttpHeaders({ 'Content-Type': 'application/json' });        
    headers.append("Access-Control-Allow-Origin", "*");

    headers.append('Access-Control-Allow-Methods', 'GET, POST, OPTIONS, PUT, PATCH, DELETE');

    headers.append('Access-Control-Allow-Headers', 'X-Requested-With,content-type');

    return this.http.get<TodoTask[]>(environment.tasksApi, {headers : headers});
 }

  public getAllCompletedTasks(): Observable<TodoTask[]> {

    let headers = new HttpHeaders({ 'Content-Type': 'application/json' });        
    headers.append("Access-Control-Allow-Origin", "*");

    headers.append('Access-Control-Allow-Methods', 'GET, POST, OPTIONS, PUT, PATCH, DELETE');

    headers.append('Access-Control-Allow-Headers', 'X-Requested-With,content-type');

    return this.http.get<TodoTask[]>(`${this.completedTasksUrl}`, { headers : headers });
  }

  public getAllOverdueTasks(): Observable<TodoTask[]> {

    let headers = new HttpHeaders({ 'Content-Type': 'application/json' });        
    headers.append("Access-Control-Allow-Origin", "*");

    headers.append('Access-Control-Allow-Methods', 'GET, POST, OPTIONS, PUT, PATCH, DELETE');

    headers.append('Access-Control-Allow-Headers', 'X-Requested-With,content-type');

    return this.http.get<TodoTask[]>(`${this.overDueTasksUrl}`, { headers : headers });
  }

  public getAllInProgressTasks(): Observable<TodoTask[]> {

    let headers = new HttpHeaders({ 'Content-Type': 'application/json' });        
    headers.append("Access-Control-Allow-Origin", "*");

    headers.append('Access-Control-Allow-Methods', 'GET, POST, OPTIONS, PUT, PATCH, DELETE');

    headers.append('Access-Control-Allow-Headers', 'X-Requested-With,content-type');

    return this.http.get<TodoTask[]>(`${this.inProgressTasksUrl}`, { headers : headers });
  }

  public getPagedRecords(pageNumber: Number, pageSize : Number): Observable<PaginatedResponse> {
  
    let headers = new HttpHeaders({ 'Content-Type': 'application/json' });        
    headers.append("Access-Control-Allow-Origin", "*");

    headers.append('Access-Control-Allow-Methods', 'GET, POST, OPTIONS, PUT, PATCH, DELETE');

    headers.append('Access-Control-Allow-Headers', 'X-Requested-With,content-type');

    return this.http.get<PaginatedResponse>(`${this.pagedRecordsTasksUrl}?pageNumber=${pageNumber}&pageSize=${pageSize}`, { headers : headers });
  }
}
