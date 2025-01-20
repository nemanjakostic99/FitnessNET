import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChatIconComponent } from '../chat-icon/chat-icon.component';
import { ChatPanelComponent } from '../chat-panel/chat-panel.component';

@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [CommonModule, ChatIconComponent, ChatPanelComponent],
  template: `
    <app-chat-icon (chatToggled)="handleChatToggle($event)"></app-chat-icon>
    <app-chat-panel [isOpen]="isChatOpen"></app-chat-panel>
  `
})
export class ChatComponent {
  isChatOpen = false;

  handleChatToggle(isOpen: boolean): void {
    this.isChatOpen = isOpen;
  }
} 