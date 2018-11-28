import { Routes } from "@angular/router";
import { HomeComponent } from "./home/home.component";
import { RegisterComponent } from "./register/register.component";
import { MembersListComponent } from "./members-list/members-list.component";
import { MessagesComponent } from "./messages/messages.component";
import { ListsComponent } from "./lists/lists.component";
import { AuthGuard } from "./_guards/auth.guard";

export const appRoutes: Routes = [
    {
        path: '',
        component: HomeComponent,
    },
    {
        path: 'register',
        component: RegisterComponent
    },
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            {
                path: 'members',
                component: MembersListComponent,
                canActivate: [AuthGuard]

            },
            {
                path: 'messages',
                component: MessagesComponent,
                canActivate: [AuthGuard]
            },
            {
                path: 'lists',
                component: ListsComponent,
                canActivate: [AuthGuard]
            },
        ]
    },
    {
        path: '**',
        redirectTo: '',
        pathMatch: 'full'
    },
]