import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit, OnDestroy {
  userName: string | null = null;
  userInitial: string = '';

  ngOnInit(): void {
    this.updateUserInfo();
    // Listen for user name changes
    window.addEventListener('userNameChanged', this.onUserNameChanged.bind(this));
  }

  ngOnDestroy(): void {
    window.removeEventListener('userNameChanged', this.onUserNameChanged.bind(this));
  }

  private updateUserInfo(): void {
    this.userName = localStorage.getItem('userName');
    this.userInitial = this.userName ? this.userName.charAt(0).toUpperCase() : '';
  }

  private onUserNameChanged(): void {
    this.updateUserInfo();
  }
}
