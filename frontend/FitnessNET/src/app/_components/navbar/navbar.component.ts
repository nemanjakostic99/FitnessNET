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
  profilePicture: string = 'assets/images/default-avatar.png';

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
    this.loadProfilePicture();
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

  private loadProfilePicture(): void {
    this.userService.getProfilePicture().subscribe({
      next: (blob: Blob) => {
        if (blob.size > 0) {
          const reader = new FileReader();
          reader.onloadend = () => {
            this.profilePicture = reader.result as string;
          };
          reader.readAsDataURL(blob);
        }
      },
      error: () => {
        this.profilePicture = 'assets/images/default-avatar.png';
      }
    });
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
    event.target.src = 'assets/images/default-avatar.png';
  }
}

