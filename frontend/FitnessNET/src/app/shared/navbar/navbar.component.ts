import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { AuthService } from '../../_services/auth.service';
import { UserService } from '../../_services/user.service';
import { RouterModule, Router } from '@angular/router';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
  standalone: true,
  imports: [RouterModule, NgIf]
})
export class NavbarComponent implements OnInit {
  @Output() logoutEvent = new EventEmitter<void>();
  currentUser: any = null;

  getInitials(): string {
    if (this.currentUser?.name && this.currentUser?.surname) {
      return (this.currentUser.name[0] + this.currentUser.surname[0]).toUpperCase();
    }
    return 'U';
  }

  constructor(
    private authService: AuthService,
    private userService: UserService,
    private router: Router
  ) {}

  ngOnInit() {
    if (this.authService.isLoggedIn()) {
      this.loadUserData();
    }
  }

  private loadUserData() {
    this.userService.getCurrentUser().subscribe(
      (user: any) => {
        this.currentUser = user;
      },
      (error: any) => {
        console.error('Error loading user data:', error);
      }
    );
  }

  navigateToProfile(event: Event) {
    event.preventDefault();
    this.router.navigate(['/personal']);
  }

  logout() {
    this.authService.logout();
    this.logoutEvent.emit();
  }

  onImageError(event: any) {
    event.target.src = './default-avatar.png';
  }
}

