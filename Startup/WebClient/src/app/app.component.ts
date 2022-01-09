import { Component, OnDestroy } from '@angular/core';
import { NavigationStart, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { AuthService } from './shared/services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnDestroy {
  subscription: Subscription;

  constructor(private _router: Router, private _authService: AuthService) {
    this.subscription = _router.events.subscribe((event) => {
      if (event instanceof NavigationStart) {
        if (!_router.navigated) {
          this._authService.loadPermissions();
        }
      }
    });
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
