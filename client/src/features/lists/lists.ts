import { Component, inject, OnInit, signal } from '@angular/core';
import { LikesService } from '../../core/services/likes-service';
import { LikesParams, Member } from '../../types/member';
import { MemberCard } from "../members/member-card/member-card";
import { PaginatedResult } from '../../types/pagination';
import { Paginator } from "../../shared/paginator/paginator";

@Component({
  selector: 'app-lists',
  imports: [MemberCard, Paginator],
  templateUrl: './lists.html',
  styleUrl: './lists.css'
})
export class Lists implements OnInit {
  private likesService = inject(LikesService);
  protected paginatedMembers = signal<PaginatedResult<Member> | null>(null);
  protected likesParams = new LikesParams();
  protected members = signal<Member[]>([]);
  protected predicate = 'liked';

  tabs = [
    { label: 'Liked', value: 'liked' },
    { label: 'Liked me', value: 'likedBy' },
    { label: 'Mutual', value: 'mutual' },
  ];

  constructor() {

  }

  ngOnInit(): void {
    this.loadLikes();
  }

  onPageChange(event: { pageNumber: number, pageSize: number }) {
    this.likesParams.pageSize = event.pageSize;
    this.likesParams.pageNumber = event.pageNumber;
    this.loadLikes();
  }

  setPredicate(predicate: string) {
    if (this.predicate !== predicate) {
      this.predicate = predicate;
      this.loadLikes();
    }
  }

  loadLikes() {
    this.likesService.getLikes(this.predicate, this.likesParams).subscribe({
      next: members => this.paginatedMembers.set(members)
    });
  }
}
