import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly TOKEN_KEY = 'authToken';
  private readonly CLIENT_ID = 'clientId';

  private loggedInSubject = new BehaviorSubject<boolean>(!!localStorage.getItem(this.TOKEN_KEY));

  // Observable for external subscribers
  loggedIn$ = this.loggedInSubject.asObservable();

  constructor() {
  }

  isLoggedIn(): boolean {
    return this.loggedInSubject.value;  
  }

  login(token: string, clientId: string): void {
    localStorage.setItem(this.TOKEN_KEY, token);
    localStorage.setItem(this.CLIENT_ID, clientId);
    // Notify subscribers
    this.loggedInSubject.next(true); 
  }

  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  logout(): void {
    localStorage.removeItem(this.TOKEN_KEY);
    localStorage.removeItem(this.CLIENT_ID);
    // Notify subscribers
    this.loggedInSubject.next(false);  
  }
}
