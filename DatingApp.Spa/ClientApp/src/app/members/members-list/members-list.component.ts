import { Component, OnInit } from "@angular/core";
import { User } from "../../../app/models/interfaces/user";
import { UserService } from "../../_services/user.service";
import { AlertifyService } from "../../_services/alertify.service";
import { ActivatedRoute } from "@angular/router";

@Component({
  selector: "app-members-list",
  templateUrl: "./members-list.component.html",
  styleUrls: ["./members-list.component.css"]
})
export class MembersListComponent implements OnInit {
  users: Array<User>;
  constructor(
    private userService: UserService,
    private alertifyService: AlertifyService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.users = data["users"].result;
    });
  }
}
