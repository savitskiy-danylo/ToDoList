export interface ToDoTask
{
  id: string;
  title: string;
  description: string;
  isCompleted: boolean;
  createdAt: Date;
  expiryDate: Date;
  index: number;
  listId: string;
}