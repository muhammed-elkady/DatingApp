import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { NavComponent } from '../app/nav/nav.component';
import { AppComponent } from './app.component';
import { AuthService } from '../app/_services/auth.service'
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';

@NgModule({
   declarations: [
      AppComponent,
      NavComponent,
      HomeComponent,
      RegisterComponent
   ],
   imports: [
      BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
      HttpClientModule,
      FormsModule,
      ReactiveFormsModule,
      RouterModule.forRoot([

      ])
   ],
   providers: [AuthService],
   bootstrap: [AppComponent]
})
export class AppModule { }
