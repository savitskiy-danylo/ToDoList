import { List } from "./list";

export interface User
{
  id: string;
  userName: string;
  token: string;
  lists: List[];
}