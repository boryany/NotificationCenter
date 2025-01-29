import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'app-header',
  imports: [
    CommonModule
  ],
  standalone: true,        
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent {

  constructor(public authService: AuthService, private notificationService: NotificationService, private router: Router) {
  }

  logout(): void {
    this.authService.logout();
    this.notificationService.clearNotifications();
    this.router.navigate(['/login']);
  }
}
