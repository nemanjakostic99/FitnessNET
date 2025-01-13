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
  constructor(private authService: AuthService) {
    this.isLoggedIn = this.authService.isLoggedIn();
  }
  
  title = 'FitnessNET';
  isLoggedIn: boolean;
  
  handleLoginChange(): void {
    this.isLoggedIn = this.authService.isLoggedIn(); // Update isLoggedIn based on child component
  }
}
