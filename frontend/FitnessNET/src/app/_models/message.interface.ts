export interface Message {
  senderUsername: string;
  receiverUsername: string;
  content: string;
  sentAt: string | Date;
} 