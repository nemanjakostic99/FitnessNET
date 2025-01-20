import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-chat-panel',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  template: `
    <div class="chat-panel" [class.open]="isOpen">
      <div class="chat-header">
        <h2>Messages</h2>
        <div class="tabs">
          <button 
            class="tab-btn" 
            [class.active]="activeTab === 'friends'"
            (click)="activeTab = 'friends'">
            Friends
          </button>
          <button 
            class="tab-btn"
            [class.active]="activeTab === 'trainers'"
            (click)="activeTab = 'trainers'">
            Trainers
          </button>
        </div>
        <div class="search-box">
          <input 
            type="text" 
            placeholder="Search in {{activeTab}}..." 
            class="search-input"
          >
        </div>
      </div>

      <div class="chat-content">
        <div class="user-list">
          <div *ngIf="activeTab === 'friends' && !hasConnections" class="empty-state">
            <i class="fas fa-user-friends empty-icon"></i>
            <p>No friends yet</p>
            <a routerLink="/community" class="find-btn">Find Friends</a>
          </div>
          <div *ngIf="activeTab === 'trainers' && !hasConnections" class="empty-state">
            <i class="fas fa-dumbbell empty-icon"></i>
            <p>No trainers yet</p>
            <a routerLink="/community" class="find-btn">Find Trainers</a>
          </div>
          <div class="user-item" *ngFor="let i of [1,2,3,4,5]">
            <div class="user-avatar"></div>
            <div class="user-info">
              <h3>User {{i}}</h3>
              <p>Status: Online</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .chat-panel {
      position: fixed;
      top: 64px;
      right: -400px;
      width: 400px;
      height: calc(100vh - 64px);
      background: white;
      box-shadow: -2px 0 10px rgba(0, 0, 0, 0.1);
      transition: right 0.3s ease;
      z-index: 999;
      display: flex;
      flex-direction: column;
      border-left: 1px solid #eee;

      &.open {
        right: 0;
      }
    }

    .chat-header {
      padding: 1rem;
      border-bottom: 1px solid #eee;

      h2 {
        margin: 0 0 1rem;
        color: #333;
      }
    }

    .tabs {
      display: flex;
      gap: 1rem;
      margin-bottom: 1rem;
      justify-content: center;
    }

    .tab-btn {
      padding: 0.5rem 1rem;
      border: none;
      background: #f5f5f5;
      border-radius: 20px;
      cursor: pointer;
      transition: all 0.2s ease;

      &:hover {
        background: #e5e5e5;
      }

      &.active {
        background: #007bff;
        color: white;
      }
    }

    .search-box {
      margin-bottom: 1rem;

      .search-input {
        width: 100%;
        padding: 0.75rem;
        border: 1px solid #ddd;
        border-radius: 4px;
        font-size: 1rem;

        &:focus {
          outline: none;
          border-color: #007bff;
        }
      }
    }

    .chat-content {
      flex: 1;
      overflow-y: auto;
      padding: 1rem;
    }

    .user-list {
      display: flex;
      flex-direction: column;
      gap: 1rem;
    }

    .user-item {
      display: flex;
      align-items: center;
      gap: 1rem;
      padding: 0.75rem;
      border-radius: 8px;
      background: #f8f9fa;
      cursor: pointer;
      transition: all 0.2s ease;

      &:hover {
        background: #e9ecef;
      }
    }

    .user-avatar {
      width: 40px;
      height: 40px;
      border-radius: 50%;
      background: #dee2e6;
    }

    .user-info {
      h3 {
        margin: 0;
        font-size: 1rem;
        color: #333;
      }

      p {
        margin: 0;
        font-size: 0.875rem;
        color: #6c757d;
      }
    }

    .empty-state {
      display: flex;
      flex-direction: column;
      align-items: center;
      padding: 2rem;
      text-align: center;
      color: #6c757d;

      .empty-icon {
        font-size: 3rem;
        margin-bottom: 1rem;
        color: #dee2e6;
      }

      p {
        margin: 0 0 1rem;
      }

      .find-btn {
        padding: 0.5rem 1rem;
        background: #007bff;
        color: white;
        text-decoration: none;
        border-radius: 20px;
        transition: all 0.2s ease;

        &:hover {
          background: #0056b3;
          transform: translateY(-1px);
        }
      }
    }
  `]
})
export class ChatPanelComponent {
  @Input() isOpen = false;
  activeTab: 'friends' | 'trainers' = 'friends';
  hasConnections = false; // This will be connected to a service later
} 