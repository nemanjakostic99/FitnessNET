import { NgIf } from '@angular/common';
import { Component } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { RegisterForm } from '../_models/registerForm';

@Component({
  selector: 'app-auth-dialog',
  standalone: true,
  templateUrl: './auth-dialog.component.html',
  styleUrls: ['./auth-dialog.component.scss'],
  imports: [NgIf]
})
export class AuthDialogComponent {
  constructor(private authService: AuthService) {}

  isLogin = true;

  toggleView(login: boolean): void {
    this.isLogin = login;
  }

  onLogin(username: HTMLInputElement, password: HTMLInputElement): void {
    this.authService.login(username.value, password.value).subscribe({
      next: (token) => {
        this.authService.saveToken(token);
        alert('Login successful!');
      },
      error: (err) => {
        alert('Login failed: ' + err.message);
      }
    });
  
    // alert(`Logged in with email: ${username.value}`);
    // localStorage.setItem('user', username.value); // Save user to simulate login
    //window.location.reload(); // Reload page after login
  }

  onRegister(
    username: HTMLInputElement,
    email: HTMLInputElement,
    name: HTMLInputElement,
    surname: HTMLInputElement,
    gender: HTMLSelectElement,
    height: HTMLInputElement,
    weight: HTMLInputElement,
    password: HTMLInputElement,
    confirmPassword: HTMLInputElement
  ): void {
    if (password.value !== confirmPassword.value) {
      alert('Passwords do not match!');
      return;
    }

    var registerForm = new RegisterForm(username.value, email.value, name.value, surname.value, gender.value, +height.value, +weight.value, password.value)

    this.authService.register(registerForm).subscribe({
      next: (token) => {
        this.authService.saveToken(token);
        alert('Registration successful!');
      },
      error: (err) => {
        alert('Registration failed: ' + err.message);
      }
    });
    this.isLogin = true; // Redirect to login after registration
  }
}
