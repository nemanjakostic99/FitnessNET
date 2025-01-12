import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { RegisterForm } from '../_models/registerForm';


@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:5001/'; // Replace with your API endpoint

  constructor(private http: HttpClient) {}

  // Login method
  login(username: string, password: string): Observable<string> {
    const url = `${this.apiUrl}api/auth/login`;
    const body = { 
      "Username": username, 
      "Password": password 
    };

    return this.http.post<{ token: string }>(url, body).pipe(
      map(response => response.token) // Extract the JWT token
    );
  }

  // Register method
  register(form: RegisterForm)
  : Observable<string> {
    const url = `${this.apiUrl}api/auth/register`;

    return this.http.post<{ token: string }>(url, form).pipe(
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
  }
}
