import { CommunityUser } from "./communityUser.interface";

export interface ChatDialog {
  user: CommunityUser;
  isMinimized: boolean;
  messages: any[]; // Replace with proper message interface
} 