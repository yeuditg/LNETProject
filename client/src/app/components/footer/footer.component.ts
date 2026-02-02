import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-footer',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.css']
})
export class FooterComponent implements OnInit, OnDestroy {
  userName: string | null = null;

  ngOnInit(): void {
    this.updateUserName();
    // Listen for user name changes
    window.addEventListener('userNameChanged', this.onUserNameChanged.bind(this));
  }

  ngOnDestroy(): void {
    window.removeEventListener('userNameChanged', this.onUserNameChanged.bind(this));
  }

  private updateUserName(): void {
    this.userName = localStorage.getItem('userName');
  }

  private onUserNameChanged(): void {
    this.updateUserName();
  }
}
