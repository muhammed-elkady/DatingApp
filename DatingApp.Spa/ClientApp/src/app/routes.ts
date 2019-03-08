import { ListsResolver } from './_resolvers/lists.resolver';
import { Routes } from "@angular/router";
import { HomeComponent } from "./home/home.component";
import { RegisterComponent } from "./register/register.component";
import { MessagesComponent } from "./messages/messages.component";
import { ListsComponent } from "./lists/lists.component";
import { AuthGuard } from "./_guards/auth.guard";
import { MembersListComponent } from "./members/members-list/members-list.component";
import { MemberDetailComponent } from "./members/member-detail/member-detail.component";
import { MemberDetailResolver } from "./_resolvers/member-detail.resolver";
import { MemberListResolver } from './_resolvers/member-list.resolver';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberEditResolver } from './_resolvers/member-edit.resolver';
import { PreventUnsavedChangesGuard } from "./_guards/prevent-unsaved-changes.guard";

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
                resolve: { users: MemberListResolver }
            },
            {
                path: 'members/edit',
                component: MemberEditComponent,
                resolve: { user: MemberEditResolver },
                canDeactivate: [PreventUnsavedChangesGuard]
            },
            {
                path: 'members/:id',
                component: MemberDetailComponent,
                resolve: { user: MemberDetailResolver }
            },

            {
                path: 'messages',
                component: MessagesComponent,
            },
            {
                path: 'lists',
                component: ListsComponent,
                resolve: {users: ListsResolver}
            },
        ]
    },
    {
        path: '**',
        redirectTo: '',
        pathMatch: 'full'
    },
]