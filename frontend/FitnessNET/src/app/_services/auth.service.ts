import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, interval } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { Router } from '@angular/router';
import { environment } from '../../environments/environment';
import { RegisterFormDTO } from '../_models/registerFormDTO';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = environment.apiUrl;
  private tokenCheckInterval: any;

  constructor(
    private http: HttpClient,
    private router: Router
  ) {
    // Start checking token expiration every minute
    this.startTokenExpirationCheck();
  }

  private startTokenExpirationCheck() {
    this.tokenCheckInterval = interval(60000).subscribe(() => {
      if (this.isTokenExpired()) {
        this.logout();
      }
    });
  }

  private isTokenExpired(): boolean {
    const token = this.getToken();
    if (!token) return true;

    try {
      const tokenData = JSON.parse(atob(token.split('.')[1]));
      const expirationDate = new Date(tokenData.exp * 1000);
      return expirationDate < new Date();
    } catch {
      return true;
    }
  }

  // Login method
  login(username: string, password: string): Observable<string> {
    const url = `${this.apiUrl}/auth/login`;
    const body = { 
      "Username": username, 
      "Password": password 
    };

    return this.http.post<{ token: string }>(url, body).pipe(
      map(response => response.token) // Extract the JWT token
    );
  }

  // Register method
  register(form: RegisterFormDTO)
  : Observable<string> {
    const url = `${this.apiUrl}/auth/register`;
    const body = { 
      "Username": form.username, 
      "Email":form.email,
      "Name": form.name,
      "Surname": form.surname,
      "Gender": form.gender,
      "Height": form.height,
      "Weight": form.weight,
      "IsTrainer": form.isTrainer,
      "Password": form.password 
    };

    return this.http.post<{ token: string }>(url, body).pipe(
      map(response => response.token) // Extract the JWT token
    );
  }

  // Store the JWT token in localStorage
  saveToken(token: string): void {
    localStorage.setItem('fitnessNetjwt', token);
  }

  // Retrieve the JWT token from localStorage
  getToken(): string | null {
    return localStorage.getItem('fitnessNetjwt');
  }

  // Clear the token on logout
  logout(): void {
    localStorage.removeItem('fitnessNetjwt');
    if (this.tokenCheckInterval) {
      this.tokenCheckInterval.unsubscribe();
    }
  }

  isLoggedIn(): boolean {
    return this.getToken() !== null && !this.isTokenExpired();
  }  
}
