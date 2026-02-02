import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavigationComponent } from './components/navigation/navigation.component';
import { FooterComponent } from './components/footer/footer.component';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NavigationComponent, FooterComponent, CommonModule, FormsModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class AppComponent implements OnInit {
  protected readonly title = 'my-projct';
  showNameModal = false;
  userName = '';
  hasAskedForName = false;

  ngOnInit(): void {
    // Check if we already have a name and have asked before
    const savedName = localStorage.getItem('userName');
    const hasAsked = localStorage.getItem('hasAskedForName');

    // Only set initial userName if already saved
    if (savedName) {
      this.userName = savedName;
      this.showNameModal = false;
    } else if (!hasAsked) {
      // First time visitor - show modal
      this.showNameModal = true;
      this.userName = '';
    } else {
      // User dismissed modal before
      this.showNameModal = false;
      this.userName = '';
    }
  }

  saveName(): void {
    if (this.userName.trim()) {
      localStorage.setItem('userName', this.userName);
      localStorage.setItem('hasAskedForName', 'true');
      this.showNameModal = false;
      // Emit event to update header
      window.dispatchEvent(new Event('userNameChanged'));
    }
  }
}