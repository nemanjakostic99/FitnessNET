import { Routes } from '@angular/router';
import { MainComponent } from './_components/pages/main/main.component';
import { PersonalComponent } from './_components/pages/personal/personal.component';
import { DietComponent } from './_components/pages/diet/diet.component';
import { TrainingComponent } from './_components/pages/training/training.component';
import { CommunityComponent } from './_components/community/community.component';

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

