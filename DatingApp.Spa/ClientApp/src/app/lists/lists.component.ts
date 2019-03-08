
import { ActivatedRoute } from '@angular/router';
import { UserService } from './../_services/user.service';
import { Pagination, PaginatedResult } from './../models/interfaces/pagination';
import { Component, OnInit } from '@angular/core';
import { User } from '../models/interfaces/user';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {
  users: User[];
  pagination: Pagination;
  likesParam: string

  constructor(private authService: AuthService,
    private userService: UserService,
    private route: ActivatedRoute,
    private alertifyService: AlertifyService) { }

  ngOnInit() {
    this.route.data.subscribe(
      data => {
        this.users = data['users'].result;
        this.pagination = data['users'].pagination;
      }
    );
    this.likesParam = 'Likers';
  }


  pageChanged(args: { itemsPerPage: number; page: number }): void {
    this.pagination.currentPage = args.page;
    this.loadUsers();
  }

  loadUsers() {
    this.userService
      .getUsers(
        this.pagination.currentPage,
        this.pagination.itemsPerPage,
        null,
        this.likesParam
      )
      .subscribe(
        (res: PaginatedResult<User[]>) => {
          this.users = res.result;
          this.pagination = res.pagination;
        },
        err => this.alertifyService.error(err)
      );
  }

}
