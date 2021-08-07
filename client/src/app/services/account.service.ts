import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private currentUserSource = new ReplaySubject<User>(1);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient) {}

  login(model: any) {
    return this.http.post<any>(this.baseUrl + 'account/login', model).pipe(
      map((user: User) => {
        this.setCurrentUser(user);
      })
    );
  }

  register(model: any) {
    return this.http.post<any>(this.baseUrl + 'account/register', model).pipe(
      map((user: User) => {
        this.setCurrentUser(user);
        return user;
      })
    );
  }

  setCurrentUser(user: User) {
    if (user) {
      localStorage.setItem('user', JSON.stringify(user));
      console.log(user);
      this.currentUserSource.next(user);
    }
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }
}
