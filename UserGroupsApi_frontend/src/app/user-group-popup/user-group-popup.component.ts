import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { UserGroupService } from '../user-group.service';

interface Group {
  groupId: number;
  groupName: string;
}

@Component({
  selector: 'app-user-group-popup',
  templateUrl: './user-group-popup.component.html',
  styleUrls: ['./user-group-popup.component.scss']
})
export class UserGroupPopupComponent implements OnInit {
  availableGroups: Group[] = [];
  selectedGroupId: number | null = null;

  constructor(
    private userGroupService: UserGroupService,
    public dialogRef: MatDialogRef<UserGroupPopupComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { userId: number }
  ) {}

  ngOnInit(): void {
    this.userGroupService.getAvailableGroups(this.data.userId).subscribe((groups: Group[]) => {
      this.availableGroups = groups;
    });
  }

  onSave(): void {
    if (this.selectedGroupId) {
      this.dialogRef.close(this.selectedGroupId);
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}