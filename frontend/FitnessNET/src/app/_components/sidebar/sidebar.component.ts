import { NgFor } from '@angular/common';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-sidebar',
  imports: [NgFor],
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent implements OnInit {
  connections = [
    { name: 'Trainer John', profilePicture: 'assets/trainer-john.jpg' },
    { name: 'Jane Doe', profilePicture: 'assets/jane-doe.jpg' },
    { name: 'Trainer Anna', profilePicture: 'assets/trainer-anna.jpg' },
  ];

  defaultProfilePicture = 'assets/default-profile.png';

  constructor() {}

  ngOnInit(): void {}

  onConnect(user: any): void {
    alert(`Messaging ${user.name}`);
    // Implement actual messaging logic here
  }
}
