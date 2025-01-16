import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-community',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="community-container">
      <header class="community-header">
        <h1>Community</h1>
        <div class="filters">
          <button 
            class="filter-btn" 
            [class.active]="activeFilter === 'all'"
            (click)="activeFilter = 'all'">
            All
          </button>
          <button 
            class="filter-btn" 
            [class.active]="activeFilter === 'trainers'"
            (click)="activeFilter = 'trainers'">
            Trainers
          </button>
          <button 
            class="filter-btn" 
            [class.active]="activeFilter === 'users'"
            (click)="activeFilter = 'users'">
            Users
          </button>
        </div>
        <div class="search-box">
          <input 
            type="text" 
            placeholder="Search by name, specialization, or interests..."
            class="search-input"
          >
        </div>
      </header>

      <div class="community-grid">
        <!-- Placeholder cards -->
        <div class="user-card" *ngFor="let i of [1,2,3,4,5,6,7,8]" [class.trainer-card]="i % 2 === 0">
          <div class="user-avatar"></div>
          <h3>User {{i}}</h3>
          <p class="user-type">{{i % 2 === 0 ? 'Trainer' : 'Member'}}</p>
          <ng-container *ngIf="i % 2 === 0">
            <div class="rating">
              <i *ngFor="let star of [1,2,3,4,5]" 
                 class="fas fa-star"
                 [class.filled]="star <= getRandomRating(i)">
              </i>
              <span class="rating-value">{{getRandomRating(i).toFixed(1)}}</span>
            </div>
            <p class="trainer-description">
              {{getTrainerDescription(i)}}
            </p>
          </ng-container>
          <p *ngIf="i % 2 !== 0" class="user-info">Fitness Enthusiast</p>
          <button class="connect-btn">Connect</button>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .community-container {
      padding: 2rem;
      max-width: 1200px;
      margin: 0 auto;
    }

    .community-header {
      margin-bottom: 2rem;
      text-align: center;

      h1 {
        margin-bottom: 1.5rem;
        color: #333;
      }
    }

    .filters {
      display: flex;
      justify-content: center;
      gap: 1rem;
      margin-bottom: 1.5rem;
    }

    .filter-btn {
      padding: 0.5rem 1.5rem;
      border: none;
      border-radius: 20px;
      background: #f5f5f5;
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
      max-width: 600px;
      margin: 0 auto;

      .search-input {
        width: 100%;
        padding: 0.75rem 1rem;
        border: 1px solid #ddd;
        border-radius: 4px;
        font-size: 1rem;

        &:focus {
          outline: none;
          border-color: #007bff;
        }
      }
    }

    .community-grid {
      display: grid;
      grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
      gap: 2rem;
      margin-top: 2rem;
    }

    .user-card {
      background: white;
      border-radius: 8px;
      padding: 1.5rem;
      text-align: center;
      box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
      transition: transform 0.2s ease;
      min-height: 300px;
      display: flex;
      flex-direction: column;
      justify-content: space-between;

      &:hover {
        transform: translateY(-5px);
      }

      .user-avatar {
        width: 80px;
        height: 80px;
        border-radius: 50%;
        background: #dee2e6;
        margin: 0 auto 1rem;
      }

      h3 {
        margin: 0 0 0.5rem;
        color: #333;
      }

      .user-type {
        color: #007bff;
        font-weight: 500;
        margin: 0 0 0.5rem;
      }

      .user-info {
        color: #6c757d;
        margin: 0 0 1rem;
        font-size: 0.9rem;
      }

      .connect-btn {
        padding: 0.5rem 1.5rem;
        border: none;
        border-radius: 20px;
        background: #007bff;
        color: white;
        cursor: pointer;
        transition: all 0.2s ease;

        &:hover {
          background: #0056b3;
        }
      }
    }

    .trainer-card {
      background: #f8f9fa;
      
      .trainer-description {
        color: #495057;
        margin: 0.5rem 0 1rem;
        font-size: 0.9rem;
        line-height: 1.4;
        height: 3.8em;
        overflow: hidden;
        text-overflow: ellipsis;
        display: -webkit-box;
        -webkit-line-clamp: 3;
        -webkit-box-orient: vertical;
      }
    }

    .rating {
      margin: 0.5rem 0;
      color: #ffc107;
      
      .fa-star {
        color: #e4e5e9;
        margin: 0 2px;
        
        &.filled {
          color: #ffc107;
        }
      }
      
      .rating-value {
        margin-left: 0.5rem;
        color: #6c757d;
        font-weight: 500;
      }
    }
  `]
})
export class CommunityComponent {
  activeFilter: 'all' | 'trainers' | 'users' = 'all';
  
  private trainerDescriptions = [
    'Specialized in HIIT and strength training. Helping clients achieve their fitness goals.',
    'Certified nutrition coach and personal trainer. Focus on sustainable results.',
    'Expert in weight loss and muscle building. 10+ years experience.',
    'Yoga and mindfulness instructor. Holistic approach to fitness.',
    'Sports performance specialist. Former professional athlete.'
  ];

  getRandomRating(seed: number): number {
    // Using seed to keep the rating consistent for each trainer
    return 3.5 + (seed % 3) * 0.5;
  }

  getTrainerDescription(index: number): string {
    return this.trainerDescriptions[index % this.trainerDescriptions.length];
  }
} 