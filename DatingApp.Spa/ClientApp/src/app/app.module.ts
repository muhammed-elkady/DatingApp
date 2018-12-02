import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule, Routes } from '@angular/router';
import { NavComponent } from '../app/nav/nav.component';
import { AppComponent } from './app.component';
import { AuthService } from '../app/_services/auth.service'
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { ErrorInterceptorProvider } from './_services/interceptors/error.interceptor';
import { AlertifyService } from './_services/alertify.service';
import { BsDropdownModule } from 'ngx-bootstrap';
import { JwtModule, JWT_OPTIONS } from '@auth0/angular-jwt';

import { MessagesComponent } from './messages/messages.component';
import { appRoutes } from './routes';
import { ListsComponent } from './lists/lists.component';
import { AuthGuard } from './_guards/auth.guard';
import { UserService } from './_services/user.service';
import { MembersListComponent } from './members/members-list/members-list.component';
import { MemberCardComponent } from './members/member-card/member-card.component';
import { environment } from '../environments/environment';

export function jwtOptionsFactory() {
   return {
      tokenGetter: () => {
         return localStorage.getItem('token');
      },
      whitelistedDomains: environment.whitelistedDomains,
      // blacklistedRoutes: environment.blacklistedRoutes
   }
}


@NgModule({
   declarations: [
      AppComponent,
      NavComponent,
      HomeComponent,
      RegisterComponent,
      MembersListComponent,
      MessagesComponent,
      ListsComponent,
      MemberCardComponent
   ],
   imports: [
      BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
      HttpClientModule,
      FormsModule,
      ReactiveFormsModule,
      RouterModule.forRoot(appRoutes),
      BsDropdownModule.forRoot(),
      JwtModule.forRoot({
         jwtOptionsProvider: {
            provide: JWT_OPTIONS,
            useFactory: jwtOptionsFactory
         }
      })
   ],
   providers: [
      AuthService,
      ErrorInterceptorProvider,
      AlertifyService,
      AuthGuard,
      UserService
   ],
   bootstrap: [AppComponent]
})
export class AppModule { }
