import { TodoTask } from "../todoTask";

export interface PaginatedResponse {
    totalItems: number;
    pageNumber: number;
    pageSize: number;
    items: TodoTask[];
}
  