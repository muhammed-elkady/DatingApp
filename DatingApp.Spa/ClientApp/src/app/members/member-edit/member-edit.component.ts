import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { User } from './../../models/interfaces/user';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from './../../_services/alertify.service';
import { NgForm } from '@angular/forms';
import { UserService } from './../../_services/user.service';
import { AuthService } from './../../_services/auth.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {

  // Prevent the user of reloading the page before saving changes
  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event: any) {
    if (this.editForm.dirty) {
      $event.returnValue = true;
    }
  }

  @ViewChild('editForm') editForm: NgForm;

  user: User;
  constructor(private route: ActivatedRoute,
    private alertifyService: AlertifyService,
    private userService: UserService,
    private authService: AuthService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.user = data['user'];
    })
  }

  updateUser() {
    this.userService.updateUser(this.authService.decodedToken.nameid, this.user)
      .subscribe(
        () => {
          this.alertifyService.success('data updated successfully');
          this.editForm.reset(this.user);
        },
        err => this.alertifyService.error(err)
      )
  }

  updateMainPhoto(newUrl: string) {
    this.user.photoUrl = newUrl;
  }

}
