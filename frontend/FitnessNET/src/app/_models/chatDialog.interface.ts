import { CommunityUser } from "./communityUser.interface";
import { Message } from "./message.interface";

export interface ChatDialog {
  user: CommunityUser;
  isMinimized: boolean;
  messages: Message[];
  currentPage: number;
  totalPages: number;
  isLoading: boolean;
} 