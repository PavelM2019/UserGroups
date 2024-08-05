import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

interface Group {
  groupId: number;
  groupName: string;
}

interface UserWithGroups {
  userId: number;
  userName: string;
  groups: Group[];
}

@Injectable({
  providedIn: 'root'
})
export class UserGroupService {
  private apiUrl = 'https://localhost:7198/api/User'; 

  constructor(private http: HttpClient) {}

  getUsersWithGroups(): Observable<UserWithGroups[]> {
    return this.http.get<UserWithGroups[]>(`${this.apiUrl}/users-with-groups`);
  }

  addUser(userName: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/add-user`, { userName });
  }

  addGroup(groupName: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/add-group`, { groupName });
  }

  getAvailableGroups(userId: number): Observable<Group[]> {
    return this.http.get<Group[]>(`${this.apiUrl}/available-groups/${userId}`);
  }

  addUserToGroup(userId: number, groupId: number): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/add-user-to-group`, { userId, groupId });
  }
}