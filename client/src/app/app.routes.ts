import { Routes } from '@angular/router';
import { VoteComponent } from './components/vote/vote.component';
import { CreateComponent } from './components/create/create.component';
import { ResultsComponent } from './components/results/results.component';
import { NavigationComponent } from './components/navigation/navigation.component';
import { HomeComponent } from './components/home/home.component';

export const routes: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  { path: 'navigation', component: NavigationComponent },
  { path: 'vote', component: VoteComponent },
  { path: 'create', component: CreateComponent },
  { path: 'results', component: ResultsComponent }
];


