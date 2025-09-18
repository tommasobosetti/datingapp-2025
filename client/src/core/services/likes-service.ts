import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { LikesParams, Member } from '../../types/member';
import { PaginatedResult } from '../../types/pagination';
import { tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LikesService {
  private baseUrl = environment.apiUrl;
  private http = inject(HttpClient);
  likeIds = signal<string[]>([]);

  toggleLike(targetMemberId: string) {
    return this.http.post(`${this.baseUrl}likes/${targetMemberId}`, {});
  }

  getLikes(predicate: string, likesParams: LikesParams) {
    let params = new HttpParams();

    params = params.append('pageNumber', likesParams.pageNumber);
    params = params.append('pageSize', likesParams.pageSize);

    return this.http.get<PaginatedResult<Member>>(this.baseUrl + 'likes?predicate=' + predicate, { params });
  }

  getLikeIds() {
    return this.http.get<string[]>(this.baseUrl + 'likes/list').subscribe({
      next: ids => {
        this.likeIds.set(ids);
      }
    })
  }

  clearLikeIds() {
    this.likeIds.set([]);
  }

}
