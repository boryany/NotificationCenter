import { Component, OnInit, OnDestroy } from '@angular/core';
import { NotificationService } from '../../services/notification.service';
import { Subscription } from 'rxjs';
import { Notification } from '../../models/notification.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-notifications',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.css'],
})
export class NotificationsComponent implements OnInit, OnDestroy {
  notifications: Notification[] =[];
  currentPage = 0;
  pageSize = 10000; //Paging is outside current scope
  clientId!: number; 
  private subscription!: Subscription;

  constructor(private notificationService: NotificationService) { }

  ngOnInit(): void {
    const storedClientId = localStorage.getItem('clientId');
    if (storedClientId) {
      this.clientId = parseInt(storedClientId, 10);

      this.notificationService.notifications$.subscribe(newNotifications => {
        this.notifications = newNotifications;
      });

      this.loadNotifications();
    } else {
      console.error('Client ID not found. Redirecting to login...');
      window.location.href = '/login';
    }
  }
  
  loadNotifications(): void {
    this.subscription = this.notificationService
      .getNotifications(this.clientId, this.currentPage * this.pageSize, this.pageSize)
      .subscribe({
        next: (data: Notification[]) => {
          // push to the service subject
          this.notificationService.addNotifications(data);
          this.currentPage++;
        },
        error: (error) => {
          console.error('Error fetching notifications:', error);
        },
        complete: () => {
          console.log('Notification loading completed');
        }
    });
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
