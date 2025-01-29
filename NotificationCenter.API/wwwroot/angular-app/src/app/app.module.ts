import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { NotificationsComponent } from './components/notifications/notifications.component';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HeaderComponent } from './components/header/header.component'; // Import HeaderComponent

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    NotificationsComponent,
    HeaderComponent
  ],
  imports: [
    BrowserModule,
    CommonModule,
    FormsModule,
    AppRoutingModule
  ],
  exports: [HeaderComponent],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule { }



