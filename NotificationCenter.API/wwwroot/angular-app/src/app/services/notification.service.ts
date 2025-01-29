import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { HttpHeaders } from '@angular/common/http';
import { AuthService } from './auth.service';
import { Notification } from '../models/notification.model';

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  private apiUrl = '/api/notifications';
  private apiAuthUrl = '/api/auth';
  private hubConnection: signalR.HubConnection | null = null;
  private notificationsSubject = new BehaviorSubject<Notification[]>([]);
  public notifications$ = this.notificationsSubject.asObservable();

  constructor(private http: HttpClient, private router: Router, private authService: AuthService) {
    this.startSignalRConnection();
  }

  login(username: string, password: string): Observable<any> {
    return this.http.post(`${this.apiAuthUrl}/login`, { username, password });
  }

  getNotifications(clientId: number, skip: number, take: number): Observable<Notification[]> {
    const headers = new HttpHeaders().set('Authorization', `Bearer ${this.authService.getToken()}`);

    return this.http.get<Notification[]>(`${this.apiUrl}/${clientId}/notifications?skip=${skip}&take=${take}`, { headers });
  }

  public addNotifications(notificationsToAdd: Notification[]) {
    const current = this.notificationsSubject.getValue();
    notificationsToAdd.sort((a, b) => b.id - a.id);
    this.notificationsSubject.next([...current, ...notificationsToAdd]);
  }

  private startSignalRConnection() {
    
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('/notificationHub', { //{ withCredentials: true }
        accessTokenFactory: () => this.authService.getToken() || '',
      })
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Debug)
      .build();
    this.hubConnection.keepAliveIntervalInMilliseconds = 10000; // 10 seconds
    this.hubConnection.serverTimeoutInMilliseconds = 60000; // 60 seconds

    this.hubConnection
      .start()
      .then(() => console.log('SignalR connected'))
      .catch((err) => console.error('Error connecting SignalR', err));

    // Listen for notifications
    this.hubConnection.on('ReceiveNotification', (notification: Notification) => {
      const currentNotifications = this.notificationsSubject.getValue();

      if (notification.createdAt && notification.createdAt.endsWith('Z')) {
        notification.createdAt = notification.createdAt.slice(0, -1);
      }

      this.notificationsSubject.next([notification, ...currentNotifications]);
    });

    this.hubConnection.onclose(async () => {
      setTimeout(() => this.startSignalRConnection(), 2000); // Retry connection
    });
   
  }

  public clearNotifications(): void {
    this.notificationsSubject.next([]);
  }

  public stopConnection(): void {
    if (this.hubConnection) {
      this.hubConnection.stop().then(() => console.log('SignalR Disconnected'));
    }
  }
}





 



