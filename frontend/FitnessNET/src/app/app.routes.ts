import { Routes } from '@angular/router';
import { MainComponent } from './pages/main/main.component';
import { PersonalComponent } from './pages/personal/personal.component';
import { DietComponent } from './pages/diet/diet.component';
import { TrainingComponent } from './pages/training/training.component';
import { CommunityComponent } from './features/community/community.component';

export const routes: Routes = [
  { path: '', redirectTo: 'main', pathMatch: 'full' },
  { path: 'main', component: MainComponent },
  { path: 'personal', component: PersonalComponent },
  { path: 'diet', component: DietComponent },
  { path: 'training', component: TrainingComponent },
  {
    path: 'community',
    component: CommunityComponent,
    title: 'Community - FitnessNET'
  }
];

