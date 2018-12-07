import { Component, OnInit } from '@angular/core';
import { UserService } from '../../_services/user.service';

import { User } from '../../models/interfaces/user';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from './../../_services/alertify.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {

  user: User;
  constructor(private userService: UserService,
    private alertifyService: AlertifyService,
    private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(
      data => {
        this.user = data['user']
      }
    );
  }

}
