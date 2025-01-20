import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-chat-icon',
  standalone: true,
  imports: [CommonModule],
  template: `
    <button 
      class="chat-icon" 
      [class.active]="isOpen" 
      (click)="toggleChat()"
      aria-label="Toggle chat panel">
      <i class="fas fa-comments"></i>
    </button>
  `,
  styles: [`
    .chat-icon {
      position: fixed;
      bottom: 5rem;
      right: 2rem;
      width: 3.5rem;
      height: 3.5rem;
      border-radius: 50%;
      background: #007bff;
      border: none;
      color: white;
      font-size: 1.5rem;
      cursor: pointer;
      box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
      transition: all 0.3s ease;
      z-index: 9999;
      display: flex;
      align-items: center;
      justify-content: center;

      &:hover {
        transform: scale(1.1);
        background: #0056b3;
        box-shadow: 0 6px 16px rgba(0, 0, 0, 0.2);
      }

      &.active {
        background: #0056b3;
        box-shadow: 0 6px 16px rgba(0, 0, 0, 0.2);
      }
    }
  `]
})
export class ChatIconComponent {
  @Output() chatToggled = new EventEmitter<boolean>();
  isOpen = false;

  toggleChat(): void {
    this.isOpen = !this.isOpen;
    this.chatToggled.emit(this.isOpen);
  }
} 