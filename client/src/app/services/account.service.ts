import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { User } from '../models/user';
import { PresenceService } from './presence.service';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private currentUserSource = new ReplaySubject<User>(1);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient, private presence: PresenceService) {}

  login(model: any) {
    return this.http.post<any>(this.baseUrl + 'account/login', model).pipe(
      map((user: User) => {
        this.setCurrentUser(user);
        this.presence.createHubConnection(user);
      })
    );
  }

  register(model: any) {
    return this.http.post<any>(this.baseUrl + 'account/register', model).pipe(
      map((user: User) => {
        this.setCurrentUser(user);
        this.presence.createHubConnection(user);
        return user;
      })
    );
  }

  setCurrentUser(user: User) {
    user.roles = [];
    const role = this.getRolesFromToken(user.token).role;
    Array.isArray(role) ? (user.roles = role) : user.roles.push(role);
    if (user) {
      localStorage.setItem('user', JSON.stringify(user));
      this.currentUserSource.next(user);
    }
  }

  getRolesFromToken(token) {
    const roles = JSON.parse(atob(token.split('.')[1]));
    return roles;
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
    this.presence.stopHubConnection();
  }
}
