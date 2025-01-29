import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
})
export class LoginComponent {
  username = '';
  password = '';
  errorMessage = '';

  constructor(private authService: AuthService, private router: Router, private http: HttpClient) { }

  ngOnInit(): void {
    // logged in - navigate to notifications paage
    if (this.authService.isLoggedIn()) {
      this.router.navigate(['/notifications']);
    }
  }

  login(): void {
    const credentials = { username: this.username, password: this.password };

    this.http.post<{ token: string; clientId: string }>('/api/auth/login', credentials).subscribe({
      next: (response) => {
        this.authService.login(response.token, response.clientId);
        this.router.navigate(['/notifications']);
      },
      error: () => {
        this.errorMessage = 'Invalid username or password';
      },
    });
  }

  
}
