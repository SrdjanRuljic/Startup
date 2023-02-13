import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from 'src/app/shared/services/auth.service';

@Component({
  selector: 'app-group-conversation',
  templateUrl: './group-conversation.component.html',
  styleUrls: ['./group-conversation.component.scss'],
})
export class GroupConversationComponent implements OnInit {
  name: string = '';

  constructor(
    private _router: Router,
    private _route: ActivatedRoute,
    private _authService: AuthService
  ) {
    this._router.routeReuseStrategy.shouldReuseRoute = () => false;
  }

  ngOnInit() {
    this._route.params.subscribe((params) => {
      this.name = params['name'];
      if (this.name != '') {
        const token = this._authService.getToken();
      }
    });
  }

  goToInterlocutors() {
    this._router.navigate(['/interlocutors']);
  }
}
