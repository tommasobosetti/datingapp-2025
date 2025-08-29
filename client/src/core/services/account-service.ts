import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { User } from '../../types/user';
import { tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private http = inject(HttpClient);
  currentUser = signal<User | null>(null);

  baseurl = 'https://localhost:5001/api/';

  login(creds: any) {
    return this.http.post<User>(this.baseurl + 'account/login', creds).pipe(
      tap(user => {
        if (user) {
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUser.set(user);
        }
      })
    )
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUser.set(null);
  }
}
