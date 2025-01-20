import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavbarComponent } from './_components/navbar/navbar.component';
import { SidebarComponent } from './_components/sidebar/sidebar.component';
import { FooterComponent } from './_components/footer/footer.component';
import { NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthDialogComponent } from './_components/auth-dialog/auth-dialog.component';
import { HttpClientModule } from '@angular/common/http';
import { AuthService } from './_services/auth.service';
import { ChatComponent } from './_components/chat_components/chat/chat.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet, 
    NavbarComponent, 
    SidebarComponent, 
    FooterComponent,
    NgIf,
    FormsModule,
    AuthDialogComponent, 
    HttpClientModule,
    ChatComponent 
  ],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  constructor(private authService: AuthService) {
    this.isLoggedIn = this.authService.isLoggedIn();
  }
  
  title = 'FitnessNET';
  isLoggedIn: boolean;
  
  handleLoginChange(): void {
    this.isLoggedIn = this.authService.isLoggedIn();
  }
}
