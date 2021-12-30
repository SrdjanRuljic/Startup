export interface IUserWithRoles {
  id: string;
  firstName: string;
  lastName: string;
  userName: string;
  email: string;
  roles: string[];
}

export interface IUser {
  id: string;
  firstName: string;
  lastName: string;
  userName: string;
  email: string;
}
