import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { AuthService } from './auth.service';
import { PaginatedResponse } from '../_models/paginatedResponse.interface';
import { CommunityUser } from '../_models/communityUser.interface';
import { FriendRequestDTO } from '../_models/friendRequestDTO';

interface UserSearchParams {
    page: number;
    pageSize: number;
    searchTerm?: string;
    isTrainer: boolean | null;
  }

@Injectable({
  providedIn: 'root'
})
export class FriendshipService {
  private apiUrl = environment.apiUrl;

  constructor(
    private http: HttpClient,
    private authService: AuthService
  ) { }

  private getHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }

  sendFriendRequest(receiverUsername: string): Observable<any> {
    const queryParams = new HttpParams()
      .set('receiverUsername', receiverUsername.toString());
    return this.http.post(`${this.apiUrl}/friendship/send-request`, {}, {
      headers: this.getHeaders(),
      params: queryParams
    });
  }

  acceptFriendRequest(senderUsername: string): Observable<any> {
    const queryParams = new HttpParams()
      .set('senderUsername', senderUsername.toString());
    return this.http.post(`${this.apiUrl}/friendship/accept-request`, {}, {
      headers: this.getHeaders(),
      params: queryParams
    });
  }

  deleteFriendRequest(senderUsername: string): Observable<any> {
    const queryParams = new HttpParams()
      .set('senderUsername', senderUsername.toString());
    return this.http.delete(`${this.apiUrl}/friendship/remove-request`, {
      headers: this.getHeaders(),
      params: queryParams
    });
  }

  removeFriend(friend: string): Observable<any> {
    const queryParams = new HttpParams()
      .set('friend', friend.toString());
    return this.http.delete(`${this.apiUrl}/friendship/remove-friend`, {
      headers: this.getHeaders(),
      params: queryParams
    });
  }

  
  searchFriends(params: UserSearchParams): Observable<PaginatedResponse<CommunityUser>> {
    const queryParams = new HttpParams()
      .set('page', params.page.toString())
      .set('pageSize', params.pageSize.toString())
      .set('searchTerm', params.searchTerm || '')
      .set('isTrainer', false);

    return this.http.get<PaginatedResponse<CommunityUser>>(`${this.apiUrl}/friendship/SearchFriends`, {
      headers: this.getHeaders(),
      params: queryParams
    });
  }

  searchTrainers(params: UserSearchParams): Observable<PaginatedResponse<CommunityUser>> {
    const queryParams = new HttpParams()
      .set('page', params.page.toString())
      .set('pageSize', params.pageSize.toString())
      .set('searchTerm', params.searchTerm || '')
      .set('isTrainer', false);

    return this.http.get<PaginatedResponse<CommunityUser>>(`${this.apiUrl}/friendship/SearchFriends`, {
      headers: this.getHeaders(),
      params: queryParams
    });
  }

  searchFriendRequests(params: UserSearchParams): Observable<PaginatedResponse<FriendRequestDTO>> {
    const queryParams = new HttpParams()
      .set('page', params.page.toString())
      .set('pageSize', params.pageSize.toString())
      .set('searchTerm', params.searchTerm || '')
      .set('isTrainer', false);

    return this.http.get<PaginatedResponse<CommunityUser>>(`${this.apiUrl}/friendship/searchRequests`, {
      headers: this.getHeaders(),
      params: queryParams
    });
  }

} 