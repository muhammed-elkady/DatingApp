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

  constructor() { }

  ngOnInit() {
  }

}
