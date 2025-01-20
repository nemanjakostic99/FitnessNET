import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CommunityUser } from '../../_models/communityUser.interface';
import { UserService } from '../../_services/user.service';
import { Subject, debounceTime, distinctUntilChanged, takeUntil } from 'rxjs';
import { FriendshipService } from '../../_services/friendship.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-community',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './community.component.html',
  styleUrls: ['./community.component.scss']
})
export class CommunityComponent implements OnInit, OnDestroy {
  activeFilter: boolean | null = null;
  users: CommunityUser[] = [];
  searchTerm = '';
  currentPage = 1;
  pageSize = 8;
  totalPages = 0;
  isLoading = false;
  private searchSubject = new Subject<string>();
  private destroy$ = new Subject<void>();

  constructor(
    private userService: UserService,
    private friendshipService: FriendshipService,
    private snackBar: MatSnackBar
  ) {
    this.searchSubject.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      takeUntil(this.destroy$)
    ).subscribe(() => {
      this.currentPage = 1;
      this.loadUsers();
    });
  }

  ngOnInit(): void {
    this.loadUsers();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  onSearch(event: Event): void {
    const target = event.target as HTMLInputElement;
    this.searchTerm = target.value;
    this.searchSubject.next(this.searchTerm);
  }

  onFilterChange(filter: string): void {
    this.activeFilter = filter === 'all' ? null : filter === 'trainers';
    this.currentPage = 1;
    this.loadUsers();
  }

  loadMore(): void {
    this.currentPage++;
    this.loadUsers(true);
  }

  private loadUsers(append = false): void {
    if (this.isLoading) return;
    this.isLoading = true;

    this.userService.searchUsers({
      page: this.currentPage,
      pageSize: this.pageSize,
      searchTerm: this.searchTerm,
      isTrainer: this.activeFilter
    }).subscribe({
      next: (response) => {
        if (append) {
          this.users = [...this.users, ...response.items];
        } else {
          this.users = response.items;
        }
        this.totalPages = response.totalPages;
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
      }
    });
  }

  getProfilePictureUrl(profilePicture: any): string {
    if (!profilePicture || !profilePicture.pictureData) {
      return 'assets/images/default-avatar.png';
    }
    return `data:${profilePicture.contentType};base64,${profilePicture.pictureData}`;
  }

  handleImageError(event: Event): void {
    const img = event.target as HTMLImageElement;
    img.src = 'assets/images/default-avatar.png';
  }

  sendFriendRequest(senderUsername: string) {
    this.friendshipService.sendFriendRequest(senderUsername).subscribe({
      next: () => {
        this.snackBar.open('Friend request sent successfully', 'Close', {
          duration: 3000
        });
      },
      error: (error) => {
        this.snackBar.open(
          error.error?.message || 'Failed to send friend request',
          'Close',
          { duration: 3000 }
        );
      }
    });
  }
} 