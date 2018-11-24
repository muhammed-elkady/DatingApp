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

const appRoutes: Routes = [
   {
      path: '',
      component: HomeComponent,
      pathMatch: 'full'
   },
   {
      path: 'register',
      component: RegisterComponent
   }
]

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
      RouterModule.forRoot(appRoutes)
   ],
   providers: [
      AuthService,
      ErrorInterceptorProvider
   ],
   bootstrap: [AppComponent]
})
export class AppModule { }
