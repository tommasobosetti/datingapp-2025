import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private http = inject(HttpClient);

  baseurl = 'https://localhost:5001/api/';

  login(creds: any) {
    return this.http.post(this.baseurl + 'account/login', creds);
  }
}
