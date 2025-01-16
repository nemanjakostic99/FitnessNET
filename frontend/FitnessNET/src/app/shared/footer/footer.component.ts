import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-footer',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <footer class="footer">
      <div class="footer-content">
        <div class="footer-section">
          <h4>FitnessNET</h4>
          <p>Your personal fitness companion</p>
        </div>
        <div class="footer-section">
          <h4>Quick Links</h4>
          <a routerLink="/main">Feed</a>
          <a routerLink="/diet">Diet</a>
          <a routerLink="/training">Training</a>
        </div>
        <div class="footer-section">
          <h4>Contact</h4>
          <p>Email: nkostic037&#64;gmail.com</p>
        </div>
      </div>
      <div class="footer-bottom">
        <p>&copy; 2024 FitnessNET. All rights reserved.</p>
      </div>
    </footer>
  `,
  styles: [`
    .footer {
      background-color: #333;
      color: rgba(255, 255, 255, 0.7);
      padding: 2rem 0 1rem 0;
      margin-top: auto;
    }

    .footer-content {
      max-width: 1200px;
      margin: 0 auto;
      display: flex;
      justify-content: space-around;
      padding: 0 2rem;
    }

    .footer-section {
      display: flex;
      flex-direction: column;
      gap: 0.5rem;

      h4 {
        color: white;
        margin-bottom: 1rem;
        opacity: 0.9;
      }

      a {
        color: rgba(255, 255, 255, 0.7);
        text-decoration: none;
        transition: color 0.2s;
        
        &:hover {
          color: white;
        }
      }

      p {
        opacity: 0.7;
      }
    }

    .footer-bottom {
      text-align: center;
      margin-top: 2rem;
      padding-top: 1rem;
      border-top: 1px solid rgba(255,255,255,0.1);
      opacity: 0.7;
    }
  `]
})
export class FooterComponent {}
