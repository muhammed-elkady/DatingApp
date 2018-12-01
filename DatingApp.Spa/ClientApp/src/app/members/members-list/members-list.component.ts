import { Component, OnInit } from '@angular/core';
import { User } from '../../../app/models/interfaces/user';
import { UserService } from '../../_services/user.service';
import { AlertifyService } from '../../_services/alertify.service';


@Component({
  selector: 'app-members-list',
  templateUrl: './members-list.component.html',
  styleUrls: ['./members-list.component.css']
})
export class MembersListComponent implements OnInit {
  users: Array<User>;
  constructor(
    private userService: UserService,
    private alertifyService: AlertifyService
  ) { }

  ngOnInit() {
    this.loadUsers();
  }


  loadUsers() {
    this.userService.getUsers().subscribe(
      (res: User[]) => {
        debugger;
        this.users = res
      },
      (err) => {
        this.alertifyService.error(err)
        this.alertifyService.error('Error happened while loading users')
      }
    )
  }

}
