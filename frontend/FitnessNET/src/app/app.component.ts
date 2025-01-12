import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavbarComponent } from './shared/navbar/navbar.component';
import { SidebarComponent } from './shared/sidebar/sidebar.component';
import { NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthDialogComponent } from './auth-dialog/auth-dialog.component';
import { HttpClientModule } from '@angular/common/http'; // Import HttpClientModule
import { AuthService } from './_services/auth.service'; // Import your AuthService

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet, 
    NavbarComponent, 
    SidebarComponent, 
    NgIf,
    FormsModule,
    AuthDialogComponent, 
    HttpClientModule 
  ],
  providers: [AuthService],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'FitnessNET';
  isLoggedIn = !!localStorage.getItem('fitnessNetjwt'); // Check if user is logged in
}
