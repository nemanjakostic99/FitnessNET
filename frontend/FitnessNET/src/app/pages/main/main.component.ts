import { NgFor } from '@angular/common';
import { Component } from '@angular/core';

@Component({
  selector: 'app-main',
  imports: [NgFor],
  templateUrl: './main.component.html',
  styleUrl: './main.component.css'
})
export class MainComponent {
  posts = [
    { title: 'Workout of the Day', content: 'Try this new HIIT workout for maximum fat burn!' },
    { title: 'Healthy Recipe', content: 'Check out this low-calorie avocado toast recipe.' }
  ];
}
