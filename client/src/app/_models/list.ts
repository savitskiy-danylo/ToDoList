import { ToDoTask } from "./task";

export interface List
{
  id: string;
  title: string;
  description: string;
  tasks: ToDoTask[];
  userId: string;
}