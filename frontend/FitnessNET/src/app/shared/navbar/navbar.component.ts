import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { AuthService } from '../../_services/auth.service';


@Component({
  selector: 'app-navbar',
  imports: [FormsModule],
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent {
  @Output() logoutEvent = new EventEmitter<void>();
  
  isLoggedIn = false;
  username = '';

  constructor(public dialog: MatDialog, private authService: AuthService) {}

  onLogout() {
    this.authService.logout();
    this.logoutEvent.emit();
  }
}

