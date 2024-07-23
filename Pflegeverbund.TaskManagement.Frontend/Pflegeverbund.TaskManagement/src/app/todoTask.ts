export class TodoTask {

    constructor(
      public id : string,
      public description: string,
      public deadline: Date,
      public isDone : boolean,
      public isOverdue : boolean,
    ) {}
  }

export enum TaskStatuses{

  Done,
  Overdue,
  InProgress
}