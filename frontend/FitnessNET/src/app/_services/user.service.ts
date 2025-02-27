import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { AuthService } from './auth.service';
import { PaginatedResponse } from '../_models/paginatedResponse.interface';
import { CommunityUser } from '../_models/communityUser.interface';

interface UserSearchParams {
  page: number;
  pageSize: number;
  searchTerm?: string;
  isTrainer: boolean | null;
}

@Injectable({
  providedIn: 'root'
})
export class UserService {
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

  getCurrentUser(): Observable<any> {
    return this.http.get(`${this.apiUrl}/users/me`, { headers: this.getHeaders() });
  }

  getProfile(): Observable<any> {
    return this.http.get(`${this.apiUrl}/users/profile`, { headers: this.getHeaders() });
  }

  getProfilePicture(): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/users/profile-picture`, { 
      headers: this.getHeaders(),
      responseType: 'blob'
    });
  }

  updateProfile(userData: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/users/update-profile`, userData, { headers: this.getHeaders() });
  }

  updateProfilePicture(file: File): Observable<any> {
    const formData = new FormData();
    formData.append('profilePicture', file);
    return this.http.put(`${this.apiUrl}/users/profile-picture`, formData, { headers: this.getHeaders() });
  }

  removeProfilePicture(): Observable<any> {
    return this.http.delete(`${this.apiUrl}/users/profile-picture`, { headers: this.getHeaders() });
  }

  searchUsers(params: UserSearchParams): Observable<PaginatedResponse<CommunityUser>> {
    const queryParams = new HttpParams()
      .set('page', params.page.toString())
      .set('pageSize', params.pageSize.toString())
      .set('searchTerm', params.searchTerm || '')
      .set('isTrainer', params.isTrainer === null ? '' : params.isTrainer.toString());

    return this.http.get<PaginatedResponse<CommunityUser>>(`${this.apiUrl}/users/searchUsers`, {
      headers: this.getHeaders(),
      params: queryParams
    });
  }
} 