import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { UserGroupPopupComponent } from '../user-group-popup/user-group-popup.component';
import { UserGroupService } from '../user-group.service';

interface Group {
  groupId: number;
  groupName: string;
}

interface UserWithGroups {
  userId: number;
  userName: string;
  groups: Group[];
}

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss']
})
export class MainComponent implements OnInit {
  users: UserWithGroups[] = [];
  userName: string = '';
  groupName: string = '';
  displayedColumns: string[] = ['userName', 'groups', 'actions'];

  constructor(private userGroupService: UserGroupService, public dialog: MatDialog) {}

  ngOnInit(): void {
    this.loadUsersWithGroups();
  }

  loadUsersWithGroups(): void {
    this.userGroupService.getUsersWithGroups().subscribe(users => {
      this.users = users;
    });
  }

  addUser(): void {
    if (this.userName) {
      this.userGroupService.addUser(this.userName).subscribe(() => {
        this.userName = '';
        this.loadUsersWithGroups();
      });
    }
  }

  addGroup(): void {
    if (this.groupName) {
      this.userGroupService.addGroup(this.groupName).subscribe(() => {
        this.groupName = '';
        this.loadUsersWithGroups();
      });
    }
  }

  openGroupPopup(userId: number): void {
    const dialogRef = this.dialog.open(UserGroupPopupComponent, {
      width: '300px',
      data: { userId: userId }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.userGroupService.addUserToGroup(userId, result).subscribe(() => {
          this.loadUsersWithGroups();
        });
      }
    });
  }
}