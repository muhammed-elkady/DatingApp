import { UserService } from './../../_services/user.service';
import { AuthService } from './../../_services/auth.service';
import { AlertifyService } from './../../_services/alertify.service';
import { Component, OnInit, Input } from '@angular/core';
import { User } from '../../../app/models/interfaces/user';


@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {

  defaultPhotoUrl = '../../../assets/default-user.png';
  @Input() user: User;

  constructor(private authService: AuthService,
    private userService: UserService,
    private alertifyService: AlertifyService) { }

  ngOnInit() {
  }

  sendLike(recipientId: string) {
    this.userService.sendLike(this.authService.decodedToken.nameid, recipientId)
      .subscribe(
        (data) => { this.alertifyService.success(`You've liked: ${this.user.knownAs}`) },
        (err) => this.alertifyService.error(err)
      )
  }
}
