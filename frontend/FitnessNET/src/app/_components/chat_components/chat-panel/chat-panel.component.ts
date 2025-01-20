import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { UserService } from '../../../_services/user.service';
import { CommunityUser } from '../../../_models/communityUser.interface';
import { FriendshipService } from '../../../_services/friendship.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { FriendRequestDTO } from '../../../_models/friendRequestDTO';
import { ChatDialog } from '../../../_models/chatDialog.interface';

@Component({
  selector: 'app-chat-panel',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './chat-panel.component.html',
  styleUrls: ['./chat-panel.component.scss']
})
export class ChatPanelComponent {
  @Input() isOpen = false;
  activeTab: 'friends' | 'trainers' | 'requests' = 'friends';
  friends: CommunityUser[] = [];
  trainers: CommunityUser[] = [];
  friendRequests: FriendRequestDTO[] = [];
  
  currentPage = 1;
  pageSize = 10;
  totalPages = { friends: 0, trainers: 0, requests: 0 };
  searchTerm = '';
  isLoading = false;
  activeChats: ChatDialog[] = [];

  constructor(
    private userService: UserService,
    private friendshipService: FriendshipService,
    private snackBar: MatSnackBar
  ) {
    this.loadConnections();
  }

  loadConnections(append = false) {
    const params = {
      page: this.currentPage,
      pageSize: this.pageSize,
      searchTerm: this.searchTerm,
      isTrainer: null
    };

    this.isLoading = true;

    if (this.activeTab === 'friends') {
      this.friendshipService.searchFriends(params).subscribe(response => {
        this.friends = append ? [...this.friends, ...response.items] : response.items;
        this.totalPages.friends = response.totalPages;
        this.isLoading = false;
      });
    } else if (this.activeTab === 'trainers') {
      this.friendshipService.searchTrainers(params).subscribe(response => {
        this.trainers = append ? [...this.trainers, ...response.items] : response.items;
        this.totalPages.trainers = response.totalPages;
        this.isLoading = false;
      });
    } else {
      this.friendshipService.searchFriendRequests(params).subscribe(response => {
        this.friendRequests = append ? [...this.friendRequests, ...response.items] : response.items;
        this.totalPages.requests = response.totalPages;
        this.isLoading = false;
      });
    }
  }

  onTabChange(tab: 'friends' | 'trainers' | 'requests') {
    this.activeTab = tab;
    this.currentPage = 1;
    this.loadConnections();
  }

  loadMore() {
    if (this.isLoading) return;
    this.currentPage++;
    this.loadConnections(true);
  }

  onSearch(event: Event) {
    const target = event.target as HTMLInputElement;
    this.searchTerm = target.value;
    this.currentPage = 1;
    this.loadConnections();
  }

  acceptRequest(senderUsername: string) {
    this.friendshipService.acceptFriendRequest(senderUsername).subscribe({
      next: () => {
        this.snackBar.open('Friend request accepted', 'Close', {
          duration: 3000
        });
        this.loadConnections(); // Reload the lists
      },
      error: (error) => {
        this.snackBar.open(
          error.error?.message || 'Failed to accept request',
          'Close',
          { duration: 3000 }
        );
      }
    });
  }

  declineRequest(senderUsername: string) {
    this.friendshipService.deleteFriendRequest(senderUsername).subscribe({
      next: () => {
        this.snackBar.open('Friend request declined', 'Close', {
          duration: 3000
        });
        this.loadConnections(); // Reload the lists
      },
      error: (error) => {
        this.snackBar.open(
          error.error?.message || 'Failed to decline request',
          'Close',
          { duration: 3000 }
        );
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

  openChat(user: CommunityUser) {
    // Check if chat already exists
    const existingChat = this.activeChats.find(chat => chat.user.username === user.username);
    if (existingChat) {
      existingChat.isMinimized = false;
      return;
    }

    // Create new chat
    this.activeChats.push({
      user,
      isMinimized: false,
      messages: []
    });
  }

  toggleChatMinimize(chat: ChatDialog) {
    chat.isMinimized = !chat.isMinimized;
  }

  closeChat(chat: ChatDialog) {
    const index = this.activeChats.indexOf(chat);
    if (index > -1) {
      this.activeChats.splice(index, 1);
    }
  }

  removeFriend(username: string) {
    this.friendshipService.removeFriend(username).subscribe({
      next: () => {
        this.snackBar.open('Friend removed', 'Close', {
          duration: 3000
        });
        this.loadConnections();
      },
      error: (error) => {
        this.snackBar.open(
          error.error?.message || 'Failed to remove friend',
          'Close',
          { duration: 3000 }
        );
      }
    });
  }
} 