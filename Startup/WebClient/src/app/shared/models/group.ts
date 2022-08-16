export interface IGroup {
  name: string;
  connections: Connection[];
}

interface Connection {
  id: string;
  username: string;
}
