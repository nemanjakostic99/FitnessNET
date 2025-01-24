import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Message } from '../_models/message.interface';
import { PaginatedResponse } from '../_models/paginatedResponse.interface';
import { CommunityUser } from '../_models/communityUser.interface';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  private hubConnection: HubConnection;
  private messageReceived = new BehaviorSubject<Message | null>(null);

  constructor(private http: HttpClient, private authService: AuthService) {
    
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${environment.signalRapiUrl}`, {
        accessTokenFactory: () => localStorage.getItem('fitnessNetjwt') || '' // Ensure token is fetched here
      })
      .withAutomaticReconnect()
      .build();
    
    this.startConnection();
    this.addListeners();
  }

  private getHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }

  private startConnection() {
    this.hubConnection.start()
      .catch(err => console.error('Error while starting SignalR connection:', err));
  }

  private addListeners() {
    this.hubConnection.on('ReceiveMessage', (senderUsername: string, content: string) => {
      const message: Message = {
        senderUsername,
        receiverUsername: this.authService.getUsername() || '',
        content,
        sentAt: new Date()
      };
      this.messageReceived.next(message);
    });
  }

  getMessageReceived(): Observable<Message | null> {
    return this.messageReceived.asObservable();
  }

  sendMessage(receiverUsername: string, message: string): Promise<void> {
    const currentUser = this.authService.getUsername();
    if (!currentUser) {
      return Promise.reject('User not authenticated');
    }
    const localMessage: Message = {
      senderUsername: currentUser,
      receiverUsername: receiverUsername,
      content: message,
      sentAt: new Date()
    };
    this.messageReceived.next(localMessage);
    return this.hubConnection.invoke('SendMessage', receiverUsername, message);
  }

  getMessageHistory(user2: string, page: number, pageSize: number = 10): Observable<PaginatedResponse<Message>> {
    const queryParams = new HttpParams()
        .set('user2', user2)
        .set('page', page.toString())
        .set('pageSize', pageSize.toString());

    return this.http.get<PaginatedResponse<Message>>(`${environment.apiUrl}/messageHistory/searchMessageHistory`, {
      headers: this.getHeaders(),
      params: queryParams
    });
  }
} 