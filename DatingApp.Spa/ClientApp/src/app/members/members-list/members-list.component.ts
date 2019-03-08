import { Component, OnInit } from "@angular/core";
import { User } from "../../../app/models/interfaces/user";
import { UserService } from "../../_services/user.service";
import { AlertifyService } from "../../_services/alertify.service";
import { ActivatedRoute } from "@angular/router";
import {
  Pagination,
  PaginatedResult
} from "../../../app/models/interfaces/pagination";

@Component({
  selector: "app-members-list",
  templateUrl: "./members-list.component.html",
  styleUrls: ["./members-list.component.css"]
})
export class MembersListComponent implements OnInit {
  users: User[];
  pagination: Pagination;
  // TODO: relocate this in the userService
  currentUser: User = JSON.parse(localStorage.getItem("user"));
  genderArr = [
    { display: "Males", value: "male" },
    { display: "Females", value: "female" }
  ];
  userParams: any = {};

  constructor(
    private userService: UserService,
    private alertifyService: AlertifyService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.users = data["users"].result;
      this.pagination = data["users"].pagination;
    });
    this.initFilteringParams();
  }

  initFilteringParams() {
    this.userParams.gender =
      this.currentUser.gender === "female" ? "male" : "female";
    this.userParams.minAge = 18;
    this.userParams.maxAge = 99;
    this.userParams.orderBy = "lastActive";
    this.loadUsers();
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
        this.userParams
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
